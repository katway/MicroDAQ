using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using OpcRcw.Da;
using OpcRcw.Comn;

namespace OpcOperate.Sync
{
    public class OPCServer
    {
        private OpcRcw.Da.IOPCServer ServerObj;//OPCServer对象
        private Object GroupObj = null; //OPC组对象，创建组对象时产生，添加Item时用
        private int pSvrGroupHandle = 0; //组的句柄，创建组对象时产生，释放内存时用
        private Dictionary<string, IOPCSyncIO> IOPCSyncObjs;          //异步操作对象，使用OPC DA2.0.
        private Dictionary<string, object> groups;                  //存放创建后的组对象；
        private const int LOCALE_ID = 0x804;//OPCServer返回文本的语言，0x407为英语实测返回的德语，0x804为中文，实测返回的是英语
        Int32 hClientGroup = 1;
        private bool isConnected = false;
        private bool isAddGroup = false;
        private bool isAddItems = false;
        private String objName;
        private int hClientItem;
        private string serverName;

        public OPCServer(string name)
        {
            objName = name;

        }
        public OPCServer()
        {
            groups = new Dictionary<string, object>();
            IOPCSyncObjs = new Dictionary<string, IOPCSyncIO>();
        }
        /// <summary>
        /// 建立和OPCServer的连接
        /// </summary>
        public bool Connect(string progID, string server)
        {
            try
            {
                Type svrComponenttyp = Type.GetTypeFromProgID(progID, server);
                ServerObj = (IOPCServer)Activator.CreateInstance(svrComponenttyp);
                serverName = progID;
                isConnected = true;
            }
            catch (System.Exception error)
            {
                isConnected = false;

                throw new Exception("创建OPCServer对象时出错。", error);
            }
            return isConnected;
        }
        /// <summary>
        /// 初始化组对象
        /// </summary>
        public bool AddGroup(string groupName) //返回值false为失败，true为成功
        {
            Int32 dwRequestedUpdateRate = 1000;
            Int32 hClientGroup = 1;
            Int32 pRevUpdaterate;
            float deadband = 0;
            int TimeBias = 0;
            GCHandle hTimeBias, hDeadband;
            hTimeBias = GCHandle.Alloc(TimeBias, GCHandleType.Pinned);
            hDeadband = GCHandle.Alloc(deadband, GCHandleType.Pinned);
            Guid iidRequiredInterface = typeof(IOPCItemMgt).GUID;
            try
            {
                ServerObj.AddGroup(groupName, 0,
                         dwRequestedUpdateRate, hClientGroup,
                         hTimeBias.AddrOfPinnedObject(), hDeadband.AddrOfPinnedObject(),
                         LOCALE_ID, out pSvrGroupHandle,
                         out pRevUpdaterate, ref iidRequiredInterface, out GroupObj);

                IOPCSyncObjs.Add(groupName, (IOPCSyncIO)GroupObj);
                groups.Add(groupName, GroupObj);
                //IOPCSyncObj = (IOPCSyncIO)GroupObj;//为组同步读写定义句柄
                isAddGroup = true;
            }
            catch (System.Exception error)
            {
                isAddGroup = false;
                throw new Exception("创建Group时出错。", error);
            }
            finally
            {
                if (hDeadband.IsAllocated) hDeadband.Free();
                if (hTimeBias.IsAllocated) hTimeBias.Free();
            }
            return isAddGroup;
        }

