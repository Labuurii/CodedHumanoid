using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArenaHost
{
    public enum Severity
    {
        Normal,
        Success,
        Warning,
        Fatal
    }

    public static class Log
    {
        static readonly Thread worker = new Thread(write_logs_forever);
        static readonly ConcurrentQueue<LogDesc> queue = new ConcurrentQueue<LogDesc>();
        public static ILoggerBackend Backend;

        static Log()
        {
            worker.Name = "Logger";
            worker.IsBackground = true;
            worker.Start();
        }

        struct LogDesc
        {
            internal string type;
            internal string message;
            internal string file;
            internal string method;
            internal int line;
        }

        public static void Do(Severity severity, string msg, [CallerLineNumber] int line = 0, [CallerMemberName] string caller = null, [CallerFilePath] string file = null)
        {
            switch(severity)
            {
                case Severity.Fatal:
                    fatal_impl(msg, line, caller, file);
                    break;
                case Severity.Normal:
                    msg_impl(msg, line, caller, file);
                    break;
                case Severity.Success:
                    success_impl(msg, line, caller, file);
                    break;
                case Severity.Warning:
                    warning_impl(msg, line, caller, file);
                    break;
                default:
                    throw new Exception("Unhandled enum value " + severity);
            }
        }

        public static void Warning(string msg, [CallerLineNumber] int line = 0, [CallerMemberName] string caller = null, [CallerFilePath] string file = null)
        {
            warning_impl(msg, line, caller, file);
        }

        private static void warning_impl(string msg, int line, string caller, string file)
        {
            schedule_log("Warning", msg, line, caller, file);
        }

        public static void Success(string msg, [CallerLineNumber] int line = 0, [CallerMemberName] string caller = null, [CallerFilePath] string file = null)
        {
            success_impl(msg, line, caller, file);
        }

        private static void success_impl(string msg, int line, string caller, string file)
        {
            schedule_log("Success", msg, line, caller, file);
        }

        public static void Msg(string msg, [CallerLineNumber] int line = 0, [CallerMemberName] string caller = null, [CallerFilePath] string file = null)
        {
            msg_impl(msg, line, caller, file);
        }

        private static void msg_impl(string msg, int line, string caller, string file)
        {
            schedule_log("Msg", msg, line, caller, file);
        }

        public static void Fatal(string msg, [CallerLineNumber] int line = 0, [CallerMemberName] string caller = null, [CallerFilePath] string file = null)
        {
            fatal_impl(msg, line, caller, file);
        }

        private static void fatal_impl(string msg, int line, string caller, string file)
        {
            schedule_log("Fatal", msg, line, caller, file);
        }

        public static void Exception(Exception e, [CallerLineNumber] int line = 0, [CallerMemberName] string caller = null, [CallerFilePath] string file = null)
        {
            schedule_log("Exception", string.Format("'{0}' thrown with message '{1}' with callstack:\n'{2}'", e.GetType().Name, e.Message, e.StackTrace), line, caller, file);
        }

        private static void schedule_log(string type, string msg, int line, string caller, string file)
        {
            queue.Enqueue(new LogDesc
            {
                type = type,
                message = msg,
                line = line,
                file = file,
                method = caller
            });
        }

        private static void write_logs_forever()
        {
            try
            {
                var sb = new StringBuilder();
                for (; ; )
                {
                    Thread.Sleep(1000);

                    var local_backend = Backend;
                    if (local_backend == null)
                        continue;

                    LogDesc log;
                    while (queue.TryDequeue(out log))
                    {
                        sb.Append(log.type);
                        sb.Append(" ");
                        sb.Append(DateTime.Now);
                        sb.Append(": ");
                        sb.Append(log.message);
                        sb.Append("\n\n");
                        local_backend.Log(sb.ToString());
                        sb.Clear();
                    }
                }
            } catch(Exception e)
            {
                Debug.Assert(false, e.ToString());
            }
        }
    }
}
