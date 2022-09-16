using RSPS.src.task.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.task
{
    public static class TaskManager
    {

        private static readonly List<Task> Tasks = new List<Task>();
        private static Thread Thread = new Thread(ProcessTasks);
        private static ParallelOptions MainParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        private static ParallelOptions TaskParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 5 };

        private static void AddBasicTask(Task task)
        {
            Tasks.Add(task.AddPriority(TaskPriority.LOW));
        }

        private static void AddWorldTask(Task task)
        {
            Tasks.Add(task.AddPriority(TaskPriority.NORMAL));
        }

        private static void AddGlobalTask(Task task)
        {
            Tasks.Add(task.AddPriority(TaskPriority.HIGH));
        }

        public static void StartTaskManager()
        {
            //AddGlobalTask(new MovementTask("Global Movement Handler", TimeSpan.FromMilliseconds(600)));
            Thread.Start();
        }

        private static void ProcessTasks()
        {
            while(true)
            {
                for (int i = 0; i < Tasks.Count; i++)//cancel task first
                {
                    var task = Tasks[i];
                    if (task.IsCanceled)
                    {
                        Tasks.Remove(task);
                    }
                }

                Parallel.ForEach(Tasks, Program.TaskParallelOptions, task =>
                {
                    if (task.Priority == TaskPriority.HIGH)
                    {
                        task.Process();
                    }
                });

                Parallel.ForEach(Tasks, Program.TaskParallelOptions, task =>
                {
                    if (task.Priority == TaskPriority.NORMAL)
                    {
                        task.Process();
                    }
                });

                Parallel.ForEach(Tasks, Program.TaskParallelOptions, task =>
                {
                    if (task.Priority == TaskPriority.LOW)
                    {
                        task.Process();
                    }
                });
                Thread.Sleep(25);
            }
        }

    }
}
