using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace JiraTaskTimer.Time
{
    public class JiraStopWatch
    {
        // Interval for updating the worklog for this issue on jira
        private readonly DispatcherTimer serverUpdateTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 1, 0) };
        private readonly DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
        private DateTime timeSpanLog;
        private TimeSpan elapsedTime;
        private TimeSpan aggregateTime;

        public delegate void TimerTick(TimeSpan timeSpan);
        public delegate void ServerTick(string elapsedTimeFormatted);
        private event TimerTick TimerTickCallback;
        private event ServerTick ServerUpdateCallback;


        public void StartTimer(TimerTick timerTickCallback, ServerTick serverUpdateCallback)
        {
            TimerTickCallback = timerTickCallback;
            ServerUpdateCallback = serverUpdateCallback;

            // Start interval for sending worklogs to Jira
            serverUpdateTimer.Tick += OnServerUpdateTick;
            serverUpdateTimer.Start();
            // Get the current time, and start the timer
            timeSpanLog = DateTime.Now;
            timer.Tick += OnTimerTick;
            timer.Start();
        }


        public void StopTimer()
        {
            timer.Stop();
            TimerTickCallback = null;
            ServerUpdateCallback = null;
            serverUpdateTimer.Tick -= OnServerUpdateTick;
            timer.Tick -= OnTimerTick;
            aggregateTime = elapsedTime;
        }


        private void OnTimerTick(object sender, EventArgs eventArgs)
        {
            elapsedTime = (DateTime.Now - timeSpanLog) + aggregateTime;
            TimerTickCallback?.Invoke(elapsedTime);
        }


        private void OnServerUpdateTick(object sender, EventArgs e)
        {
            if(elapsedTime.Minutes < 1) return;
            var elapsedTimeFormatted = string.Empty;
            if (elapsedTime.Minutes > 0) elapsedTimeFormatted += elapsedTime.Minutes + "m";
            if (elapsedTime.Hours > 0) elapsedTimeFormatted += elapsedTime.Hours + "h";
            if (elapsedTime.Days > 0) elapsedTimeFormatted += elapsedTime.Days + "d";
            ServerUpdateCallback?.Invoke(elapsedTimeFormatted);
        }
    }
}
