
namespace MicroDAQ.Gateway
{
    public enum RunningState
    {
        Intilized,
        ConfigLoading,
        ConfigLoad,
        Ready,
        Starting = 1,
        Started = 2,
        Running = 3 ,
        Pausing = 4,
        Stopping = 6,
        Stopped = 0
    }
}