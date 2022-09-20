namespace RSPS.src.schedule
{
    public static class Scheduler
    {

        private static readonly List<Job> JobList = new List<Job>();
        private static readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = 2 };

        public static void AddJob(Job job)
        {
            JobList.Add(job);
        }

        public static void Start()
        {
            //AddSchedule(new SimpleTask("Simple Task", TimeSpan.FromMilliseconds(100)));
            Task.Run(() => { StartPerforming(); });
        }

        private static void StartPerforming()
        {
            Task.Run(() =>
            {
                Thread.BeginThreadAffinity();
                while (true)
                {
                    Parallel.ForEach(JobList, ParallelOptions, (Job? job) =>
                    {
                        if (job == null) {
                            return; 
                        }

                        bool success = job.Execute();
                        if (!success)
                        {
                            JobList.Remove(job);
                        }
                    });
                    Thread.Sleep(50);
                }
            });
        }
    
    }
}
