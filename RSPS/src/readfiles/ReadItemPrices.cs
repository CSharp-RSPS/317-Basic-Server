using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.readfiles
{
    public class ReadItemPrices
    {

        public static void ReadPrices()
        {
            //FileStream fs = FileStream.Synchronized(new FileStream())
            string[] lines = File.ReadAllLines("./data/readfiles/prices.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.None);
                string itemId = parts[0];
                string price = parts[1];
                Console.WriteLine("ITem ID: {0}, Price: {1}", itemId, price);
            }
        }
    }
}
