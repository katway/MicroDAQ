// File:    ModbusVariable.cs
// Author:  John
// Created: 2013��9��23�� 16:51:16
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
   /// ��ѡ�ģ�һ�����������;���,�����ֶ���д��
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