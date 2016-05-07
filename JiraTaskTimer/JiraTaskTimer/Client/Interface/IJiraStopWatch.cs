using JiraTaskTimer.Time;

namespace JiraTaskTimer.Client.Interface
{
    public interface IJiraStopWatch
    {
        void StartTimer(JiraStopWatch.TimerTick timerTickCallback, JiraStopWatch.ServerTick serverUpdateCallback);
        void StopTimer();
    }
}