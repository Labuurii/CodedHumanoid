protoc --csharp_out="../LoginServer/Generated" --grpc_out="../LoginServer/Generated" ./login_public_service.proto --plugin=protoc-gen-grpc=./grpc_csharp_plugin.exe
protoc --csharp_out="../Assets/Game/Scripts/Generated" --grpc_out="../Assets/Game/Scripts/Generated" ./login_public_service.proto --plugin=protoc-gen-grpc=./grpc_csharp_plugin.exe

protoc --csharp_out="../LoginServer/Generated" --grpc_out="../LoginServer/Generated" ./login_private_service.proto --plugin=protoc-gen-grpc=./grpc_csharp_plugin.exe
protoc --csharp_out="../MainServer/Generated" --grpc_out="../MainServer/Generated" ./login_private_service.proto --plugin=protoc-gen-grpc=./grpc_csharp_plugin.exe