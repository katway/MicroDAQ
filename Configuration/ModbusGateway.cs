// File:    ModbusGateway.cs
// Author:  John
// Created: 2013Äê9ÔÂ29ÈÕ 16:31:00
// Purpose: Definition of Class ModbusGateway

using System;
namespace MicroDAQ.Configuration
{
    public class ModbusGateway
    {
        public long serialID;
        public string name;
        public string allias;
        public string enable;

        public System.Collections.Generic.List<ModbusMaster> modbusMaster;

        /// <summary>
        /// Property for collection of ModbusMaster
        /// </summary>
        /// <pdGenerated>Default opposite class collection property</pdGenerated>
        public System.Collections.Generic.List<ModbusMaster> ModbusMaster
        {
            get
            {
                if (modbusMaster == null)
                    modbusMaster = new System.Collections.Generic.List<ModbusMaster>();
                return modbusMaster;
            }
            set
            {
                RemoveAllModbusMaster();
                if (value != null)
                {
                    foreach (ModbusMaster oModbusMaster in value)
                        AddModbusMaster(oModbusMaster);
                }
            }
        }

        /// <summary>
        /// Add a new ModbusMaster in the collection
        /// </summary>
        /// <pdGenerated>Default Add</pdGenerated>
        public void AddModbusMaster(ModbusMaster newModbusMaster)
        {
            if (newModbusMaster == null)
                return;
            if (this.modbusMaster == null)
                this.modbusMaster = new System.Collections.Generic.List<ModbusMaster>();
            if (!this.modbusMaster.Contains(newModbusMaster))
                this.modbusMaster.Add(newModbusMaster);
        }

        /// <summary>
        /// Remove an existing ModbusMaster from the collection
        /// </summary>
        /// <pdGenerated>Default Remove</pdGenerated>
        public void RemoveModbusMaster(ModbusMaster oldModbusMaster)
        {
            if (oldModbusMaster == null)
                return;
            if (this.modbusMaster != null)
                if (this.modbusMaster.Contains(oldModbusMaster))
                    this.modbusMaster.Remove(oldModbusMaster);
        }

        /// <summary>
        /// Remove all instances of ModbusMaster from the collection
        /// </summary>
        /// <pdGenerated>Default removeAll</pdGenerated>
        public void RemoveAllModbusMaster()
        {
            if (modbusMaster != null)
                modbusMaster.Clear();
        }

    }
}