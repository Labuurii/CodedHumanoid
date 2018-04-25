# IO.Swagger.Api.OrgsApi

All URIs are relative to *https://localhost/api/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetBillingPlans**](OrgsApi.md#getbillingplans) | **GET** /orgs/{orgid}/billingplan | Get billing plan
[**GetSSHKey**](OrgsApi.md#getsshkey) | **GET** /orgs/{orgid}/sshkey | Get SSH Key
[**RegenerateSSHKey**](OrgsApi.md#regeneratesshkey) | **POST** /orgs/{orgid}/sshkey | Regenerate SSH Key


<a name="getbillingplans"></a>
# **GetBillingPlans**
> InlineResponse2006 GetBillingPlans (string orgid)

Get billing plan

Get the billing plan for the specified organization

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

            var apiInstance = new OrgsApi();
            var orgid = orgid_example;  // string | Organization identifier

            try
            {
                // Get billing plan
                InlineResponse2006 result = apiInstance.GetBillingPlans(orgid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling OrgsApi.GetBillingPlans: " + e.Message );
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

[**InlineResponse2006**](InlineResponse2006.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsshkey"></a>
# **GetSSHKey**
> InlineResponse2008 GetSSHKey (string orgid)

Get SSH Key

Get the ssh public key for the specified org

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

            var apiInstance = new OrgsApi();
            var orgid = orgid_example;  // string | Organization identifier

            try
            {
                // Get SSH Key
                InlineResponse2008 result = apiInstance.GetSSHKey(orgid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling OrgsApi.GetSSHKey: " + e.Message );
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

[**InlineResponse2008**](InlineResponse2008.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="regeneratesshkey"></a>
# **RegenerateSSHKey**
> InlineResponse2008 RegenerateSSHKey (string orgid)

Regenerate SSH Key

Regenerate the ssh key for the specified org *WARNING* this is a destructive operation that will permanently remove your current SSH key.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class RegenerateSSHKeyExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: permissions
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new OrgsApi();
            var orgid = orgid_example;  // string | Organization identifier

            try
            {
                // Regenerate SSH Key
                InlineResponse2008 result = apiInstance.RegenerateSSHKey(orgid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling OrgsApi.RegenerateSSHKey: " + e.Message );
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

[**InlineResponse2008**](InlineResponse2008.md)

### Authorization

[apikey](../README.md#apikey), [permissions](../README.md#permissions)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

