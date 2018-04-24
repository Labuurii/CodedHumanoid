using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GrpcHeaders
{
    const string SessionTokenHeader = "pl_session_token";

    public static void AddSessionToken(Metadata sink, Guid session_token)
    {
        sink.Add(new Metadata.Entry(SessionTokenHeader, session_token.ToString()));
    }
}