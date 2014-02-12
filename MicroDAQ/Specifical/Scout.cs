using System;
using System.Collections.Generic;
using System.Text;
using OpcOperate.Sync;

namespace MicroDAQ.Specifical
{

    /// <summary>
    /// 用于系统正式运行前，检测各项前提条件是否满足运行需要
    /// </summary>
    internal class Scout
    {
        Loader Loader;

        internal Scout(Loader loader)
        {
            Loader = loader;
        }
        internal bool CheckPLCsState()
        {
            bool success = true;

            foreach (var plc in Loader.Configurator.PlcsInfo)
            {
                success &= GetPLCState(plc);
            }

            return success;

        }


        private bool GetPLCState(PLCStationInformation plcInfo)
        {
            bool success = false;

            OpcOperate.Sync.OPCServer SyncOpc = Loader.SyncOpc;

            ///准备ItemHandle
            int[] connectionStateItemHandle = new int[1];

            ///开始读取
            SyncOpc.AddGroup("GetConnectionState");
            SyncOpc.AddItems("GetConnectionState",
                                        new string[] { plcInfo.ConnectionStateItem },
                                        connectionStateItemHandle
                                  );
            ///接收数据
            object[] value = null;
            SyncOpc.SyncRead("GetConnectionState", value, connectionStateItemHandle);

            ///填充到PLCStationInfomation结构中
            switch (this.Loader.Configurator.opcServerType)
            {
                case "SimaticNet":
                    for (int i = 0; i < value.Length; i++)
                        Loader.Configurator.PlcsInfo[i].ConnectionState = (ushort)value[i];
                    break;
                case "Matrikon":
                    for (int i = 0; i < value.Length; i++)
                        Loader.Configurator.PlcsInfo[i].Connected = (bool)value[i];
                    break;
                default:
                    throw new Exception("不支持的OPC服务器类型");

            }
            return success;

        }


        private bool GetPLCState()
        {
            bool success = false;

            OpcOperate.Sync.OPCServer SyncOpc = Loader.SyncOpc;
            ///生成读取连接状态的Item数组
            List<string> connectionStateItem = new List<string>();
            foreach (var plc in Loader.Configurator.PlcsInfo)
            {
                connectionStateItem.Add(plc.ConnectionStateItem);
            }
            ///准备ItemHandle
            int[] connectionStateItemHandle = new int[Loader.Configurator.PlcsInfo.Count];

            ///开始读取
            SyncOpc.AddGroup("GetConnectionState");
            SyncOpc.AddItems("GetConnectionState",
                                        connectionStateItem.ToArray(),
                                        connectionStateItemHandle
                                  );
            ///接收数据
            object[] value = null;
            SyncOpc.SyncRead("GetConnectionState", value, connectionStateItemHandle);

            ///填充到PLCStationInfomation结构中

            switch (this.Loader.Configurator.opcServerType)
            {
                case "SimaticNet":
                    for (int i = 0; i < value.Length; i++)
                        Loader.Configurator.PlcsInfo[i].ConnectionState = (ushort)value[i];
                    break;
                case "Mactrikon":
                    for (int i = 0; i < value.Length; i++)
                        Loader.Configurator.PlcsInfo[i].Connected = (bool)value[i];
                    break;
                default:
                    throw new Exception("不支持的OPC服务器类型");

            }


            return success;
        }


    }
}
