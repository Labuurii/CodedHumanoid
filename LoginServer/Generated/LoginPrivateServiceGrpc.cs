// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: login_private_service.proto
#region Designer generated code

using System;
using System.Threading;
using System.Threading.Tasks;
using grpc = global::Grpc.Core;

namespace LoginServices {
  public static partial class LoginPrivateService
  {
    static readonly string __ServiceName = "login_services.LoginPrivateService";

    static readonly grpc::Marshaller<global::LoginServices.ConnectionAttempt> __Marshaller_ConnectionAttempt = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::LoginServices.ConnectionAttempt.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::LoginServices.PrivateEvent> __Marshaller_PrivateEvent = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::LoginServices.PrivateEvent.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::LoginServices.TakeOwnershipAttempt> __Marshaller_TakeOwnershipAttempt = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::LoginServices.TakeOwnershipAttempt.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::LoginServices.TakeOwnershipResult> __Marshaller_TakeOwnershipResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::LoginServices.TakeOwnershipResult.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::LoginServices.LeaveOwnershipAttempt> __Marshaller_LeaveOwnershipAttempt = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::LoginServices.LeaveOwnershipAttempt.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::LoginServices.LeaveOwnershipResult> __Marshaller_LeaveOwnershipResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::LoginServices.LeaveOwnershipResult.Parser.ParseFrom);

    static readonly grpc::Method<global::LoginServices.ConnectionAttempt, global::LoginServices.PrivateEvent> __Method_Connect = new grpc::Method<global::LoginServices.ConnectionAttempt, global::LoginServices.PrivateEvent>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "Connect",
        __Marshaller_ConnectionAttempt,
        __Marshaller_PrivateEvent);

    static readonly grpc::Method<global::LoginServices.TakeOwnershipAttempt, global::LoginServices.TakeOwnershipResult> __Method_TakeOwnership = new grpc::Method<global::LoginServices.TakeOwnershipAttempt, global::LoginServices.TakeOwnershipResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "TakeOwnership",
        __Marshaller_TakeOwnershipAttempt,
        __Marshaller_TakeOwnershipResult);

    static readonly grpc::Method<global::LoginServices.LeaveOwnershipAttempt, global::LoginServices.LeaveOwnershipResult> __Method_LeaveOwnership = new grpc::Method<global::LoginServices.LeaveOwnershipAttempt, global::LoginServices.LeaveOwnershipResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "LeaveOwnership",
        __Marshaller_LeaveOwnershipAttempt,
        __Marshaller_LeaveOwnershipResult);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::LoginServices.LoginPrivateServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of LoginPrivateService</summary>
    public abstract partial class LoginPrivateServiceBase
    {
      public virtual global::System.Threading.Tasks.Task Connect(global::LoginServices.ConnectionAttempt request, grpc::IServerStreamWriter<global::LoginServices.PrivateEvent> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::LoginServices.TakeOwnershipResult> TakeOwnership(global::LoginServices.TakeOwnershipAttempt request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::LoginServices.LeaveOwnershipResult> LeaveOwnership(global::LoginServices.LeaveOwnershipAttempt request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for LoginPrivateService</summary>
    public partial class LoginPrivateServiceClient : grpc::ClientBase<LoginPrivateServiceClient>
    {
      /// <summary>Creates a new client for LoginPrivateService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public LoginPrivateServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for LoginPrivateService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public LoginPrivateServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected LoginPrivateServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected LoginPrivateServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual grpc::AsyncServerStreamingCall<global::LoginServices.PrivateEvent> Connect(global::LoginServices.ConnectionAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return Connect(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::LoginServices.PrivateEvent> Connect(global::LoginServices.ConnectionAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_Connect, null, options, request);
      }
      public virtual global::LoginServices.TakeOwnershipResult TakeOwnership(global::LoginServices.TakeOwnershipAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return TakeOwnership(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::LoginServices.TakeOwnershipResult TakeOwnership(global::LoginServices.TakeOwnershipAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_TakeOwnership, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::LoginServices.TakeOwnershipResult> TakeOwnershipAsync(global::LoginServices.TakeOwnershipAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return TakeOwnershipAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::LoginServices.TakeOwnershipResult> TakeOwnershipAsync(global::LoginServices.TakeOwnershipAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_TakeOwnership, null, options, request);
      }
      public virtual global::LoginServices.LeaveOwnershipResult LeaveOwnership(global::LoginServices.LeaveOwnershipAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return LeaveOwnership(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::LoginServices.LeaveOwnershipResult LeaveOwnership(global::LoginServices.LeaveOwnershipAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_LeaveOwnership, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::LoginServices.LeaveOwnershipResult> LeaveOwnershipAsync(global::LoginServices.LeaveOwnershipAttempt request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return LeaveOwnershipAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::LoginServices.LeaveOwnershipResult> LeaveOwnershipAsync(global::LoginServices.LeaveOwnershipAttempt request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_LeaveOwnership, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override LoginPrivateServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new LoginPrivateServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(LoginPrivateServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Connect, serviceImpl.Connect)
          .AddMethod(__Method_TakeOwnership, serviceImpl.TakeOwnership)
          .AddMethod(__Method_LeaveOwnership, serviceImpl.LeaveOwnership).Build();
    }

  }
}
#endregion