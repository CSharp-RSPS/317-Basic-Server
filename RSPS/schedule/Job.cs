using System.Diagnostics;

namespace RSPS.Schedule
{
    public class Job
    {

        protected readonly string Name;
        private readonly int Delay;
        private readonly Stopwatch Timer = new Stopwatch();
        private bool IsRunning = true;

        public Job(string name, TimeSpan delay)
        {
            Name = name;
            Delay = (int)delay.TotalMilliseconds;
        }

        public bool Execute()
        {
            if (!IsRunning)
            {
                Halt();
            }

            if (!Timer.IsRunning)
            {
                Timer.Start();
            }

            if (Timer.ElapsedMilliseconds >= Delay)
            {
                Perform();
                Timer.Reset();
            }

            return IsRunning;
        }

        protected virtual void Perform() { }
        protected virtual void Halt() { 
            IsRunning = false;
        }
        
    }
}
