namespace Simulator.Core
{
    public interface ITickReceiver
    {
        void OnTick(TimeSpan elapsed);
    }
}