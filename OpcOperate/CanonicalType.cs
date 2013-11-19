using System;
using System.Collections.Generic;
using System.Text;

namespace OpcOperate
{
    static class CanonicalType
    {

        private static short GetRqstDataTypeMatrikon(string itemID)//Matrikon的item数据类型判断
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
                    case "D": value = 19; break;
                    case "DWORD": value = 19; break;
                    case "SHORT": value = 2; break;
                    case "CHAR": value = 16; break;
                    default: throw new Exception(string.Format("无法解析项中的数据类型{0}", itemID));
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
                    case "SHORT": value = 8194; break;
                    default: throw new Exception(string.Format("无法解析项中的数据类型{0}", itemID));
                }
            }


            return value;
        }

        private static short GetRqstDataTypeSiemens(string itemID)//西门子item数据类型判断
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
                    case "D": value = 19; break;
                    case "DWORD": value = 19; break;
                    case "SHORT": value = 2; break;
                    case "CHAR": value = 16; break;
                    default: throw new Exception(string.Format("无法解析项中的数据类型{0}", itemID));
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
                    case "SHORT": value = 8194; break;
                    default: throw new Exception(string.Format("无法解析项中的数据类型{0}", itemID));
                }
            }

            return value;
        }

        public static short GetTypeCode(string itemID, string serverName)//判断是西门子的还是Matrikon
        {
            switch (serverName)
            {
                case "Matrikon.OPC.Universal":
                    return GetRqstDataTypeMatrikon(itemID);

                case "OPC.SimaticNet":
                    return GetRqstDataTypeSiemens(itemID);

                default: throw new Exception("无法支持的OPC服务类型");
            }
        }
    }
}
