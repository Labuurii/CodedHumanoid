# IO.Swagger.Api.BuildtargetsApi

All URIs are relative to *https://localhost/api/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**AddBuildTarget**](BuildtargetsApi.md#addbuildtarget) | **POST** /orgs/{orgid}/projects/{projectid}/buildtargets | Create build target for a project
[**DeleteBuildTarget**](BuildtargetsApi.md#deletebuildtarget) | **DELETE** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid} | Delete build target
[**GetBuildTarget**](BuildtargetsApi.md#getbuildtarget) | **GET** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid} | Get a build target
[**GetBuildTargets**](BuildtargetsApi.md#getbuildtargets) | **GET** /orgs/{orgid}/projects/{projectid}/buildtargets | List all build targets for a project
[**GetBuildTargetsForOrg**](BuildtargetsApi.md#getbuildtargetsfororg) | **GET** /orgs/{orgid}/buildtargets | List all build targets for an org
[**GetEnvVariablesForBuildTarget**](BuildtargetsApi.md#getenvvariablesforbuildtarget) | **GET** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/envvars | Get environment variables
[**SetEnvVariablesForBuildTarget**](BuildtargetsApi.md#setenvvariablesforbuildtarget) | **PUT** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/envvars | Set environment variables
[**UpdateBuildTarget**](BuildtargetsApi.md#updatebuildtarget) | **PUT** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid} | Update build target details


<a name="addbuildtarget"></a>
# **AddBuildTarget**
> InlineResponse20012 AddBuildTarget (string orgid, string projectid, Options8 options)

Create build target for a project

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class AddBuildTargetExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildtargetsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var options = new Options8(); // Options8 | Options for build target create/update

            try
            {
                // Create build target for a project
                InlineResponse20012 result = apiInstance.AddBuildTarget(orgid, projectid, options);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildtargetsApi.AddBuildTarget: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **orgid** | **string**| Organization identifier | 
 **projectid** | **string**| Project identifier | 
 **options** | [**Options8**](Options8.md)| Options for build target create/update | 

### Return type

[**InlineResponse20012**](InlineResponse20012.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletebuildtarget"></a>
# **DeleteBuildTarget**
> string DeleteBuildTarget (string orgid, string projectid, string buildtargetid)

Delete build target

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteBuildTargetExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildtargetsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name

            try
            {
                // Delete build target
                string result = apiInstance.DeleteBuildTarget(orgid, projectid, buildtargetid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildtargetsApi.DeleteBuildTarget: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **orgid** | **string**| Organization identifier | 
 **projectid** | **string**| Project identifier | 
 **buildtargetid** | **string**| unique id auto-generated from the build target name | 

### Return type

**string**

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getbuildtarget"></a>
# **GetBuildTarget**
> InlineResponse20012 GetBuildTarget (string orgid, string projectid, string buildtargetid)

Get a build target

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetBuildTargetExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildtargetsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name

            try
            {
                // Get a build target
                InlineResponse20012 result = apiInstance.GetBuildTarget(orgid, projectid, buildtargetid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildtargetsApi.GetBuildTarget: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **orgid** | **string**| Organization identifier | 
 **projectid** | **string**| Project identifier | 
 **buildtargetid** | **string**| unique id auto-generated from the build target name | 

### Return type

[**InlineResponse20012**](InlineResponse20012.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getbuildtargets"></a>
# **GetBuildTargets**
> List<InlineResponse20012> GetBuildTargets (string orgid, string projectid, string include = null, bool? includeLastSuccess = null)

List all build targets for a project

Gets all configured build targets for a project, regardless of whether they are enabled. Add \"?include=settings,credentials\" as a query parameter to include the build target settings and credentials with the response. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetBuildTargetsExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildtargetsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var include = include_example;  // string | Extra fields to include in the response (optional) 
            var includeLastSuccess = true;  // bool? | Include last successful build (optional)  (default to false)

            try
            {
                // List all build targets for a project
                List&lt;InlineResponse20012&gt; result = apiInstance.GetBuildTargets(orgid, projectid, include, includeLastSuccess);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildtargetsApi.GetBuildTargets: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **orgid** | **string**| Organization identifier | 
 **projectid** | **string**| Project identifier | 
 **include** | **string**| Extra fields to include in the response | [optional] 
 **includeLastSuccess** | **bool?**| Include last successful build | [optional] [default to false]

### Return type

[**List<InlineResponse20012>**](InlineResponse20012.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getbuildtargetsfororg"></a>
# **GetBuildTargetsForOrg**
> List<InlineResponse20012> GetBuildTargetsForOrg (string orgid, string include = null, bool? includeLastSuccess = null)

List all build targets for an org

Gets all configured build targets for an org, regardless of whether they are enabled. Add \"?include=settings,credentials\" as a query parameter to include the build target settings and credentials with the response. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetBuildTargetsForOrgExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildtargetsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var include = include_example;  // string | Extra fields to include in the response (optional) 
            var includeLastSuccess = true;  // bool? | Include last successful build (optional)  (default to false)

            try
            {
                // List all build targets for an org
                List&lt;InlineResponse20012&gt; result = apiInstance.GetBuildTargetsForOrg(orgid, include, includeLastSuccess);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildtargetsApi.GetBuildTargetsForOrg: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **orgid** | **string**| Organization identifier | 
 **include** | **string**| Extra fields to include in the response | [optional] 
 **includeLastSuccess** | **bool?**| Include last successful build | [optional] [default to false]

### Return type

[**List<InlineResponse20012>**](InlineResponse20012.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getenvvariablesforbuildtarget"></a>
# **GetEnvVariablesForBuildTarget**
> Dictionary<string, string> GetEnvVariablesForBuildTarget (string orgid, string projectid, string buildtargetid)

Get environment variables

Get all configured environment variables for a given build target

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetEnvVariablesForBuildTargetExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildtargetsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name

            try
            {
                // Get environment variables
                Dictionary&lt;string, string&gt; result = apiInstance.GetEnvVariablesForBuildTarget(orgid, projectid, buildtargetid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildtargetsApi.GetEnvVariablesForBuildTarget: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **orgid** | **string**| Organization identifier | 
 **projectid** | **string**| Project identifier | 
 **buildtargetid** | **string**| unique id auto-generated from the build target name | 

### Return type

**Dictionary<string, string>**

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="setenvvariablesforbuildtarget"></a>
# **SetEnvVariablesForBuildTarget**
> Dictionary<string, string> SetEnvVariablesForBuildTarget (string orgid, string projectid, string buildtargetid, Object envvars)

Set environment variables

Set all configured environment variables for a given build target

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class SetEnvVariablesForBuildTargetExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildtargetsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var envvars = ;  // Object | Environment variables

            try
            {
                // Set environment variables
                Dictionary&lt;string, string&gt; result = apiInstance.SetEnvVariablesForBuildTarget(orgid, projectid, buildtargetid, envvars);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildtargetsApi.SetEnvVariablesForBuildTarget: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **orgid** | **string**| Organization identifier | 
 **projectid** | **string**| Project identifier | 
 **buildtargetid** | **string**| unique id auto-generated from the build target name | 
 **envvars** | **Object**| Environment variables | 

### Return type

**Dictionary<string, string>**

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatebuildtarget"></a>
# **UpdateBuildTarget**
> InlineResponse20012 UpdateBuildTarget (string orgid, string projectid, string buildtargetid, Options9 options)

Update build target details

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateBuildTargetExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildtargetsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var options = new Options9(); // Options9 | Options for build target create/update

            try
            {
                // Update build target details
                InlineResponse20012 result = apiInstance.UpdateBuildTarget(orgid, projectid, buildtargetid, options);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildtargetsApi.UpdateBuildTarget: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **orgid** | **string**| Organization identifier | 
 **projectid** | **string**| Project identifier | 
 **buildtargetid** | **string**| unique id auto-generated from the build target name | 
 **options** | [**Options9**](Options9.md)| Options for build target create/update | 

### Return type

[**InlineResponse20012**](InlineResponse20012.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

