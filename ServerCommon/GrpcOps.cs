using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    public static class GrpcOps
    {
        public static string GetHeaderString(ServerCallContext context, string header)
        {
            foreach (var entry in context.RequestHeaders)
            {
                if (entry.Key == header)
                {
                    return entry.Value;
                }
            }

            return string.Empty;
        }
    }
}
