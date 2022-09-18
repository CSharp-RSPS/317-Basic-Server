using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.schedule
{
    public class Schedule
    {

        protected readonly string Name;
        protected int Delay;
        private Stopwatch timer = new Stopwatch();
        protected bool isRunning = true;


        public Schedule(string name, TimeSpan delay)
        {
            Name = name;
            Delay = (int)delay.TotalMilliseconds;
        }

        public void Pulse()
        {
            if (!isRunning)
            {
                Cancel();
            }

            if (!timer.IsRunning)
            {
                timer.Start();
            }

            if (timer.ElapsedMilliseconds >= Delay)
            {
                Execute();
                timer.Reset();
            }
        }

        public virtual void Execute() { }
        public virtual void Cancel() { 
            
        }
        
    }
}