        /// </summary>
        /// <param name="items">添加读写的数据项，Items为读写对象数组</param>
        /// <returns>添加Items是否执行成功</returns>
        public bool AddItems(string groupName, string[] itemsName, int[] itemHandle)
        {
            string errText = string.Empty;
            IntPtr pResults = IntPtr.Zero;
            IntPtr pErrors = IntPtr.Zero;
            if (!isAddGroup)
            {
                if (!AddGroup(groupName)) return false;  //如果还没有没有添加组，先添加组
            }
            OPCITEMDEF[] ItemDefArray = new OPCITEMDEF[itemsName.Length];
            for (int i = 0; i < itemsName.Length; i++)
            {
                hClientItem = hClientItem + 1;
                ItemDefArray[i].szAccessPath = ""; // 可选的通道路径，对于Simatiic Net不需要。
                ItemDefArray[i].szItemID = itemsName[i]; // ItemID, seeabove
                ItemDefArray[i].bActive = 1; // item is active
                ItemDefArray[i].hClient = hClientItem; //client handle
                ItemDefArray[i].dwBlobSize = 0; // blob size
                ItemDefArray[i].pBlob = IntPtr.Zero; // pointer to blob
                ItemDefArray[i].vtRequestedDataType = GetRqstDataType(itemsName[i]); //Word数据类型
            }
            try
            {
                ((IOPCItemMgt)groups[groupName]).AddItems(ItemDefArray.Length, ItemDefArray, out  pResults, out pErrors);
                int[] errors = new int[ItemDefArray.Length];
                Marshal.Copy(pErrors, errors, 0, itemsName.Length);
                IntPtr pos = pResults;
                OPCITEMRESULT result;
                for (int i = 0; i < itemsName.Length; i++)
                {
                    if (errors[i] == 0)
                    {
                        result = (OPCITEMRESULT)Marshal.PtrToStructure(pos, typeof(OPCITEMRESULT));
                        itemHandle[i] = result.hServer;
                        pos = new IntPtr(pos.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                        isAddItems = true;
                    }
                    else
                    {
                        isAddItems = false;
                        //throw new Exception(string.Format("添加Item对象出错"));
                    }
                }
            }
            catch (System.Exception ex) // catch for add item  
            {
                isAddItems = false;
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
            return isAddItems;
        }
        /// <summary>
        /// 断开连接，调用需要捕捉异常
        /// </summary>
        /// 

        private short GetRqstDataTypeMatrikon(string itemID)//Matrikon的item数据类型判断
        {
            //System.Text.RegularExpressions.Regex R = new System.Text.RegularExpressions.Regex (",",

            short value = 0;
            int first = itemID.IndexOf(':');
            int last = itemID.LastIndexOf(':');
            int mark = itemID.IndexOf('[');
            string portion = itemID.Substring(first + 1, last - first - 1);
            portion = (string)System.Text.RegularExpressions.Regex.Match(portion, "^[A-Z]+").ToString();
            if (mark == -1)
            {
                switch (portion)
                {
                    case "B": value = 17; break;
                    case "X": value = 11; break;
                    case "W": value = 18; break;
                    case "REAL": value = 4; break;
                    case "BYTE": value = 17; break;
                    case "WORD": value = 18; break;
                    case "STRING": value = 8; break;
                    case "D": value = 3; break;
                    case "DWORD": value = 3; break;
                    default: throw new Exception("不被支持的数据类型");
                }
            }
            else
            {
                switch (portion)
                {
                    case "X": value = 0; break;
                    case "B": value = 8209; break;
                    case "W": value = 8210; break;
                    case "BYTE": value = 8209; break;
                    case "WORD": value = 8210; break;
                    case "REAL": value = 8196; break;
                    case "DWORD": value = 8211; break;
                    case "D": value = 8211; break;
                    default: throw new Exception("不被支持的数据类型");
                }
            }


            return value;
        }

        private short GetRqstDataTypeSiemens(string itemID)//西门子item数据类型判断
        {
            //System.Text.RegularExpressions.Regex R = new System.Text.RegularExpressions.Regex (",",
            string[] portions;
            short value = 0;
            portions = System.Text.RegularExpressions.Regex.Split(itemID, ",");
            portions[1] = (string)System.Text.RegularExpressions.Regex.Match(portions[1], "^[A-Z]+").ToString();

            if (portions.Length == 2)
            {
                switch (portions[1])
                {
                    case "B": value = 17; break;
                    case "X": value = 11; break;
                    case "W": value = 18; break;
                    case "REAL": value = 4; break;
                    case "BYTE": value = 17; break;
                    case "WORD": value = 18; break;
                    case "STRING": value = 8; break;
                    case "D": value = 3; break;
                    case "DWORD": value = 3; break;
                    default: throw new Exception("不被支持的数据类型");
                }
            }
            if (portions.Length == 3)
            {
                switch (portions[1])
                {
                    case "X": value = 0; break;
                    case "B": value = 8209; break;
                    case "W": value = 8210; break;
                    case "BYTE": value = 8209; break;
                    case "WORD": value = 8210; break;
                    case "REAL": value = 8196; break;
                    case "DWORD": value = 8211; break;
                    case "D": value = 8211; break;
                    default: throw new Exception("不被支持的数据类型");
                }
            }

            return value;
        }

        private short GetRqstDataType(string itemID)//判断是西门子的还是Matrikon
        {
            switch (serverName)
            {
                case "Matrikon.OPC.Universal":
                    return GetRqstDataTypeMatrikon(itemID);

                case "OPC.SimaticNET":
                    return GetRqstDataTypeSiemens(itemID);

                default: throw new Exception("不被支持的数据类型");
            }


        }

        public bool SyncWrite(string groupName, object[] values, int[] itemHandle) //由编程人员保证，所写数据和添加Item的数据说明相对应
        {
            IntPtr pErrors = IntPtr.Zero;
            bool isWrited = false;
            try
            {
                if (values.Length != itemHandle.Length)
                {
                    MessageBox.Show(string.Format("写入数据的个数与添加Item的数据说明长度不一致"), "写数据出错",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                IOPCSyncObjs[groupName].Write(values.Length, itemHandle, values, out pErrors);
                int[] errors = new int[values.Length];
                Marshal.Copy(pErrors, errors, 0, values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    if (errors[i] != 0)  //写数据不成功
                    {
                        String pstrError;   //需不需要释放？
                        ServerObj.GetErrorString(errors[i], LOCALE_ID, out pstrError);
                        MessageBox.Show(string.Format("写入第{0}个数据时出错:{1}", i, pstrError), "写数据出错",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isWrited = false;
                        break;
                    }
                    else
                    {
                        isWrited = true;
                    }
                }
            }
            catch (System.Exception error)
            {
                isWrited = false;
                MessageBox.Show(error.Message, "写数据出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }

            return isWrited;
        }

        public bool SyncRead(string groupName, object[] values, int[] itemHandle) //同步读，读的结果存放在values中，读成功返回true，失败返回false
        {
            IntPtr pItemValues = IntPtr.Zero;
            IntPtr pErrors = IntPtr.Zero;
            bool isRead = false;
            try
            {
                if (values.Length != itemHandle.Length)
                {
                    MessageBox.Show(string.Format("需要读出数据的个数与添加Item的数据说明长度不一致"), "读数据出错",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                IOPCSyncObjs[groupName].Read(OPCDATASOURCE.OPC_DS_DEVICE, itemHandle.Length, itemHandle, out pItemValues, out pErrors);
                int[] errors = new int[itemHandle.Length];
                Marshal.Copy(pErrors, errors, 0, itemHandle.Length);
                OPCITEMSTATE pItemState = new OPCITEMSTATE();
                for (int i = 0; i < itemHandle.Length; i++)
                {
                    if (errors[i] == 0)
                    {
                        pItemState = (OPCITEMSTATE)Marshal.PtrToStructure(pItemValues, typeof(OPCITEMSTATE));
                        values[i] = pItemState.vDataValue;
                        pItemValues = new IntPtr(pItemValues.ToInt32() + Marshal.SizeOf(typeof(OPCITEMSTATE)));
                        isRead = true;
                    }
                    else
                    {
                        String pstrError;   //需不需要释放？
                        ServerObj.GetErrorString(errors[i], LOCALE_ID, out pstrError);
                        throw new Exception(string.Format("读Item时出错,Item:{0}", i));
                        isRead = false;
                        break;
                    }
                }
            }
            catch (System.Exception error)
            {
                isRead = false;
                MessageBox.Show(error.Message, "Result-Read Items", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (pItemValues != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pItemValues);
                    pItemValues = IntPtr.Zero;
                }
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }
            return isRead;
        }


    }
}
