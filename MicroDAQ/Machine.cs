using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using JonLibrary.OPC;
using System.Data;

namespace JonLibrary.OPC
{
    using System.Drawing;
    /// <summary>
    /// 模块编号：Machine[未完成]
    /// 作    用：各种设备的基类实现，其它设备应该继承该类
    /// 作    者：Jon
    /// 编写日期：2010-12-2
    /// 使用方法：
    /// 说    明：
    /// </summary>
    public abstract class Machine
    {
        public ConnectionState ConnectionState;


        public delegate void dgtStateChange();
        public event dgtStateChange StatusChange;

        protected AsyncPLC4 PLC;
        public Rectangle[] Shape;

        public int Position
        {
            get;
            protected set;
        }
        public string Name
        {
            get;
            set;
        }
        public string[] ItemStatus;
        public string[] ItemCtrl;
        protected const string GROUP_NAME_CTRL = "MachineCtrl";
        protected const string GROUP_NAME_STATE = "MachineState";

        public Machine()
        {
            ConnectionState = ConnectionState.Closed;
            PLC = new AsyncPLC4();
            PLC.DataChange += new AsyncPLC4.dgtDataChange(PLC_DataChange);
        }

        protected virtual void PLC_DataChange(string groupName, int[] item, object[] value, short[] Qualities)
        {
            bool r = true;
            foreach (short q in Qualities)
            {
                r &= (q >= 192) ? (true) : (false);
            }
            ConnectionState = (r) ? (ConnectionState.Open) : (ConnectionState.Closed);
        }
        internal protected virtual bool Connect(string OPCServerIP)
        {
            bool success = true;
            success &= PLC.Connect("OPC.SimaticNET", OPCServerIP);
            success &= PLC.AddGroup(GROUP_NAME_CTRL, 1, 0);
            success &= PLC.AddItems(GROUP_NAME_CTRL, ItemCtrl);
            success &= PLC.AddGroup(GROUP_NAME_STATE, 1, 0);
            success &= PLC.AddItems(GROUP_NAME_STATE, ItemStatus);
            PLC.SetState(GROUP_NAME_STATE, true);
            ConnectionState = (success) ? (ConnectionState.Open) : (ConnectionState.Closed);
            return success;
        }

        protected virtual void OnStatusChannge()
        {
            if (StatusChange != null)
            { StatusChange(); }
        }

    }
}
