// File:    ModbusSlave.cs
// Author:  John
// Created: 2013Äê9ÔÂ23ÈÕ 16:51:16
// Purpose: Definition of Class ModbusSlave

using System;
namespace MicroDAQ.Configuration
{
    public class ModbusSlave
    {
        public long serialID;
        public string name;
        public string allias;
        public string type;
        public int slave;
        public string enable;

        public System.Collections.Generic.List<ModbusVariable> modbusVariable;

        /// <summary>
        /// Property for collection of ModbusVariable
        /// </summary>
        /// <pdGenerated>Default opposite class collection property</pdGenerated>
        public System.Collections.Generic.List<ModbusVariable> ModbusVariable
        {
            get
            {
                if (modbusVariable == null)
                    modbusVariable = new System.Collections.Generic.List<ModbusVariable>();
                return modbusVariable;
            }
            set
            {
                RemoveAllModbusVariable();
                if (value != null)
                {
                    foreach (ModbusVariable oModbusVariable in value)
                        AddModbusVariable(oModbusVariable);
                }
            }
        }

        /// <summary>
        /// Add a new ModbusVariable in the collection
        /// </summary>
        /// <pdGenerated>Default Add</pdGenerated>
        public void AddModbusVariable(ModbusVariable newModbusVariable)
        {
            if (newModbusVariable == null)
                return;
            if (this.modbusVariable == null)
                this.modbusVariable = new System.Collections.Generic.List<ModbusVariable>();
            if (!this.modbusVariable.Contains(newModbusVariable))
                this.modbusVariable.Add(newModbusVariable);
        }

        /// <summary>
        /// Remove an existing ModbusVariable from the collection
        /// </summary>
        /// <pdGenerated>Default Remove</pdGenerated>
        public void RemoveModbusVariable(ModbusVariable oldModbusVariable)
        {
            if (oldModbusVariable == null)
                return;
            if (this.modbusVariable != null)
                if (this.modbusVariable.Contains(oldModbusVariable))
                    this.modbusVariable.Remove(oldModbusVariable);
        }

        /// <summary>
        /// Remove all instances of ModbusVariable from the collection
        /// </summary>
        /// <pdGenerated>Default removeAll</pdGenerated>
        public void RemoveAllModbusVariable()
        {
            if (modbusVariable != null)
                modbusVariable.Clear();
        }
        public IPSetting iPSetting;

    }
}