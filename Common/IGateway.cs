using System;
using System.Collections.Generic;
using System.Text;

namespace MicroDAQ.Common
{
    public interface IGateway : IDisposable
    {
        void Start();
        void Pause();
        void Continue();
        void Stop();
    }
}
