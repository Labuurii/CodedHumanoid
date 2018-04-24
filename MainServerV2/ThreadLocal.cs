using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    internal static class ThreadLocal<T>
        where T : class, new()
    {
        static readonly System.Threading.ThreadLocal<T> store = new System.Threading.ThreadLocal<T>();

        internal static T Get()
        {
            var v = store.Value;
            if(v == null)
            {
                store.Value = v = new T();
            }
            return v;
        }
    }
}
