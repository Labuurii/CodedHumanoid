using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    internal class ThreadSafeSinglyLinkedList<T> : IEnumerable<T>
        where T : class
    {
        Node head = new Node();

        internal class Node
        {
            internal object mutex = new object();
            internal T data;
            internal Node next;
        }

        internal class Iterator : IEnumerator<T>
        {
            Node current;

            internal Iterator(Node current)
            {
                this.current = current ?? throw new ArgumentNullException(nameof(current));
            }

            public T Current
            {
                get
                {
                    lock(current.mutex)
                    {
                        return current.data;
                    }
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                current = null;
            }

            public bool MoveNext()
            {
                if (current == null)
                    return false;
                lock(current.mutex)
                {
                    if (current.next == null)
                        return false;
                    current = current.next;
                    return true;
                }
            }

            public void Reset()
            {
                throw new NotImplementedException();
                //Not resettable
            }
        }

        internal void Add(T data)
        {
            var node = new Node
            {
                mutex = new object(),
                data = data
            };

            var current = head;
            for(;;)
            {
                lock(current.mutex)
                {
                    if(current.next == null)
                    {
                        current.next = node;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// If the equals operator is overridden by <see cref="T"/> then you have to make sure it internally is threadsafe otherwise
        /// this method is racy.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal bool Contains<C>(Func<T, C, bool> comparator, C comparand)
        {
            var current = head;
            for(; ; )
            {
                lock(current.mutex)
                {
                    if (current.next == null)
                        break;
                    lock(current.next)
                    {
                        if (comparator(current.next.data, comparand))
                            return true;
                    }
                    current = current.next;
                }
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Iterator(head.next);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
