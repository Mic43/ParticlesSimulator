using System;

namespace ClassLibrary1
{
    public interface ITickReceiver
    {
        void OnTick(TimeSpan elapsed);
    }
}