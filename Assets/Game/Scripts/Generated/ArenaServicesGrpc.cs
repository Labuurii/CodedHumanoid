// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: arena_services.proto
#region Designer generated code

using System;
using System.Threading;
using System.Threading.Tasks;
using grpc = global::Grpc.Core;

namespace ArenaServices {
  public static partial class BaseService
  {
    static readonly string __ServiceName = "arena_services.BaseService";

    static readonly grpc::Marshaller<global::ArenaServices.BaseServerSubscriptionAttempt> __Marshaller_BaseServerSubscriptionAttempt = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ArenaServices.BaseServerSubscriptionAttempt.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ArenaServices.EventBase> __Marshaller_EventBase = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ArenaServices.EventBase.Parser.ParseFrom);

    static readonly grpc::Method<global::ArenaServices.BaseServerSubscriptionAttempt, global::ArenaServices.EventBase> __Method_Subscribe = new grpc::Method<global::ArenaServices.BaseServerSubscriptionAttempt, global::ArenaServices.EventBase>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "Subscribe",
        __Marshaller_BaseServerSubscriptionAttempt,
        __Marshaller_EventBase);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::ArenaServices.ArenaServicesReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of BaseService</summary>
    public abstract partial class BaseServiceBase
    {
      public virtual global::System.Threading.Tasks.Task Subscribe(global::ArenaServices.BaseServerSubscriptionAttempt request, grpc::IServerStreamWriter<global::ArenaServices.EventBase> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for BaseService</summary>
    public partial class BaseServiceClient : grpc::ClientBase<BaseServiceClient>
    {
      /// <summary>Creates a new client for BaseService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public BaseServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for BaseService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public BaseServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected BaseServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected BaseServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual grpc::AsyncServerStreamingCall<global::ArenaServices.EventBase> Subscribe(global::ArenaServices.BaseServerSubscriptionAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return Subscribe(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::ArenaServices.EventBase> Subscribe(global::ArenaServices.BaseServerSubscriptionAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_Subscribe, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override BaseServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new BaseServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(BaseServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Subscribe, serviceImpl.Subscribe).Build();
    }

  }
  public static partial class MatchMakerService
  {
    static readonly string __ServiceName = "arena_services.MatchMakerService";

    static readonly grpc::Marshaller<global::ArenaServices.QueueAttempt> __Marshaller_QueueAttempt = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ArenaServices.QueueAttempt.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ArenaServices.QueueStateMsg> __Marshaller_QueueStateMsg = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ArenaServices.QueueStateMsg.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ArenaServices.AnswerMatchAttempt> __Marshaller_AnswerMatchAttempt = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ArenaServices.AnswerMatchAttempt.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ArenaServices.LeaveQueueAttempt> __Marshaller_LeaveQueueAttempt = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ArenaServices.LeaveQueueAttempt.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ArenaServices.SubscriptionAttempt> __Marshaller_SubscriptionAttempt = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ArenaServices.SubscriptionAttempt.Parser.ParseFrom);

    static readonly grpc::Method<global::ArenaServices.QueueAttempt, global::ArenaServices.QueueStateMsg> __Method_Queue = new grpc::Method<global::ArenaServices.QueueAttempt, global::ArenaServices.QueueStateMsg>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Queue",
        __Marshaller_QueueAttempt,
        __Marshaller_QueueStateMsg);

    static readonly grpc::Method<global::ArenaServices.AnswerMatchAttempt, global::ArenaServices.QueueStateMsg> __Method_AnswerMatch = new grpc::Method<global::ArenaServices.AnswerMatchAttempt, global::ArenaServices.QueueStateMsg>(
        grpc::MethodType.Unary,
        __ServiceName,
        "AnswerMatch",
        __Marshaller_AnswerMatchAttempt,
        __Marshaller_QueueStateMsg);

    static readonly grpc::Method<global::ArenaServices.LeaveQueueAttempt, global::ArenaServices.QueueStateMsg> __Method_LeaveQueue = new grpc::Method<global::ArenaServices.LeaveQueueAttempt, global::ArenaServices.QueueStateMsg>(
        grpc::MethodType.Unary,
        __ServiceName,
        "LeaveQueue",
        __Marshaller_LeaveQueueAttempt,
        __Marshaller_QueueStateMsg);

    static readonly grpc::Method<global::ArenaServices.SubscriptionAttempt, global::ArenaServices.QueueStateMsg> __Method_Subscribe = new grpc::Method<global::ArenaServices.SubscriptionAttempt, global::ArenaServices.QueueStateMsg>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "Subscribe",
        __Marshaller_SubscriptionAttempt,
        __Marshaller_QueueStateMsg);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::ArenaServices.ArenaServicesReflection.Descriptor.Services[1]; }
    }

    /// <summary>Base class for server-side implementations of MatchMakerService</summary>
    public abstract partial class MatchMakerServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::ArenaServices.QueueStateMsg> Queue(global::ArenaServices.QueueAttempt request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::ArenaServices.QueueStateMsg> AnswerMatch(global::ArenaServices.AnswerMatchAttempt request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::ArenaServices.QueueStateMsg> LeaveQueue(global::ArenaServices.LeaveQueueAttempt request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task Subscribe(global::ArenaServices.SubscriptionAttempt request, grpc::IServerStreamWriter<global::ArenaServices.QueueStateMsg> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for MatchMakerService</summary>
    public partial class MatchMakerServiceClient : grpc::ClientBase<MatchMakerServiceClient>
    {
      /// <summary>Creates a new client for MatchMakerService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public MatchMakerServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for MatchMakerService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public MatchMakerServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected MatchMakerServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected MatchMakerServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::ArenaServices.QueueStateMsg Queue(global::ArenaServices.QueueAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return Queue(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::ArenaServices.QueueStateMsg Queue(global::ArenaServices.QueueAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Queue, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::ArenaServices.QueueStateMsg> QueueAsync(global::ArenaServices.QueueAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return QueueAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::ArenaServices.QueueStateMsg> QueueAsync(global::ArenaServices.QueueAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Queue, null, options, request);
      }
      public virtual global::ArenaServices.QueueStateMsg AnswerMatch(global::ArenaServices.AnswerMatchAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return AnswerMatch(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::ArenaServices.QueueStateMsg AnswerMatch(global::ArenaServices.AnswerMatchAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_AnswerMatch, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::ArenaServices.QueueStateMsg> AnswerMatchAsync(global::ArenaServices.AnswerMatchAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return AnswerMatchAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::ArenaServices.QueueStateMsg> AnswerMatchAsync(global::ArenaServices.AnswerMatchAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_AnswerMatch, null, options, request);
      }
      public virtual global::ArenaServices.QueueStateMsg LeaveQueue(global::ArenaServices.LeaveQueueAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return LeaveQueue(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::ArenaServices.QueueStateMsg LeaveQueue(global::ArenaServices.LeaveQueueAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_LeaveQueue, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::ArenaServices.QueueStateMsg> LeaveQueueAsync(global::ArenaServices.LeaveQueueAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return LeaveQueueAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::ArenaServices.QueueStateMsg> LeaveQueueAsync(global::ArenaServices.LeaveQueueAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_LeaveQueue, null, options, request);
      }
      public virtual grpc::AsyncServerStreamingCall<global::ArenaServices.QueueStateMsg> Subscribe(global::ArenaServices.SubscriptionAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return Subscribe(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::ArenaServices.QueueStateMsg> Subscribe(global::ArenaServices.SubscriptionAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_Subscribe, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override MatchMakerServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new MatchMakerServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(MatchMakerServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Queue, serviceImpl.Queue)
          .AddMethod(__Method_AnswerMatch, serviceImpl.AnswerMatch)
          .AddMethod(__Method_LeaveQueue, serviceImpl.LeaveQueue)
          .AddMethod(__Method_Subscribe, serviceImpl.Subscribe).Build();
    }

  }
  public static partial class ArenaBaseService
  {
    static readonly string __ServiceName = "arena_services.ArenaBaseService";

    static readonly grpc::Marshaller<global::ArenaServices.SubscriptionAttempt> __Marshaller_SubscriptionAttempt = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ArenaServices.SubscriptionAttempt.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ArenaServices.EventArena> __Marshaller_EventArena = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ArenaServices.EventArena.Parser.ParseFrom);

    static readonly grpc::Method<global::ArenaServices.SubscriptionAttempt, global::ArenaServices.EventArena> __Method_Subscribe = new grpc::Method<global::ArenaServices.SubscriptionAttempt, global::ArenaServices.EventArena>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "Subscribe",
        __Marshaller_SubscriptionAttempt,
        __Marshaller_EventArena);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::ArenaServices.ArenaServicesReflection.Descriptor.Services[2]; }
    }

    /// <summary>Base class for server-side implementations of ArenaBaseService</summary>
    public abstract partial class ArenaBaseServiceBase
    {
      public virtual global::System.Threading.Tasks.Task Subscribe(global::ArenaServices.SubscriptionAttempt request, grpc::IServerStreamWriter<global::ArenaServices.EventArena> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for ArenaBaseService</summary>
    public partial class ArenaBaseServiceClient : grpc::ClientBase<ArenaBaseServiceClient>
    {
      /// <summary>Creates a new client for ArenaBaseService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public ArenaBaseServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for ArenaBaseService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public ArenaBaseServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected ArenaBaseServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected ArenaBaseServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual grpc::AsyncServerStreamingCall<global::ArenaServices.EventArena> Subscribe(global::ArenaServices.SubscriptionAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return Subscribe(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::ArenaServices.EventArena> Subscribe(global::ArenaServices.SubscriptionAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_Subscribe, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override ArenaBaseServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new ArenaBaseServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(ArenaBaseServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Subscribe, serviceImpl.Subscribe).Build();
    }

  }
  public static partial class Arena3DService
  {
    static readonly string __ServiceName = "arena_services.Arena3DService";

    static readonly grpc::Marshaller<global::ArenaServices.SubscriptionAttempt> __Marshaller_SubscriptionAttempt = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ArenaServices.SubscriptionAttempt.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ArenaServices.Event3D> __Marshaller_Event3D = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ArenaServices.Event3D.Parser.ParseFrom);

    static readonly grpc::Method<global::ArenaServices.SubscriptionAttempt, global::ArenaServices.Event3D> __Method_Subscribe = new grpc::Method<global::ArenaServices.SubscriptionAttempt, global::ArenaServices.Event3D>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "Subscribe",
        __Marshaller_SubscriptionAttempt,
        __Marshaller_Event3D);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::ArenaServices.ArenaServicesReflection.Descriptor.Services[3]; }
    }

    /// <summary>Base class for server-side implementations of Arena3DService</summary>
    public abstract partial class Arena3DServiceBase
    {
      public virtual global::System.Threading.Tasks.Task Subscribe(global::ArenaServices.SubscriptionAttempt request, grpc::IServerStreamWriter<global::ArenaServices.Event3D> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for Arena3DService</summary>
    public partial class Arena3DServiceClient : grpc::ClientBase<Arena3DServiceClient>
    {
      /// <summary>Creates a new client for Arena3DService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public Arena3DServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Arena3DService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public Arena3DServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected Arena3DServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected Arena3DServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual grpc::AsyncServerStreamingCall<global::ArenaServices.Event3D> Subscribe(global::ArenaServices.SubscriptionAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return Subscribe(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::ArenaServices.Event3D> Subscribe(global::ArenaServices.SubscriptionAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_Subscribe, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override Arena3DServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new Arena3DServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(Arena3DServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Subscribe, serviceImpl.Subscribe).Build();
    }

  }
}
#endregion
