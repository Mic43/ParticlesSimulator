using System;

namespace Core
{
    public interface ITickReceiver
    {
        void OnTick(TimeSpan elapsed);
    }
}