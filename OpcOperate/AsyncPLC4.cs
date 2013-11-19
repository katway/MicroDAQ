using System;
using System.Collections.Generic;
using System.Text;
using OpcRcw.Da;
using OpcRcw.Comn;
using System.Runtime.InteropServices;

namespace OpcOperate.ASync
{
    /// <summary>
    /// 模块编号：AsyncPLC4
    /// 作用：使用OPC方式和PLC进行通信
    /// 作者：John
    /// 编写日期：2010-11-5
    /// 使用方法：New > Connect > AddGroupu > AddItem > [SetState] > ... >ReleaseSvr。
    /// 说明：相对于CAsyncPLC2，此类提供了多组支持，可以多次添加不同名称的组；
    /// 但对于Item仍需要要一次完整添加，否则无法保证每个item客户编号的唯一性；
    /// item客户编号默认从0开始，且仅从0开始。
    /// </summary>
    /// <summary>
    /// Log编号：1
    /// 修改描述：
    /// 
    /// </summary>
    public class AsyncPLC4 : IOPCDataCallback
    {

        const int LocalID = 0x407;
        const int clientNum = 1;                                    //?客户应用程序编号？
        private IOPCServer svr;                                     //服务器对象，此版本仅为一个
        private IConnectionPointContainer connPointContainer;       //连接点容器
        private IConnectionPoint connPoint;                         //连接点；
        private Dictionary<string, IOPCAsyncIO2> asyncIO2;          //异步操作对象，使用OPC DA2.0.
        private Dictionary<string, IOPCGroupStateMgt> grpstaMgt;    //组状态管理对象，使用版本1.0；
        private Dictionary<string, object> groups;                  //存放创建后的组对象；
        private Dictionary<string, object> itmes;                   //存放项对象；
        private Dictionary<string, Int32> groupIDsFromSvr;          //存放由服务器返回的组ID;
        private Dictionary<string, Int32> groupIDsOnClient;         //客户指定的Group编号；
        private Dictionary<string, int> resultItemID;
        private Dictionary<string, string[]> groupItemsPair;
        private string serverName;
        private object tmpGroup = null;                             //临时组对象；
        //private int[] itemSvrHandle;                              //保存AddItem时返回的result.hServer；
        //private int[] svrGroupHandle;                             //保存AddGroup时返回的pSvrGroupHandle；
        private Int32 dwCookie = 0;                                 //建立回调时的Cookie;
        private int GroupNumOnClient = 0;                           //?客户组号计数;
        //private int ClientItem = 0;                               //?Item编号计数；
        private int IONumber = 0;                                   //读写操作计数；

