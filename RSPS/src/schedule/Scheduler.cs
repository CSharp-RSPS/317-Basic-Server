using RSPS.src.schedule.impl;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RSPS.src.schedule
{
    public class Scheduler
    {

        private static readonly ConcurrentDictionary<Schedule, SchedulePriority> dictonary = new ConcurrentDictionary<Schedule, SchedulePriority>();
        private static readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = 4 };

        public static void AddSchedule(SchedulePriority priority, Schedule schedule)
        {
            dictonary.TryAdd(schedule, priority);
        }

        public static void StartTask()
        {
/*            for (int i = 0; i < 10000000; i++)
            {
                AddSchedule(SchedulePriority.HIGH, new SimpleTask("Simple Task " + i, TimeSpan.FromMilliseconds(600)));
            }*/
            Task.Run(() => { Pulse(); });
        }

        public static void Pulse()
        {
            while (true)
            {
                Parallel.ForEach(dictonary.Keys, ParallelOptions, (Schedule schedule) =>
                {
                    schedule.Pulse();
                });

                Thread.Sleep(25);
            }
        }
    
    }
}
