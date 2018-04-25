# IO.Swagger.Api.BuildsApi

All URIs are relative to *https://localhost/api/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**BatchDeleteBuildArtifacts**](BuildsApi.md#batchdeletebuildartifacts) | **POST** /orgs/{orgid}/projects/{projectid}/artifacts/delete | Delete artifacts for a batch of builds
[**CancelAllBuilds**](BuildsApi.md#cancelallbuilds) | **DELETE** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds | Cancel all builds
[**CancelBuild**](BuildsApi.md#cancelbuild) | **DELETE** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds/{number} | Cancel build
[**CancelBuildsForOrg**](BuildsApi.md#cancelbuildsfororg) | **DELETE** /orgs/{orgid}/builds | Cancel builds for org
[**CreateShare**](BuildsApi.md#createshare) | **POST** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds/{number}/share | Create a new link to share a project
[**DeleteAllBuildArtifacts**](BuildsApi.md#deleteallbuildartifacts) | **DELETE** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds/artifacts | Delete all artifacts associated with all non-favorited builds for a specified buildtargetid (_all is allowed).
[**DeleteBuildArtifacts**](BuildsApi.md#deletebuildartifacts) | **DELETE** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds/{number}/artifacts | Delete all artifacts associated with a specific build
[**GetAuditLog**](BuildsApi.md#getauditlog) | **GET** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/auditlog | Get audit log
[**GetAuditLog_0**](BuildsApi.md#getauditlog_0) | **GET** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds/{number}/auditlog | Get audit log
[**GetBuild**](BuildsApi.md#getbuild) | **GET** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds/{number} | Build Status
[**GetBuildLog**](BuildsApi.md#getbuildlog) | **GET** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds/{number}/log | Get build log
[**GetBuilds**](BuildsApi.md#getbuilds) | **GET** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds | List all builds
[**GetBuildsForOrg**](BuildsApi.md#getbuildsfororg) | **GET** /orgs/{orgid}/builds | List all builds for org
[**GetShare**](BuildsApi.md#getshare) | **GET** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds/{number}/share | Get the share link
[**RevokeShare**](BuildsApi.md#revokeshare) | **DELETE** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds/{number}/share | Revoke a shared link
[**StartBuilds**](BuildsApi.md#startbuilds) | **POST** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds | Create new build
[**UpdateBuild**](BuildsApi.md#updatebuild) | **PUT** /orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds/{number} | Update build information


<a name="batchdeletebuildartifacts"></a>
# **BatchDeleteBuildArtifacts**
> string BatchDeleteBuildArtifacts (string orgid, string projectid, Options10 options)

Delete artifacts for a batch of builds

Delete all artifacts associated with the builds identified by the provided build target ids and build numbers. Builds marked as do not delete or that are currently building will be ignored. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class BatchDeleteBuildArtifactsExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var options = new Options10(); // Options10 | Options to specify what builds to delete

            try
            {
                // Delete artifacts for a batch of builds
                string result = apiInstance.BatchDeleteBuildArtifacts(orgid, projectid, options);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.BatchDeleteBuildArtifacts: " + e.Message );
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
 **options** | [**Options10**](Options10.md)| Options to specify what builds to delete | 

### Return type

**string**

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="cancelallbuilds"></a>
# **CancelAllBuilds**
> string CancelAllBuilds (string orgid, string projectid, string buildtargetid)

Cancel all builds

Cancel all builds in progress for this build target (or all targets, if '_all' is specified as the buildtargetid). Canceling an already finished build will do nothing and respond successfully. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class CancelAllBuildsExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name

            try
            {
                // Cancel all builds
                string result = apiInstance.CancelAllBuilds(orgid, projectid, buildtargetid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.CancelAllBuilds: " + e.Message );
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

<a name="cancelbuild"></a>
# **CancelBuild**
> string CancelBuild (string orgid, string projectid, string buildtargetid, string number)

Cancel build

Cancel a build in progress. Canceling an already finished build will do nothing and respond successfully. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class CancelBuildExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var number = number_example;  // string | Build number or in some cases _all

            try
            {
                // Cancel build
                string result = apiInstance.CancelBuild(orgid, projectid, buildtargetid, number);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.CancelBuild: " + e.Message );
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
 **number** | **string**| Build number or in some cases _all | 

### Return type

**string**

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="cancelbuildsfororg"></a>
# **CancelBuildsForOrg**
> string CancelBuildsForOrg (string orgid)

Cancel builds for org

Cancel all in progress builds for an organization. Canceling an already finished build will do nothing and respond successfully. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class CancelBuildsForOrgExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier

            try
            {
                // Cancel builds for org
                string result = apiInstance.CancelBuildsForOrg(orgid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.CancelBuildsForOrg: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **orgid** | **string**| Organization identifier | 

### Return type

**string**

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="createshare"></a>
# **CreateShare**
> InlineResponse20015 CreateShare (string orgid, string projectid, string buildtargetid, string number)

Create a new link to share a project

Create a new short link to share a project. If this is called when a share already exists, that share will be revoked and a new one created.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class CreateShareExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var number = number_example;  // string | Build number or in some cases _all

            try
            {
                // Create a new link to share a project
                InlineResponse20015 result = apiInstance.CreateShare(orgid, projectid, buildtargetid, number);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.CreateShare: " + e.Message );
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
 **number** | **string**| Build number or in some cases _all | 

### Return type

[**InlineResponse20015**](InlineResponse20015.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/x-www-form-urlencoded
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteallbuildartifacts"></a>
# **DeleteAllBuildArtifacts**
> string DeleteAllBuildArtifacts (string orgid, string projectid, string buildtargetid)

Delete all artifacts associated with all non-favorited builds for a specified buildtargetid (_all is allowed).

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteAllBuildArtifactsExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name

            try
            {
                // Delete all artifacts associated with all non-favorited builds for a specified buildtargetid (_all is allowed).
                string result = apiInstance.DeleteAllBuildArtifacts(orgid, projectid, buildtargetid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.DeleteAllBuildArtifacts: " + e.Message );
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

<a name="deletebuildartifacts"></a>
# **DeleteBuildArtifacts**
> string DeleteBuildArtifacts (string orgid, string projectid, string buildtargetid, string number)

Delete all artifacts associated with a specific build

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteBuildArtifactsExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var number = number_example;  // string | Build number or in some cases _all

            try
            {
                // Delete all artifacts associated with a specific build
                string result = apiInstance.DeleteBuildArtifacts(orgid, projectid, buildtargetid, number);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.DeleteBuildArtifacts: " + e.Message );
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
 **number** | **string**| Build number or in some cases _all | 

### Return type

**string**

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getauditlog"></a>
# **GetAuditLog**
> List<InlineResponse20011> GetAuditLog (string orgid, string projectid, string buildtargetid, decimal? perPage = null, decimal? page = null)

Get audit log

Retrieve a list of historical settings changes for this build target.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAuditLogExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var perPage = 8.14;  // decimal? | Number of audit log records to retrieve (optional)  (default to 25)
            var page = 8.14;  // decimal? | Skip to page number, based on per_page value (optional)  (default to 1)

            try
            {
                // Get audit log
                List&lt;InlineResponse20011&gt; result = apiInstance.GetAuditLog(orgid, projectid, buildtargetid, perPage, page);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.GetAuditLog: " + e.Message );
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
 **perPage** | **decimal?**| Number of audit log records to retrieve | [optional] [default to 25]
 **page** | **decimal?**| Skip to page number, based on per_page value | [optional] [default to 1]

### Return type

[**List<InlineResponse20011>**](InlineResponse20011.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getauditlog_0"></a>
# **GetAuditLog_0**
> List<InlineResponse20011> GetAuditLog_0 (string orgid, string projectid, string buildtargetid, string number, decimal? perPage = null, decimal? page = null)

Get audit log

Retrieve a list of settings changes between the last and current build.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAuditLog_0Example
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var number = number_example;  // string | Build number or in some cases _all
            var perPage = 8.14;  // decimal? | Number of audit log records to retrieve (optional)  (default to 25)
            var page = 8.14;  // decimal? | Skip to page number, based on per_page value (optional)  (default to 1)

            try
            {
                // Get audit log
                List&lt;InlineResponse20011&gt; result = apiInstance.GetAuditLog_0(orgid, projectid, buildtargetid, number, perPage, page);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.GetAuditLog_0: " + e.Message );
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
 **number** | **string**| Build number or in some cases _all | 
 **perPage** | **decimal?**| Number of audit log records to retrieve | [optional] [default to 25]
 **page** | **decimal?**| Skip to page number, based on per_page value | [optional] [default to 1]

### Return type

[**List<InlineResponse20011>**](InlineResponse20011.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getbuild"></a>
# **GetBuild**
> OrgsorgidprojectsprojectidbuildtargetsBuilds GetBuild (string orgid, string projectid, string buildtargetid, string number, string include = null)

Build Status

Retrieve information for a specific build. A Build resource contains information related to a build attempt for a build target, including the build number, changeset, build times, and other pertinent data. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetBuildExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var number = number_example;  // string | Build number or in some cases _all
            var include = include_example;  // string | Extra fields to include in the response (optional) 

            try
            {
                // Build Status
                OrgsorgidprojectsprojectidbuildtargetsBuilds result = apiInstance.GetBuild(orgid, projectid, buildtargetid, number, include);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.GetBuild: " + e.Message );
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
 **number** | **string**| Build number or in some cases _all | 
 **include** | **string**| Extra fields to include in the response | [optional] 

### Return type

[**OrgsorgidprojectsprojectidbuildtargetsBuilds**](OrgsorgidprojectsprojectidbuildtargetsBuilds.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getbuildlog"></a>
# **GetBuildLog**
> void GetBuildLog (string orgid, string projectid, string buildtargetid, string number, decimal? offsetlines = null, bool? linenumbers = null, decimal? lastLineNumber = null, bool? compact = null, bool? withHtml = null)

Get build log

Retrieve the plain text log for a specifc build.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetBuildLogExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var number = number_example;  // string | Build number or in some cases _all
            var offsetlines = 8.14;  // decimal? | Stream log from the given line number (optional)  (default to 1)
            var linenumbers = true;  // bool? | Include log line numbers in the text output (optional)  (default to false)
            var lastLineNumber = 8.14;  // decimal? | The last line number seen, numbering will continue from here (optional)  (default to 0)
            var compact = true;  // bool? | Return the compact log, showing only errors and warnings (optional)  (default to false)
            var withHtml = true;  // bool? | Surround important lines (errors, warnings) with SPAN and CSS markup  (optional)  (default to false)

            try
            {
                // Get build log
                apiInstance.GetBuildLog(orgid, projectid, buildtargetid, number, offsetlines, linenumbers, lastLineNumber, compact, withHtml);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.GetBuildLog: " + e.Message );
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
 **number** | **string**| Build number or in some cases _all | 
 **offsetlines** | **decimal?**| Stream log from the given line number | [optional] [default to 1]
 **linenumbers** | **bool?**| Include log line numbers in the text output | [optional] [default to false]
 **lastLineNumber** | **decimal?**| The last line number seen, numbering will continue from here | [optional] [default to 0]
 **compact** | **bool?**| Return the compact log, showing only errors and warnings | [optional] [default to false]
 **withHtml** | **bool?**| Surround important lines (errors, warnings) with SPAN and CSS markup  | [optional] [default to false]

### Return type

void (empty response body)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getbuilds"></a>
# **GetBuilds**
> List<OrgsorgidprojectsprojectidbuildtargetsBuilds> GetBuilds (string orgid, string projectid, string buildtargetid, string include = null, decimal? perPage = null, decimal? page = null, string buildStatus = null, string platform = null, bool? showDeleted = null, bool? onlyFavorites = null)

List all builds

List all running and finished builds, sorted by build number (optionally paginating the results). Use '_all' as the buildtargetid to get all configured build targets. The response includes a Content-Range header that identifies the range of results returned and the total number of results matching the given query parameters. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetBuildsExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var include = include_example;  // string | Extra fields to include in the response (optional) 
            var perPage = 8.14;  // decimal? | Number of audit log records to retrieve (optional)  (default to 25)
            var page = 8.14;  // decimal? | Skip to page number, based on per_page value (optional)  (default to 1)
            var buildStatus = buildStatus_example;  // string | Query for only builds of a specific status (optional) 
            var platform = platform_example;  // string | Query for only builds of a specific platform (optional) 
            var showDeleted = true;  // bool? | Query for builds that have been deleted (optional)  (default to false)
            var onlyFavorites = true;  // bool? | Query for builds that have been favorited (optional)  (default to false)

            try
            {
                // List all builds
                List&lt;OrgsorgidprojectsprojectidbuildtargetsBuilds&gt; result = apiInstance.GetBuilds(orgid, projectid, buildtargetid, include, perPage, page, buildStatus, platform, showDeleted, onlyFavorites);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.GetBuilds: " + e.Message );
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
 **include** | **string**| Extra fields to include in the response | [optional] 
 **perPage** | **decimal?**| Number of audit log records to retrieve | [optional] [default to 25]
 **page** | **decimal?**| Skip to page number, based on per_page value | [optional] [default to 1]
 **buildStatus** | **string**| Query for only builds of a specific status | [optional] 
 **platform** | **string**| Query for only builds of a specific platform | [optional] 
 **showDeleted** | **bool?**| Query for builds that have been deleted | [optional] [default to false]
 **onlyFavorites** | **bool?**| Query for builds that have been favorited | [optional] [default to false]

### Return type

[**List<OrgsorgidprojectsprojectidbuildtargetsBuilds>**](OrgsorgidprojectsprojectidbuildtargetsBuilds.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getbuildsfororg"></a>
# **GetBuildsForOrg**
> List<OrgsorgidprojectsprojectidbuildtargetsBuilds> GetBuildsForOrg (string orgid, string include = null, decimal? perPage = null, decimal? page = null, string buildStatus = null, string platform = null, bool? showDeleted = null, bool? onlyFavorites = null)

List all builds for org

List all running and finished builds, sorted by build number (optionally paginating the results). The response includes a Content-Range header that identifies the range of results returned and the total number of results matching the given query parameters. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetBuildsForOrgExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var include = include_example;  // string | Extra fields to include in the response (optional) 
            var perPage = 8.14;  // decimal? | Number of audit log records to retrieve (optional)  (default to 25)
            var page = 8.14;  // decimal? | Skip to page number, based on per_page value (optional)  (default to 1)
            var buildStatus = buildStatus_example;  // string | Query for only builds of a specific status (optional) 
            var platform = platform_example;  // string | Query for only builds of a specific platform (optional) 
            var showDeleted = true;  // bool? | Query for builds that have been deleted (optional)  (default to false)
            var onlyFavorites = true;  // bool? | Query for builds that have been favorited (optional)  (default to false)

            try
            {
                // List all builds for org
                List&lt;OrgsorgidprojectsprojectidbuildtargetsBuilds&gt; result = apiInstance.GetBuildsForOrg(orgid, include, perPage, page, buildStatus, platform, showDeleted, onlyFavorites);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.GetBuildsForOrg: " + e.Message );
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
 **perPage** | **decimal?**| Number of audit log records to retrieve | [optional] [default to 25]
 **page** | **decimal?**| Skip to page number, based on per_page value | [optional] [default to 1]
 **buildStatus** | **string**| Query for only builds of a specific status | [optional] 
 **platform** | **string**| Query for only builds of a specific platform | [optional] 
 **showDeleted** | **bool?**| Query for builds that have been deleted | [optional] [default to false]
 **onlyFavorites** | **bool?**| Query for builds that have been favorited | [optional] [default to false]

### Return type

[**List<OrgsorgidprojectsprojectidbuildtargetsBuilds>**](OrgsorgidprojectsprojectidbuildtargetsBuilds.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getshare"></a>
# **GetShare**
> InlineResponse20015 GetShare (string orgid, string projectid, string buildtargetid, string number)

Get the share link

Gets a share link if it exists

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetShareExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var number = number_example;  // string | Build number or in some cases _all

            try
            {
                // Get the share link
                InlineResponse20015 result = apiInstance.GetShare(orgid, projectid, buildtargetid, number);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.GetShare: " + e.Message );
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
 **number** | **string**| Build number or in some cases _all | 

### Return type

[**InlineResponse20015**](InlineResponse20015.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="revokeshare"></a>
# **RevokeShare**
> string RevokeShare (string orgid, string projectid, string buildtargetid, string number)

Revoke a shared link

Revoke a shared link, both {buildtargetid} and {number} may use _all to revoke all share links for a given buildtarget or entire project.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class RevokeShareExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var number = number_example;  // string | Build number or in some cases _all

            try
            {
                // Revoke a shared link
                string result = apiInstance.RevokeShare(orgid, projectid, buildtargetid, number);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.RevokeShare: " + e.Message );
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
 **number** | **string**| Build number or in some cases _all | 

### Return type

**string**

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="startbuilds"></a>
# **StartBuilds**
> List<OrgsorgidprojectsprojectidbuildtargetsBuilds> StartBuilds (string orgid, string projectid, string buildtargetid, Options11 options = null)

Create new build

Start the build process for this build target (or all targets, if '_all' is specified as the buildtargetid), if there is not one currently in process.  If a build is currently in process that information will be related in the 'error' field. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class StartBuildsExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var options = new Options11(); // Options11 | Options for starting the builds. You can specify a platform and label only when  starting a local (_local) build. A local build will return immediately and be  marked as successful.  (optional) 

            try
            {
                // Create new build
                List&lt;OrgsorgidprojectsprojectidbuildtargetsBuilds&gt; result = apiInstance.StartBuilds(orgid, projectid, buildtargetid, options);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.StartBuilds: " + e.Message );
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
 **options** | [**Options11**](Options11.md)| Options for starting the builds. You can specify a platform and label only when  starting a local (_local) build. A local build will return immediately and be  marked as successful.  | [optional] 

### Return type

[**List<OrgsorgidprojectsprojectidbuildtargetsBuilds>**](OrgsorgidprojectsprojectidbuildtargetsBuilds.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatebuild"></a>
# **UpdateBuild**
> OrgsorgidprojectsprojectidbuildtargetsBuilds UpdateBuild (string orgid, string projectid, string buildtargetid, string number, Options12 options)

Update build information

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateBuildExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new BuildsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var buildtargetid = buildtargetid_example;  // string | unique id auto-generated from the build target name
            var number = number_example;  // string | Build number or in some cases _all
            var options = new Options12(); // Options12 | Options for build update

            try
            {
                // Update build information
                OrgsorgidprojectsprojectidbuildtargetsBuilds result = apiInstance.UpdateBuild(orgid, projectid, buildtargetid, number, options);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling BuildsApi.UpdateBuild: " + e.Message );
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
 **number** | **string**| Build number or in some cases _all | 
 **options** | [**Options12**](Options12.md)| Options for build update | 

### Return type

[**OrgsorgidprojectsprojectidbuildtargetsBuilds**](OrgsorgidprojectsprojectidbuildtargetsBuilds.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

