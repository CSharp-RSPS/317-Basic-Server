using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update
{
    public interface IUpdateProtocol<T> where T : Entity
    {

        public void Process();

    }
}
