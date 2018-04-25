# IO.Swagger.Api.ProjectsApi

All URIs are relative to *https://localhost/api/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**AddProject**](ProjectsApi.md#addproject) | **POST** /orgs/{orgid}/projects | Create project
[**ArchiveProject**](ProjectsApi.md#archiveproject) | **DELETE** /orgs/{orgid}/projects/{projectid} | Archive project
[**GetAuditLog**](ProjectsApi.md#getauditlog) | **GET** /orgs/{orgid}/projects/{projectid}/auditlog | Get audit log
[**GetBillingPlans**](ProjectsApi.md#getbillingplans) | **GET** /orgs/{orgid}/projects/{projectid}/billingplan | Get billing plan
[**GetProject**](ProjectsApi.md#getproject) | **GET** /orgs/{orgid}/projects/{projectid} | Get project details
[**GetProjectByUpid**](ProjectsApi.md#getprojectbyupid) | **GET** /projects/{projectupid} | Get project details
[**GetSSHKey**](ProjectsApi.md#getsshkey) | **GET** /orgs/{orgid}/projects/{projectid}/sshkey | Get SSH Key
[**GetStats**](ProjectsApi.md#getstats) | **GET** /orgs/{orgid}/projects/{projectid}/stats | Get project statistics
[**ListProjectsForOrg**](ProjectsApi.md#listprojectsfororg) | **GET** /orgs/{orgid}/projects | List all projects (org)
[**ListProjectsForUser**](ProjectsApi.md#listprojectsforuser) | **GET** /projects | List all projects (user)
[**UpdateProject**](ProjectsApi.md#updateproject) | **PUT** /orgs/{orgid}/projects/{projectid} | Update project details


<a name="addproject"></a>
# **AddProject**
> InlineResponse2009 AddProject (string orgid, Options4 options)

Create project

Create a project for the specified organization

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class AddProjectExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ProjectsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var options = new Options4(); // Options4 | Options for project create/update

            try
            {
                // Create project
                InlineResponse2009 result = apiInstance.AddProject(orgid, options);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProjectsApi.AddProject: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **orgid** | **string**| Organization identifier | 
 **options** | [**Options4**](Options4.md)| Options for project create/update | 

### Return type

[**InlineResponse2009**](InlineResponse2009.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="archiveproject"></a>
# **ArchiveProject**
> string ArchiveProject (string orgid, string projectid)

Archive project

This will archive the project in Cloud Build ONLY. Use with caution - this process is not reversible. The projects UPID will be removed from Cloud Build allowing the project to be reconfigured.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class ArchiveProjectExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ProjectsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier

            try
            {
                // Archive project
                string result = apiInstance.ArchiveProject(orgid, projectid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProjectsApi.ArchiveProject: " + e.Message );
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
> List<InlineResponse20011> GetAuditLog (string orgid, string projectid, decimal? perPage = null, decimal? page = null)

Get audit log

Retrieve a list of historical settings changes for this project

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

            var apiInstance = new ProjectsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var perPage = 8.14;  // decimal? | Number of audit log records to retrieve (optional)  (default to 25)
            var page = 8.14;  // decimal? | Skip to page number, based on per_page value (optional)  (default to 1)

            try
            {
                // Get audit log
                List&lt;InlineResponse20011&gt; result = apiInstance.GetAuditLog(orgid, projectid, perPage, page);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProjectsApi.GetAuditLog: " + e.Message );
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

<a name="getbillingplans"></a>
# **GetBillingPlans**
> InlineResponse2006 GetBillingPlans (string orgid, string projectid)

Get billing plan

Get the billing plan for the specified project

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetBillingPlansExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ProjectsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier

            try
            {
                // Get billing plan
                InlineResponse2006 result = apiInstance.GetBillingPlans(orgid, projectid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProjectsApi.GetBillingPlans: " + e.Message );
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

### Return type

[**InlineResponse2006**](InlineResponse2006.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getproject"></a>
# **GetProject**
> InlineResponse2009 GetProject (string orgid, string projectid, string include = null)

Get project details

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetProjectExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ProjectsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var include = include_example;  // string | Extra fields to include in the response (optional) 

            try
            {
                // Get project details
                InlineResponse2009 result = apiInstance.GetProject(orgid, projectid, include);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProjectsApi.GetProject: " + e.Message );
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

### Return type

[**InlineResponse2009**](InlineResponse2009.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getprojectbyupid"></a>
# **GetProjectByUpid**
> InlineResponse2009 GetProjectByUpid (string projectupid)

Get project details

Gets the same data as /orgs/{orgid}/project/{projectid} but looks up the project by the Unity Project ID. This value is returned in the project's guid field.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetProjectByUpidExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ProjectsApi();
            var projectupid = projectupid_example;  // string | Project UPID - Unity global id

            try
            {
                // Get project details
                InlineResponse2009 result = apiInstance.GetProjectByUpid(projectupid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProjectsApi.GetProjectByUpid: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **projectupid** | **string**| Project UPID - Unity global id | 

### Return type

[**InlineResponse2009**](InlineResponse2009.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsshkey"></a>
# **GetSSHKey**
> InlineResponse2008 GetSSHKey (string orgid, string projectid)

Get SSH Key

Get the ssh public key for the specified project

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSSHKeyExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ProjectsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier

            try
            {
                // Get SSH Key
                InlineResponse2008 result = apiInstance.GetSSHKey(orgid, projectid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProjectsApi.GetSSHKey: " + e.Message );
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

### Return type

[**InlineResponse2008**](InlineResponse2008.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getstats"></a>
# **GetStats**
> InlineResponse20010 GetStats (string orgid, string projectid)

Get project statistics

Get statistics for the specified project

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetStatsExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ProjectsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier

            try
            {
                // Get project statistics
                InlineResponse20010 result = apiInstance.GetStats(orgid, projectid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProjectsApi.GetStats: " + e.Message );
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

### Return type

[**InlineResponse20010**](InlineResponse20010.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="listprojectsfororg"></a>
# **ListProjectsForOrg**
> List<InlineResponse2009> ListProjectsForOrg (string orgid, string include = null)

List all projects (org)

List all projects that belong to the specified organization. Add \"?include=settings\" as a query parameter to include the project settings with the response. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class ListProjectsForOrgExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ProjectsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var include = include_example;  // string | Extra fields to include in the response (optional) 

            try
            {
                // List all projects (org)
                List&lt;InlineResponse2009&gt; result = apiInstance.ListProjectsForOrg(orgid, include);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProjectsApi.ListProjectsForOrg: " + e.Message );
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

### Return type

[**List<InlineResponse2009>**](InlineResponse2009.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="listprojectsforuser"></a>
# **ListProjectsForUser**
> List<InlineResponse2009> ListProjectsForUser (string include = null)

List all projects (user)

List all projects that you have permission to access across all organizations. Add \"?include=settings\" as a query parameter to include the project settings with the response. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class ListProjectsForUserExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new ProjectsApi();
            var include = include_example;  // string | Extra fields to include in the response (optional) 

            try
            {
                // List all projects (user)
                List&lt;InlineResponse2009&gt; result = apiInstance.ListProjectsForUser(include);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProjectsApi.ListProjectsForUser: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **include** | **string**| Extra fields to include in the response | [optional] 

### Return type

[**List<InlineResponse2009>**](InlineResponse2009.md)

### Authorization

[apikey](../README.md#apikey)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateproject"></a>
# **UpdateProject**
> InlineResponse2009 UpdateProject (string orgid, string projectid, Options5 options)

Update project details

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateProjectExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ProjectsApi();
            var orgid = orgid_example;  // string | Organization identifier
            var projectid = projectid_example;  // string | Project identifier
            var options = new Options5(); // Options5 | Options for project create/update

            try
            {
                // Update project details
                InlineResponse2009 result = apiInstance.UpdateProject(orgid, projectid, options);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProjectsApi.UpdateProject: " + e.Message );
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
 **options** | [**Options5**](Options5.md)| Options for project create/update | 

### Return type

[**InlineResponse2009**](InlineResponse2009.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

