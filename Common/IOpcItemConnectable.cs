using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MicroDAQ.Common
{
    public interface IOpcItemConnectable
    {
        ConnectionState ConnectionState { get; set; }
        bool Connect(string OpcServerProgramID, string OPCServerIP);
    }
}
