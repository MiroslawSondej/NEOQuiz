using System;

namespace NEO_Quiz
{
    public interface ITimerListener
    {
        void OnTimerTick(object sender, EventArgs e);
    }
}
