using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    internal struct WorkList<T>
    {
        readonly List<T> data;
        readonly ConcurrentQueue<T> add_q, remove_q;

        internal List<T> List
        {
            get
            {
                return data;
            }
        }

        private WorkList(int capacity)
        {
            data = new List<T>(capacity);
            add_q = new ConcurrentQueue<T>();
            remove_q = new ConcurrentQueue<T>();
        }

        internal static WorkList<T> Create(int capacity = 0)
        {
            return new WorkList<T>(capacity);
        }

        internal void Add(T v)
        {
            add_q.Enqueue(v);
        }

        internal void Remove(T v)
        {
            remove_q.Enqueue(v);
        }

        internal void Update()
        {
            T v;
            while(remove_q.TryDequeue(out v))
            {
                data.Remove(v);
            }

            while (add_q.TryDequeue(out v))
            {
                data.Add(v);
            }
        }
    }
}