        public delegate void dgtDataChange(string groupName, int[] item, object[] value, short[] Qualities);
        public delegate void dgtReadComplete(string groupName, int[] item, object[] value, short[] Qualities);
        public delegate void dgtWriteComplete();
        public event dgtDataChange DataChange;
        public event dgtReadComplete ReadComplete;
        public event dgtWriteComplete WriteComplete;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AsyncPLC4()
        {
            groups = new Dictionary<string, object>();
            itmes = new Dictionary<string, object>();
            groupIDsFromSvr = new Dictionary<string, int>();
            groupIDsOnClient = new Dictionary<string, int>();
            resultItemID = new Dictionary<string, int>();
            asyncIO2 = new Dictionary<string, IOPCAsyncIO2>();
            grpstaMgt = new Dictionary<string, IOPCGroupStateMgt>();
            groupItemsPair = new Dictionary<string, string[]>();

        }
        /// <summary>
        /// 建立和OPC-Server的连接
        /// </summary>
        /// <param name="progID">OPC服务器的程序ID</param>
        /// <param name="server">OPCServer的IP地址</param>
        /// <returns>建立连接是否成功</returns>
        public bool Connect(string progID, string server)
        {
            bool success = true;
            Type svrComponenttyp;
            serverName = progID;
            try
            {
                svrComponenttyp = Type.GetTypeFromProgID(progID, server);//OPCServer  
                svr = (OpcRcw.Da.IOPCServer)Activator.CreateInstance(svrComponenttyp);//注册 
            }
            catch (Exception ex)
            {
                success = false;
                throw new Exception("创建OPCServer对象时出错。", ex);
            }
            return success;
        }
        /// <summary>
        /// 销毁OPC服务器对象，使用于程序退出时。
        /// </summary>
        /// <returns></returns>
        public bool ReleaseSever()
        {
            bool success = true;
            try
            {
                if (dwCookie != 0)
                {
                    connPoint.Unadvise(dwCookie);
                    dwCookie = 0;
                }
                // Free unmanaged code  
                Marshal.ReleaseComObject(connPoint);
                connPoint = null;

                Marshal.ReleaseComObject(connPointContainer);
                connPointContainer = null;

                foreach (IOPCAsyncIO2 io in asyncIO2.Values)
                {
                    if (io != null)
                    {
                        Marshal.ReleaseComObject(io);
                    }
                }
                //遍历删除每个组
                foreach (KeyValuePair<string, Int32> entry in groupIDsFromSvr)
                {
                    svr.RemoveGroup(entry.Value, 0);
                }
                foreach (IOPCGroupStateMgt2 mgt in grpstaMgt.Values)
                {
                    if (mgt != null)
                    {
                        Marshal.ReleaseComObject(mgt);
                    }
                }
                grpstaMgt = null;

                if (tmpGroup != null)
                {
                    Marshal.ReleaseComObject(tmpGroup);
                    tmpGroup = null;
                }
                if (svr != null)
                {
                    Marshal.ReleaseComObject(svr);
                    svr = null;
                }
            }
            catch (Exception ex)
            {
                success = false;
                throw new Exception("销毁服务时出现错误。", ex);
            }
            return success;
        }
        /// <summary>
        /// 添加Group
        /// </summary>
        /// <param name="grpName">组名，区分大小写</param>
        /// <param name="bActive">是否激活</param>
        /// <param name="updateRate">更新速率，0为尽可能快</param>
        /// <returns></returns>
        public bool AddGroup(string grpName, int bActive, int updateRate)
        {
            bool success = true;
            groupIDsOnClient.Add(grpName, ++GroupNumOnClient);          //将编号+1并使用；

            int pRevUpdateRate;
            float deadband = 0;
            int TimeBias = 0;
            GCHandle hTimeBias, hDeadband;
            hTimeBias = GCHandle.Alloc(TimeBias, GCHandleType.Pinned);
            hDeadband = GCHandle.Alloc(deadband, GCHandleType.Pinned);
            Guid iidRequiredInterface = typeof(IOPCItemMgt).GUID;
            Int32 pSvrGroupHandle;
            try
            {
                svr.AddGroup(grpName, bActive, updateRate, GroupNumOnClient,
                                        hTimeBias.AddrOfPinnedObject(),
                                        hDeadband.AddrOfPinnedObject(),
                                        LocalID,
                                    out pSvrGroupHandle,
                                    out pRevUpdateRate,
                                    ref iidRequiredInterface,
                                    out tmpGroup);
                groups.Add(grpName, tmpGroup);
                groupIDsFromSvr.Add(grpName, pSvrGroupHandle);
                asyncIO2.Add(grpName, (IOPCAsyncIO2)tmpGroup);                              //是否每个group对应一个IOPCAsyncIO ？
                grpstaMgt.Add(grpName, (IOPCGroupStateMgt)tmpGroup);                        //是否每个group对应一个IOPCGroupStateMgt ？
                connPointContainer = (IConnectionPointContainer)tmpGroup;                   //是否每个group对应一个IConnectionPointContainer ？
                Guid iid = typeof(IOPCDataCallback).GUID;
                connPointContainer.FindConnectionPoint(ref iid, out connPoint);
                connPoint.Advise(this, out dwCookie);
            }
            catch (Exception ex)
            {
                success = false;
                throw new Exception("创建Group时出错。", ex);
            }
            finally
            {
                if (hDeadband.IsAllocated) hDeadband.Free();
                if (hTimeBias.IsAllocated) hTimeBias.Free();
            }
            return success;
        }

        /// <summary>
        /// 添加Item,不支持多次添加请注意。
        /// </summary>
        /// <param name="groupName">组名，区分大小写</param>
        /// <param name="itemsName">item字符串数组</param>
        /// <returns></returns>
        public bool AddItems(string groupName, string[] itemsName)
        {
            if (itemsName != null)
            {
                short[] dataType = new short[itemsName.Length];
                for (int i = 0; i < itemsName.Length; i++)
                {
                    dataType[i] = CanonicalType.GetTypeCode(itemsName[i], serverName);
                }
                return AddItems(groupName, itemsName, dataType);
            }
            else
            {
                return true;
            }
        }

