// File:    ModbusMaster.cs
// Author:  John
// Created: 2013Äê9ÔÂ23ÈÕ 16:51:16
// Purpose: Definition of Class ModbusMaster

using System;

public class ModbusMaster
{
   public long serialID;
   public string name;
   public string allias;
   public string type;
   public string enable;
   
   public System.Collections.Generic.List<ModbusSlave> modbusSlave;
   
   /// <summary>
   /// Property for collection of ModbusSlave
   /// </summary>
   /// <pdGenerated>Default opposite class collection property</pdGenerated>
   public System.Collections.Generic.List<ModbusSlave> ModbusSlave
   {
      get
      {
         if (modbusSlave == null)
            modbusSlave = new System.Collections.Generic.List<ModbusSlave>();
         return modbusSlave;
      }
      set
      {
         RemoveAllModbusSlave();
         if (value != null)
         {
            foreach (ModbusSlave oModbusSlave in value)
               AddModbusSlave(oModbusSlave);
         }
      }
   }
   
   /// <summary>
   /// Add a new ModbusSlave in the collection
   /// </summary>
   /// <pdGenerated>Default Add</pdGenerated>
   public void AddModbusSlave(ModbusSlave newModbusSlave)
   {
      if (newModbusSlave == null)
         return;
      if (this.modbusSlave == null)
         this.modbusSlave = new System.Collections.Generic.List<ModbusSlave>();
      if (!this.modbusSlave.Contains(newModbusSlave))
         this.modbusSlave.Add(newModbusSlave);
   }
   
   /// <summary>
   /// Remove an existing ModbusSlave from the collection
   /// </summary>
   /// <pdGenerated>Default Remove</pdGenerated>
   public void RemoveModbusSlave(ModbusSlave oldModbusSlave)
   {
      if (oldModbusSlave == null)
         return;
      if (this.modbusSlave != null)
         if (this.modbusSlave.Contains(oldModbusSlave))
            this.modbusSlave.Remove(oldModbusSlave);
   }
   
   /// <summary>
   /// Remove all instances of ModbusSlave from the collection
   /// </summary>
   /// <pdGenerated>Default removeAll</pdGenerated>
   public void RemoveAllModbusSlave()
   {
      if (modbusSlave != null)
         modbusSlave.Clear();
   }
   public SerialPort serialPort;

}