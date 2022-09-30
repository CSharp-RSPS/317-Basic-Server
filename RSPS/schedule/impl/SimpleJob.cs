using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Schedule.Impl
{
    public class SimpleJob : Job
    {

        int x = 0;

        public SimpleJob(string name, TimeSpan delay) : base(name, delay)
        {
        }

        protected override void Perform()
        {
            for (int i = 100000; i < x; i++)
            {
                int prime = (i / 2) % 2;
            }
            x ++;
            if (x >= 10)
            {
                Halt();
                Console.WriteLine("Removing Job: {0}", Name);
            }
        }
    }
}
