using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.schedule.impl
{
    public class SimpleTask : Schedule
    {

        int x = 0;

        public SimpleTask(string name, TimeSpan delay) : base(name, delay)
        {
        }

        public override void Execute()
        {
            for (int i = 100000; i < x; i++)
            {
                int prime = (i / 2) % 2;
            }
            x = 0;
        }
    }
}
