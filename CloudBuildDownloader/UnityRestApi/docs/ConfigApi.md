# IO.Swagger.Api.ConfigApi

All URIs are relative to *https://localhost/api/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**ListUnityVersions**](ConfigApi.md#listunityversions) | **GET** /versions/unity | List all unity versions
[**ListXcodeVersions**](ConfigApi.md#listxcodeversions) | **GET** /versions/xcode | List all xcode versions


<a name="listunityversions"></a>
# **ListUnityVersions**
> List<InlineResponse200> ListUnityVersions ()

List all unity versions

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class ListUnityVersionsExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new ConfigApi();

            try
            {
                // List all unity versions
                List&lt;InlineResponse200&gt; result = apiInstance.ListUnityVersions();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ConfigApi.ListUnityVersions: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**List<InlineResponse200>**](InlineResponse200.md)

### Authorization

[apikey](../README.md#apikey)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="listxcodeversions"></a>
# **ListXcodeVersions**
> List<InlineResponse2001> ListXcodeVersions ()

List all xcode versions

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class ListXcodeVersionsExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new ConfigApi();

            try
            {
                // List all xcode versions
                List&lt;InlineResponse2001&gt; result = apiInstance.ListXcodeVersions();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ConfigApi.ListXcodeVersions: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**List<InlineResponse2001>**](InlineResponse2001.md)

### Authorization

[apikey](../README.md#apikey)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

