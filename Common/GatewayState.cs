
namespace MicroDAQ.Common
{
    public enum GatewayState
    {

        Intilized,
        ConfigLoading,
        ConfigLoad,
        Ready,
        Stopped = 0,
        Starting = 1,
        Started = 2,
        Running = 3,
        Pausing = 4,
        Paused = 5,
        Stopping = 6

    }
}