# IO.Swagger.Api.UsersApi

All URIs are relative to *https://localhost/api/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetUserApiKey**](UsersApi.md#getuserapikey) | **GET** /users/me/apikey | Get current user&#39;s API key
[**GetUserSelf**](UsersApi.md#getuserself) | **GET** /users/me | Get current user
[**RegenApiKey**](UsersApi.md#regenapikey) | **POST** /users/me/apikey | Regenerate API Key
[**UpdateUserSelf**](UsersApi.md#updateuserself) | **PUT** /users/me | Update current user


<a name="getuserapikey"></a>
# **GetUserApiKey**
> InlineResponse2003 GetUserApiKey ()

Get current user's API key

Get the currently authenticated user's API key.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetUserApiKeyExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new UsersApi();

            try
            {
                // Get current user's API key
                InlineResponse2003 result = apiInstance.GetUserApiKey();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UsersApi.GetUserApiKey: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**InlineResponse2003**](InlineResponse2003.md)

### Authorization

[apikey](../README.md#apikey)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getuserself"></a>
# **GetUserSelf**
> InlineResponse2002 GetUserSelf (string include = null)

Get current user

Get the currently authenticated user.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetUserSelfExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new UsersApi();
            var include = include_example;  // string | Extra fields to include in the response (optional) 

            try
            {
                // Get current user
                InlineResponse2002 result = apiInstance.GetUserSelf(include);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UsersApi.GetUserSelf: " + e.Message );
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

[**InlineResponse2002**](InlineResponse2002.md)

### Authorization

[apikey](../README.md#apikey)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="regenapikey"></a>
# **RegenApiKey**
> InlineResponse2004 RegenApiKey ()

Regenerate API Key

Remove current API key and generate a new one. *WARNING* you will need to use the returned API key in all subsequent calls.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class RegenApiKeyExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new UsersApi();

            try
            {
                // Regenerate API Key
                InlineResponse2004 result = apiInstance.RegenApiKey();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UsersApi.RegenApiKey: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**InlineResponse2004**](InlineResponse2004.md)

### Authorization

[apikey](../README.md#apikey)

### HTTP request headers

 - **Content-Type**: application/x-www-form-urlencoded
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateuserself"></a>
# **UpdateUserSelf**
> InlineResponse2002 UpdateUserSelf (Options options = null)

Update current user

You can update a few fields on the current user. Each field is optional and you do not need to specify all fields on update.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateUserSelfExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: apikey
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new UsersApi();
            var options = new Options(); // Options |  (optional) 

            try
            {
                // Update current user
                InlineResponse2002 result = apiInstance.UpdateUserSelf(options);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UsersApi.UpdateUserSelf: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **options** | [**Options**](Options.md)|  | [optional] 

### Return type

[**InlineResponse2002**](InlineResponse2002.md)

### Authorization

[apikey](../README.md#apikey)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, text/plain, text/html, text/csv

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