        public bool AddItems(string groupName, string[] itemsName, short[] dataType)
        {
            bool success = true;
            OPCITEMDEF[] Items = new OPCITEMDEF[itemsName.Length];
            for (int i = 0; i < itemsName.Length; i++)
            {
                Items[i].szAccessPath = "";             // 可选的通道路径，对于Simatiic Net不需要。
                Items[i].szItemID = itemsName[i];       // ItemID, seeabove
                Items[i].bActive = 1;                   // item is active
                Items[i].hClient = i;                   //client handle
                Items[i].dwBlobSize = 0;                // blob size
                Items[i].pBlob = IntPtr.Zero;           // pointer to blob
                Items[i].vtRequestedDataType = dataType[i];// GetRqstDataType(itemsName[i]);// DataType[i];      //数据类型
            }
            groupItemsPair.Add(groupName, itemsName);
            //初始化输出参数
            IntPtr pResults = IntPtr.Zero;
            IntPtr pErrors = IntPtr.Zero;
            try
            {
                //是否可以使用一个ItemMgt字典？
                ((IOPCItemMgt)groups[groupName]).AddItems(itemsName.Length, Items, out pResults, out pErrors);
                int[] errors = new int[itemsName.Length];
                //ItemServerHandle = new int[itemsName.Length];
                IntPtr pos = pResults;
                Marshal.Copy(pErrors, errors, 0, itemsName.Length);
                for (int i = 0; i < itemsName.Length; i++) //循环检查错误
                {
                    if (errors[i] == 0)
                    {
                        OPCITEMRESULT result = (OPCITEMRESULT)Marshal.PtrToStructure(pos, typeof(OPCITEMRESULT));
                        //itemsID[i] = result.hServer;
                        resultItemID.Add(itemsName[i], result.hServer);
                        pos = new IntPtr(pos.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                    }
                    else
                    {
                        success = false;
                        break;
                    }
                }
            }
            catch (System.Exception ex) // catch for add items  
            {
                success = false;
                throw new Exception("添加Item时出错。", ex);
            }
            finally
            {
                // Free the memory  
                if (pResults != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pResults);
                    pResults = IntPtr.Zero;
                }
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }
            return success;
        }

