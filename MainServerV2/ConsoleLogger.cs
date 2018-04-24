using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    public class ConsoleLogger : ILoggerBackend
    {
        public void Log(string msg)
        {
            Console.Write(msg);
        }
    }
}
