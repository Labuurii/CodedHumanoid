using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    public struct AtomicData<T>
        where T : class
    {
        T data;

        internal delegate void Callback(T data);

        internal AtomicData(T data)
        {
            this.data = data;
        }

        internal void Access(Callback cb)
        {
            lock(data)
            {
                cb(data);
            }
        }
    }
}
