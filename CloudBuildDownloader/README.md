Tool for downloading the next sucessful unity cloudbuild and then process according to debug or release procedure.

## Updating to new Rest API version
This tool makes use of Swagger for the interfacing with the Unity Online Services REST API.
Therefore run swagger cli on the unity_rest_api.json file and output it to UnityRestApi directory
to regenerate the Swagger interface. Eg:
>> java -jar D:/swagger/swagger-codegen-cli-2.3.1.jar generate -i https://build-api.cloud.unity3d.com/api/v1/api.json -l csharp -o ./UnityRestApi

To get the new swagger file it is documented under https://build-api.cloud.unity3d.com/docs/1.0.0/index.html#clients
on the unity rest api docs.