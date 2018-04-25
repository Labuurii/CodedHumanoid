# IO.Swagger.Model.OrgsorgidprojectsprojectidbuildtargetsSettingsAdvancedUnityPlayerExporter
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**SceneList** | **List&lt;string&gt;** | A list of scenes to build overriding those specified in the Build Settings menu of your Unity project. | [optional] 
**BuildOptions** | **List&lt;string&gt;** | Unity Editor build options. Use BuildOptions.Development and BuildOptions.AllowDebugging to create a development build. | [optional] 
**Export** | **bool?** | Enable exporting a player from Unity (i.e. running BuildPipeline.BuildPlayer). In general, this should be true, unless you are doing something like an asset bundle only build or unit test only build without generating an actual build artifact. | [optional] [default to true]

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

