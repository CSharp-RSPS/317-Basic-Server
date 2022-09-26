using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update.flag
{
    public class EntityFlags
    {

        private readonly BitArray flag = new(Enum.GetValues(typeof(FlagType)).Length);

        public void UpdateFlag(FlagType flagType, bool value)
        {
            flag[(int)flagType] = value;
        }

        public bool IsFlagged(FlagType flagType)
        {
            return flag.Get((int)flagType);
        }

        public bool IsUpdateNeeded()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i <= 9 && flag[i] == true)
                {
                    return true;
                }
            }
            return false;
        }

        public void ResetUpdateFlags()
        {
            for (int i = 0; i < 9; i++)
            {
                flag[i] = false;
            }
        }


    }
}
