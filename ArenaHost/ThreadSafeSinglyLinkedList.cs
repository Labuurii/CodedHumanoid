using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaHost
{
    internal class ThreadSafeSinglyLinkedList<T> where T : class
    {
        Node head;

        internal class Node
        {
            internal object mutex = new object();
            internal T data;
            internal Node next;

            internal Node() { }
            internal Node(T data)
            {
                this.data = data;
            }
        }

        internal void Add(T data)
        {
            var node = new Node(data);
            var current = head;
            for(;;)
            {
                lock(current.mutex)
                {
                    if(current.next == null)
                    {
                        current.next = node;
                        break;
                    } else
                    {
                        current = current.next;
                    }
                }
            }
        }

        internal bool AddIfCountIsLessThan(T data, int max_count)
        {
            var node = new Node(data);
            var current = head;
            int count = 0;
            for (; ; )
            {
                lock (current.mutex)
                {
                    if (current.next == null)
                    {
                        current.next = node;
                        return true;
                    }
                    else
                    {
                        current = current.next;
                    }
                }

                if (++count >= max_count)
                    return false;
            }
        }

        internal void Remove(T data)
        {
            var current = head;
            for(; ; )
            {
                lock(current.mutex)
                {
                    if (current.next == null)
                        break;
                    lock(current.next) //Technically this lock is not required
                    {
                        if(current.next.data == data)
                        {
                            var node_to_remove = current.next;
                            var node_before = current;
                            var node_after = node_to_remove.next;
                            node_before.next = node_after;
                            return; //Success
                        }
                    }
                }
            }
        }

        internal int Count()
        {
            var current = head;
            int count = 0;
            for(; ; )
            {
                lock (current.mutex)
                {
                    if (current.next == null)
                        break;
                    current = current.next;
                }
                ++count;
            }

            return count;
        }
    }
}
