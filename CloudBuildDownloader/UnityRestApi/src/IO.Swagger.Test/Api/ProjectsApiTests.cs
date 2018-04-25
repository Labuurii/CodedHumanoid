/* 
 * Unity Cloud Build
 *
 * This API is intended to be used in conjunction with the Unity Cloud Build service. A tool for building your Unity projects in the Cloud.  See https://developer.cloud.unity3d.com for more information.  ## Making requests This website is built to allow requests to be made against the API. If you are currently logged into Cloud Build you should be able to make requests without entering an API key.   You can find your API key in the Unity Cloud Services portal by clicking on 'Cloud Build Preferences' in the sidebar. Copy the API Key and paste it into the upper left corner of this website. It will be used in all subsequent requests.  ## Clients The Unity Cloud Build API is based upon Swagger. Client libraries to integrate with your projects can easily be generated with the [Swagger Code Generator](https://github.com/swagger-api/swagger-codegen).  The JSON schema required to generate a client for this API version is located here:  ``` [API_URL][BASE_PATH]/api.json ```  ## Authorization The Unity Cloud Build API requires an access token from your Unity Cloud Build account, which can be found at https://build.cloud.unity3d.com/login/me  To authenticate requests, include a Basic Authentication header with your API key as the value. e.g.  ``` Authorization: Basic [YOUR API KEY] ```  ## Pagination Paged results will take two parameters. A page number that is calculated based upon the per_page amount. For instance if there are 40 results and you specify page 2 with per_page set to 10 you will receive records 11-20.  Paged results will also return a Content-Range header. For the example above the content range header would look like this:  ``` Content-Range: items 11-20/40 ```  ## Versioning The API version is indicated in the request URL. Upgrading to a newer API version can be done by changing the path.  The API will receive a new version in the following cases:    * removal of a path or request type   * addition of a required field   * removal of a required field  The following changes are considered backwards compatible and will not trigger a new API version:    * addition of an endpoint or request type   * addition of an optional field   * removal of an optional field   * changes to the format of ids  ## Rate Limiting Requests against the Cloud Build API are limited to a rate of 100 per minute. To preserve the quality of service throughout Cloud Build, additional rate limits may apply to some actions. For example, polling aggressively instead of using webhooks or making API calls with a high concurrency may result in rate limiting.  It is not intended for these rate limits to interfere with any legitimate use of the API. Please contact support at <cloudbuild@unity3d.com> if your use is affected by this rate limit.  You can check the returned HTTP headers for any API request to see your current rate limit status.   * __X-RateLimit-Limit:__ maximum number of requests per minute   * __X-RateLimit-Remaining:__ remaining number of requests in the current window   * __X-RateLimit-Reset:__ time at which the current window will reset (UTC epoch seconds)  Once you go over the rate limit you will receive an error response: ``` HTTP Status: 429 {   \"error\": \"Rate limit exceeded, retry in XX seconds\" } ``` 
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using RestSharp;
using NUnit.Framework;

using IO.Swagger.Client;
using IO.Swagger.Api;
using IO.Swagger.Model;

namespace IO.Swagger.Test
{
    /// <summary>
    ///  Class for testing ProjectsApi
    /// </summary>
    /// <remarks>
    /// This file is automatically generated by Swagger Codegen.
    /// Please update the test case below to test the API endpoint.
    /// </remarks>
    [TestFixture]
    public class ProjectsApiTests
    {
        private ProjectsApi instance;

        /// <summary>
        /// Setup before each unit test
        /// </summary>
        [SetUp]
        public void Init()
        {
            instance = new ProjectsApi();
        }

        /// <summary>
        /// Clean up after each unit test
        /// </summary>
        [TearDown]
        public void Cleanup()
        {

        }

        /// <summary>
        /// Test an instance of ProjectsApi
        /// </summary>
        [Test]
        public void InstanceTest()
        {
            // TODO uncomment below to test 'IsInstanceOfType' ProjectsApi
            //Assert.IsInstanceOfType(typeof(ProjectsApi), instance, "instance is a ProjectsApi");
        }

        
        /// <summary>
        /// Test AddProject
        /// </summary>
        [Test]
        public void AddProjectTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string orgid = null;
            //Options4 options = null;
            //var response = instance.AddProject(orgid, options);
            //Assert.IsInstanceOf<InlineResponse2009> (response, "response is InlineResponse2009");
        }
        
        /// <summary>
        /// Test ArchiveProject
        /// </summary>
        [Test]
        public void ArchiveProjectTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string orgid = null;
            //string projectid = null;
            //var response = instance.ArchiveProject(orgid, projectid);
            //Assert.IsInstanceOf<string> (response, "response is string");
        }
        
        /// <summary>
        /// Test GetAuditLog
        /// </summary>
        [Test]
        public void GetAuditLogTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string orgid = null;
            //string projectid = null;
            //decimal? perPage = null;
            //decimal? page = null;
            //var response = instance.GetAuditLog(orgid, projectid, perPage, page);
            //Assert.IsInstanceOf<List<InlineResponse20011>> (response, "response is List<InlineResponse20011>");
        }
        
        /// <summary>
        /// Test GetBillingPlans
        /// </summary>
        [Test]
        public void GetBillingPlansTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string orgid = null;
            //string projectid = null;
            //var response = instance.GetBillingPlans(orgid, projectid);
            //Assert.IsInstanceOf<InlineResponse2006> (response, "response is InlineResponse2006");
        }
        
        /// <summary>
        /// Test GetProject
        /// </summary>
        [Test]
        public void GetProjectTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string orgid = null;
            //string projectid = null;
            //string include = null;
            //var response = instance.GetProject(orgid, projectid, include);
            //Assert.IsInstanceOf<InlineResponse2009> (response, "response is InlineResponse2009");
        }
        
        /// <summary>
        /// Test GetProjectByUpid
        /// </summary>
        [Test]
        public void GetProjectByUpidTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string projectupid = null;
            //var response = instance.GetProjectByUpid(projectupid);
            //Assert.IsInstanceOf<InlineResponse2009> (response, "response is InlineResponse2009");
        }
        
        /// <summary>
        /// Test GetSSHKey
        /// </summary>
        [Test]
        public void GetSSHKeyTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string orgid = null;
            //string projectid = null;
            //var response = instance.GetSSHKey(orgid, projectid);
            //Assert.IsInstanceOf<InlineResponse2008> (response, "response is InlineResponse2008");
        }
        
        /// <summary>
        /// Test GetStats
        /// </summary>
        [Test]
        public void GetStatsTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string orgid = null;
            //string projectid = null;
            //var response = instance.GetStats(orgid, projectid);
            //Assert.IsInstanceOf<InlineResponse20010> (response, "response is InlineResponse20010");
        }
        
        /// <summary>
        /// Test ListProjectsForOrg
        /// </summary>
        [Test]
        public void ListProjectsForOrgTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string orgid = null;
            //string include = null;
            //var response = instance.ListProjectsForOrg(orgid, include);
            //Assert.IsInstanceOf<List<InlineResponse2009>> (response, "response is List<InlineResponse2009>");
        }
        
        /// <summary>
        /// Test ListProjectsForUser
        /// </summary>
        [Test]
        public void ListProjectsForUserTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string include = null;
            //var response = instance.ListProjectsForUser(include);
            //Assert.IsInstanceOf<List<InlineResponse2009>> (response, "response is List<InlineResponse2009>");
        }
        
        /// <summary>
        /// Test UpdateProject
        /// </summary>
        [Test]
        public void UpdateProjectTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string orgid = null;
            //string projectid = null;
            //Options5 options = null;
            //var response = instance.UpdateProject(orgid, projectid, options);
            //Assert.IsInstanceOf<InlineResponse2009> (response, "response is InlineResponse2009");
        }
        
    }

}
