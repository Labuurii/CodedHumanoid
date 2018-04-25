# IO.Swagger.Api.UserdevicesApi

All URIs are relative to *https://localhost/api/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**CreateDevice**](UserdevicesApi.md#createdevice) | **POST** /users/me/devices | Create iOS device profile
[**ListDevicesForUser**](UserdevicesApi.md#listdevicesforuser) | **GET** /users/me/devices | List iOS device profiles


<a name="createdevice"></a>
# **CreateDevice**
> Options1 CreateDevice (Options1 options)

Create iOS device profile

Create iOS device profile for the current user

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class CreateDeviceExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new UserdevicesApi();
            var options = new Options1(); // Options1 | 

            try
            {
                // Create iOS device profile
                Options1 result = apiInstance.CreateDevice(options);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserdevicesApi.CreateDevice: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **options** | [**Options1**](Options1.md)|  | 

### Return type

[**Options1**](Options1.md)

### Authorization

[apikey](../README.md#apikey)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="listdevicesforuser"></a>
# **ListDevicesForUser**
> List<InlineResponse2005> ListDevicesForUser ()

List iOS device profiles

List all iOS device profiles for the current user

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class ListDevicesForUserExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new UserdevicesApi();

            try
            {
                // List iOS device profiles
                List&lt;InlineResponse2005&gt; result = apiInstance.ListDevicesForUser();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserdevicesApi.ListDevicesForUser: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**List<InlineResponse2005>**](InlineResponse2005.md)

### Authorization

[apikey](../README.md#apikey)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

