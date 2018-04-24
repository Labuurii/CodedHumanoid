using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaHost
{
    internal struct PortAllocator
    {
        object mutex;
        int base_;
        bool[] port_info; //False == not_allocated

        internal static PortAllocator Create(ushort base_ , ushort top_)
        {
            if (top_ >= base_)
                throw new ArgumentException("top_ is not bigger than base");
            return new PortAllocator
            {
                mutex = new object(),
                base_ = base_,
                port_info = new bool[top_ - base_]
            };
        }

        internal int? Allocate()
        {
            lock(mutex)
            {
                for (var i = 0; i < port_info.Length; ++i)
                {
                    if(!port_info[i])
                    {
                        port_info[i] = true;
                        var port = base_ + i;
                        return port;
                    }
                }
            }

            return null;
        }

        internal void Release(int port)
        {
            var idx = port - base_;
            lock(mutex)
            {
                if (!port_info[idx])
                    Log.Warning("Tried to release port which is not allocated.");
                port_info[idx] = false;
            }
            
        }
    }
}
