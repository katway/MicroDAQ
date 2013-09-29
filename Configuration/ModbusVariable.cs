// File:    ModbusVariable.cs
// Author:  John
// Created: 2013年9月23日 16:51:16
// Purpose: Definition of Class ModbusVariable

using System;

public class ModbusVariable
{
   public long serialID;
   public string allias;
   public string name;
   public int regesiterType;
   public int regesiterAddress;
   public string dataType;
   /// <summary>
   /// 可选的，一般由数据类型决定,不可手动填写。
   /// </summary> 
   public int length;
   public string accessibility;
   public decimal value;
   public string enable;
   public int scanPeriod;
   public decimal minimum;
   public decimal maximum;
   public decimal originalValue;
   public int decimalPlaces;

}