        public virtual bool Read(string groupName)
        {
            int outid;
            return Read(groupName, groupItemsPair[groupName], out outid);
        }
        /// <summary>
        /// 读操作
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="items">item标识字符串数组</param>
        public virtual bool Read(string groupName, string[] items, out int transactionID)
        {
            bool success = true;
            int nCancelid;
            IONumber = ++IONumber % ushort.MaxValue;
            transactionID = IONumber;
            //根据itemName取得创建Item时返回的标号
            int[] itemIDs = new int[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                itemIDs[i] = resultItemID[items[i]];
            }
            IntPtr pErrors = IntPtr.Zero;
            if (asyncIO2[groupName] != null)
            {
                try
                {
                    asyncIO2[groupName].Read(itemIDs.Length, itemIDs, IONumber, out nCancelid, out pErrors);
                    int[] errors = new int[itemIDs.Length];
                    Marshal.Copy(pErrors, errors, 0, errors.Length);
                    for (int i = 0; i < items.Length; i++) //循环检查错误
                    {
                        if (errors[i] != 0)
                        {
                            success = false;
                            throw new Exception(string.Format("异步读Item时出错,Item:{0}", items[i]));
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    success = false;
                    throw new Exception("异步读Item时出错.", ex);
                }
            }
            else
            {
                success = false;
                throw new Exception("组名不存在!");
            }
            return success;
        }

        public virtual bool Write(string groupName, object[] values)
        {
            int outid;
            return Write(groupName, groupItemsPair[groupName], values, out outid);
        }
        /// <summary>
        /// 写操作
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="items">item数组</param>
        /// <param name="values">数值数组</param>
        /// <param name="transactionID">本次操作的流水号</param>
        /// <returns></returns>
        public virtual bool Write(string groupName, string[] items, object[] values, out int transactionID)
        {
            bool success = true;
            int nCancelid;
            IONumber = ++IONumber % ushort.MaxValue;
            transactionID = IONumber;
            //根据itemName取得创建Item时返回的标号
            int[] itemIDs = new int[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                itemIDs[i] = resultItemID[items[i]];
            }
            IntPtr pErrors = IntPtr.Zero;
            if (asyncIO2[groupName] != null)
            {
                try
                {
                    asyncIO2[groupName].Write(itemIDs.Length, itemIDs, values, IONumber, out nCancelid, out pErrors);
                    int[] errors = new int[itemIDs.Length];
                    Marshal.Copy(pErrors, errors, 0, errors.Length);
                    for (int i = 0; i < items.Length; i++) //循环检查错误
                    {
                        if (errors[i] != 0)
                        {
                            success = false;
                            throw new Exception(string.Format("写Item时出错,Item:{0}", items[i]));
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    success = false;
                    throw new Exception("写Item时出错.", ex);
                }
            }
            else
            {
                success = false;
                throw new Exception("组名不存在!");
            }
            return success;

        }



        /// <summary>
        /// 设置订阅
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="active">是否活动，不活动就不产生OnDataChange?</param>
        public virtual void SetState(string groupName, bool active)
        {
            IntPtr pRequestedUpdateRate = IntPtr.Zero;
            int nRevUpdateRate = 1000;
            IntPtr hClientGroup = IntPtr.Zero;
            IntPtr pTimeBias = IntPtr.Zero;
            IntPtr pDeadband = IntPtr.Zero;
            IntPtr pLCID = IntPtr.Zero;
            int nActive = 0;

            // activates or deactivates group according to checkbox status  
            GCHandle hActive = GCHandle.Alloc(nActive, GCHandleType.Pinned);
            hActive.Target = ((active) ? (1) : (0));
            try
            {
                grpstaMgt[groupName].SetState(pRequestedUpdateRate, out nRevUpdateRate,
                          hActive.AddrOfPinnedObject(), pTimeBias, pDeadband, pLCID,
                          hClientGroup);
            }
            catch (Exception ex)
            {
                throw new Exception("设置订阅时出错错误。", ex);
            }

            finally
            {
                hActive.Free();
            }
        }
        public virtual void OnReadComplete(Int32 dwTransid,                     //异步读完成  
                                            Int32 hGroup,
                                            Int32 hrMasterquality,
                                            Int32 hrMastererror,
                                            Int32 dwCount,
                                            int[] phClientItems,
                                            object[] pvValues,                  //值  
                                            short[] pwQualities,                //质量码  
                                            OpcRcw.Da.FILETIME[] pftTimeStamps, //事件戳  
                                            int[] pErrors)
        {
            string groupName = string.Empty;
            //取得组名
            if (groupIDsOnClient.ContainsValue(hGroup)) //检查是否是有效组号
            {
                foreach (KeyValuePair<string, Int32> entry in groupIDsOnClient)
                {
                    if (entry.Value == hGroup) { groupName = entry.Key; }
                }
                //在这里写代码
                if (ReadComplete != null)
                { ReadComplete(groupName, phClientItems, pvValues, pwQualities); }
            }
            //else
            //{
            //    throw new Exception("出现这个异常是因为无法为这个组号找到对应的组名,这是一个程序逻辑错误,请重新检查关于添加组时的程序逻辑,以及hGroup和GroupNumOnClient的含义和对应关系。");
            //}
        }
        public virtual void OnDataChange(Int32 dwTransid,  //订阅方式  
                                    Int32 hGroup,
                                    Int32 hrMasterquality,
                                    Int32 hrMastererror,
                                    Int32 dwCount,
                                    int[] phClientItems,
                                    object[] pvValues,
                                    short[] pwQualities,
                                    OpcRcw.Da.FILETIME[] pftTimeStamps,
                                    int[] pErrors)
        {
            string groupName = string.Empty;
            //取得组名
            if (groupIDsOnClient.ContainsValue(hGroup)) //检查是否是有效组号
            {
                foreach (KeyValuePair<string, Int32> entry in groupIDsOnClient)
                {
                    if (entry.Value == hGroup) { groupName = entry.Key; }
                }
                //在这里写代码
                if (DataChange != null)
                { DataChange(groupName, phClientItems, pvValues, pwQualities); }
            }
            //else
            //{
            //    throw new Exception("出现这个异常是因为无法为这个组号找到对应的组名,这是一个程序逻辑错误,请重新检查关于添加组时的程序逻辑,以及hGroup和GroupNumOnClient的含义和对应关系。");
            //}

        }

        public virtual void OnCancelComplete(System.Int32 dwTransid, System.Int32 hGroup) { }

        public virtual void OnWriteComplete(Int32 dwTransid,//写完成  
                                            Int32 hGroup,
                                            Int32 hrMastererr,
                                            Int32 dwCount,
                                            int[] pClienthandles,
                                            int[] pErrors)
        {
            if (WriteComplete != null)
                WriteComplete();
        }
  
    }
}
