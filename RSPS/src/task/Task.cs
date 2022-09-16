using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.task
{
    public class Task
    {

        public string Name { get; private set; }
        private double SleepTime { get; }
        private Stopwatch Timer { get; } = new Stopwatch();
        public bool IsCanceled { get; private set; } = false;
        private bool RunOnce { get; set; } = false;
        public TaskPriority Priority { get; private set; } = TaskPriority.LOW;

        private Action Action;

        public Task(string name, TimeSpan sleepTime)
        {
            Name = name;
            SleepTime = sleepTime.TotalMilliseconds;
        }

        public Task(string name, TimeSpan sleepTime, Action action)
        {
            Name = name;
            SleepTime = sleepTime.TotalMilliseconds;
            this.Action = action;
        }

        public Task AddMethod(Action action)
        {
            this.Action = action;
            return this;
        }

        public Task AddPriority(TaskPriority priority)
        {
            Priority = priority;
            return this;
        }

        public Task ExecuteOnce()
        {
            RunOnce = true;
            return this;
        }

        protected virtual void Execute()
        {

        }

        public void Process()
        {
            try
            {
                if (IsSleeping())
                {
                    return;
                }

                Action();
                Timer.Restart();

                if (RunOnce)
                {
                    Cancel();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Task: {0} has encountered an error", Name);
            }
        }

        private bool IsSleeping()
        {
            if (!Timer.IsRunning)
                Timer.Start();

            return Timer.ElapsedMilliseconds >= SleepTime;
        }

        protected void Cancel()
        {
            IsCanceled = true;
        }


    }
}
