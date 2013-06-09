using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using OpcRcw.Da;
using System.Collections;

namespace JonLibrary.OPC
{
    class SynPLC
    {
       
        private IOPCServer pIOPCServer; // 服务器对象
        private Object pobjGroup1;
        private int nSvrGroupID;
        private IOPCSyncIO2 syncIO2 = null; //同步操作对象
        private IOPCGroupStateMgt grpstaMgt; //组状态管理对象
        private System.Collections.Hashtable groupsID = new Hashtable(11); //用于记录组名和组ID号
        private System.Collections.Hashtable hitemsID = new Hashtable(17); //用于记录项名和项ID号
        private Guid iidRequiredInterface;
        private int hClientGroup = 0; //客户组号
        private int hClientItem = 0;

        public bool Connect(string progID, string server)
        {

            bool success = true;
            Type svrComponenttyp;
            svrComponenttyp = Type.GetTypeFromProgID(progID, server);//OPCServer  
            pIOPCServer = (OpcRcw.Da.IOPCServer)Activator.CreateInstance(svrComponenttyp);//注册 
            try
            {
                
                pIOPCServer = (IOPCServer)System.Activator.CreateInstance(svrComponenttyp);
               
            }
            catch (System.Exception ex) //捕捉失败信息
            {
                success = false;
                throw new Exception("创建OPCServer对象时出错。", ex);
            }
            return true;
        }
        public bool AddGroup(string groupName, int bActive, int updateRate, out string error)
        {
            error = "";
            int dwLCID = 0x407; //本地语言为英语
            int pRevUpdateRate;
            int TimeBias = 0;
            float deadband = 0;
            GCHandle hTimeBias, hDeadband;
            hTimeBias = GCHandle.Alloc(TimeBias, GCHandleType.Pinned);
            hDeadband = GCHandle.Alloc(deadband, GCHandleType.Pinned);
            try
            {
                pIOPCServer.AddGroup(groupName, //组名
                 bActive,
                 updateRate,
                 hClientGroup,
                 hTimeBias.AddrOfPinnedObject(),
                 hDeadband.AddrOfPinnedObject(),
                 dwLCID,
                 out nSvrGroupID, //移去组时，用到的组ID号
                 out pRevUpdateRate, //返回组中的变量改变时的最短通知时间间隔
                 ref iidRequiredInterface,
                 out pobjGroup1); //指向要求的接口
                syncIO2 = (IOPCSyncIO2)pobjGroup1;
                hClientGroup = hClientGroup + 1;
                int groupID = nSvrGroupID;
                groupsID.Add(groupName, groupID);
            }
            catch (System.Exception err) //捕捉失败信息
            {
                error = "错误信息:" + err.Message;
            }
            finally
            {
                if (hDeadband.IsAllocated) hDeadband.Free();
            }
            if (error == "")
                return true;
            else
                return false;
        }
        public bool AddItems(string groupName,string[] itemsName,int[] itemsID)
           {
              bool success=true;
               OPCITEMDEF[] ItemDefArray=new OPCITEMDEF[itemsName.Length];
               for(int i=0;i<itemsName.Length;i++)
               {
                  hClientItem=hClientItem+1;
                  ItemDefArray[i].szAccessPath = ""; // 可选的通道路径，对于Simatiic Net不需要。
                  ItemDefArray[i].szItemID = itemsName[i]; // ItemID, seeabove
                  ItemDefArray[i].bActive = 1; // item is active
                  ItemDefArray[i].hClient = hClientItem; //client handle
                  ItemDefArray[i].dwBlobSize = 0; // blob size
                  ItemDefArray[i].pBlob = IntPtr.Zero; // pointer to blob
                  ItemDefArray[i].vtRequestedDataType = 2; //Word数据类型
               }
             //初始化输出参数
                  IntPtr pResults = IntPtr.Zero;
                  IntPtr pErrors = IntPtr.Zero;
              try
             {
                    // 添加项到组
                 ((IOPCItemMgt)pobjGroup1).AddItems(itemsName.Length, ItemDefArray, out
                      pResults,out pErrors);
                     int[] errors = new int[itemsName.Length];
                     Marshal.Copy(pErrors, errors, 0,itemsName.Length);
                     IntPtr pos = pResults;
                      for(int i=0;i<itemsName.Length;i++) //循环检查错误
                    {
                          if (errors[i] == 0)
                        {
                            OPCITEMRESULT result = (OPCITEMRESULT)Marshal.PtrToStructure(pos,typeof(OPCITEMRESULT));
                            itemsID[i] = result.hServer;
                             this.hitemsID.Add(itemsName[i],result.hServer);
                             pos = new IntPtr(pos.ToInt32() +
                             Marshal.SizeOf(typeof(OPCITEMRESULT)));
                          }
                          else
                        {
                               success=false;
                                  break;
                         }
                     }
               }
            catch (System.Exception err) // catch for error in adding items.
               {
                     success=false;
               }
            finally
              {
                    // 释放非托管内存
                      if(pResults != IntPtr.Zero)
              {
            Marshal.FreeCoTaskMem(pResults);
            pResults = IntPtr.Zero;
        }
               if(pErrors != IntPtr.Zero)
                 {
                       Marshal.FreeCoTaskMem(pErrors);
                        pErrors = IntPtr.Zero;
                 }
        }
                return success;
}
        public bool Write(string groupName, int[] itemID, object[] values)
        {
            bool success = true;
            IntPtr pErrors = IntPtr.Zero;
            if (syncIO2 != null)
            {
                try
                { //同步写入
                    syncIO2.Write(itemID.Length, itemID, values, out pErrors);
                    int[] errors = new int[itemID.Length];
                    Marshal.Copy(pErrors, errors, 0, itemID.Length);
                    for (int i = 0; i < itemID.Length; i++) //循环检查错误
                    {
                        if (errors[i] != 0)
                        {
                            pErrors = IntPtr.Zero;
                            success = false;
                        }
                    }
                }
                catch (System.Exception error)
                {
                    success = false;
                }
            }
            return success;
        }
        public bool Read(string groupName, int[] itemID, object[] result)
        {
            bool success = true;
            //指向非托管内存
            //指向非托管内存
            IntPtr pItemValues = IntPtr.Zero;
            IntPtr pErrors = IntPtr.Zero;
            if (syncIO2 != null)
            {
                try
                { //同步读取
                    syncIO2.Read(OPCDATASOURCE.OPC_DS_DEVICE, itemID.Length,
                    itemID, out pItemValues, out pErrors);
                    int[] errors = new int[itemID.Length];
                    Marshal.Copy(pErrors, errors, 0, itemID.Length);
                    OPCITEMSTATE[] pItemState = new OPCITEMSTATE[itemID.Length];
                    IntPtr pos = pItemValues;
                    for (int i = 0; i < itemID.Length; i++) //循环检查错误
                    {
                        if (errors[i] == 0)
                        {
                            //从非托管区封送数据到托管区
                            pItemState[i] =
                            (OPCITEMSTATE)Marshal.PtrToStructure(pos, typeof(OPCITEMSTATE));
                            pos = new IntPtr(pos.ToInt32() +
                            Marshal.SizeOf(typeof(OPCITEMSTATE)));
                            result[i] = pItemState[i].vDataValue;
                        }
                    }
                }
                catch (System.Exception error)
                {
                    return false;
                }
            }
            return success;
        }


    }
              
}


