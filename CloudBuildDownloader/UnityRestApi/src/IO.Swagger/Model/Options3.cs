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
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = IO.Swagger.Client.SwaggerDateConverter;

namespace IO.Swagger.Model
{
    /// <summary>
    /// Options3
    /// </summary>
    [DataContract]
    public partial class Options3 :  IEquatable<Options3>, IValidatableObject
    {
        /// <summary>
        /// Defines HookType
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum HookTypeEnum
        {
            
            /// <summary>
            /// Enum Web for value: web
            /// </summary>
            [EnumMember(Value = "web")]
            Web = 1,
            
            /// <summary>
            /// Enum Slack for value: slack
            /// </summary>
            [EnumMember(Value = "slack")]
            Slack = 2
        }

        /// <summary>
        /// Gets or Sets HookType
        /// </summary>
        [DataMember(Name="hookType", EmitDefaultValue=false)]
        public HookTypeEnum HookType { get; set; }
        /// <summary>
        /// Defines Events
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum EventsEnum
        {
            
            /// <summary>
            /// Enum ProjectBuildQueued for value: ProjectBuildQueued
            /// </summary>
            [EnumMember(Value = "ProjectBuildQueued")]
            ProjectBuildQueued = 1,
            
            /// <summary>
            /// Enum ProjectBuildStarted for value: ProjectBuildStarted
            /// </summary>
            [EnumMember(Value = "ProjectBuildStarted")]
            ProjectBuildStarted = 2,
            
            /// <summary>
            /// Enum ProjectBuildRestarted for value: ProjectBuildRestarted
            /// </summary>
            [EnumMember(Value = "ProjectBuildRestarted")]
            ProjectBuildRestarted = 3,
            
            /// <summary>
            /// Enum ProjectBuildSuccess for value: ProjectBuildSuccess
            /// </summary>
            [EnumMember(Value = "ProjectBuildSuccess")]
            ProjectBuildSuccess = 4,
            
            /// <summary>
            /// Enum ProjectBuildFailure for value: ProjectBuildFailure
            /// </summary>
            [EnumMember(Value = "ProjectBuildFailure")]
            ProjectBuildFailure = 5,
            
            /// <summary>
            /// Enum ProjectBuildCanceled for value: ProjectBuildCanceled
            /// </summary>
            [EnumMember(Value = "ProjectBuildCanceled")]
            ProjectBuildCanceled = 6,
            
            /// <summary>
            /// Enum ProjectBuildUpload for value: ProjectBuildUpload
            /// </summary>
            [EnumMember(Value = "ProjectBuildUpload")]
            ProjectBuildUpload = 7
        }


        /// <summary>
        /// Gets or Sets Events
        /// </summary>
        [DataMember(Name="events", EmitDefaultValue=false)]
        public List<EventsEnum> Events { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Options3" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected Options3() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Options3" /> class.
        /// </summary>
        /// <param name="HookType">HookType (required).</param>
        /// <param name="Events">Events.</param>
        /// <param name="Config">Config (required).</param>
        /// <param name="Active">Active (default to true).</param>
        public Options3(HookTypeEnum HookType = default(HookTypeEnum), List<EventsEnum> Events = default(List<EventsEnum>), Object Config = default(Object), bool? Active = true)
        {
            // to ensure "HookType" is required (not null)
            if (HookType == null)
            {
                throw new InvalidDataException("HookType is a required property for Options3 and cannot be null");
            }
            else
            {
                this.HookType = HookType;
            }
            // to ensure "Config" is required (not null)
            if (Config == null)
            {
                throw new InvalidDataException("Config is a required property for Options3 and cannot be null");
            }
            else
            {
                this.Config = Config;
            }
            this.Events = Events;
            // use default value if no "Active" provided
            if (Active == null)
            {
                this.Active = true;
            }
            else
            {
                this.Active = Active;
            }
        }
        


        /// <summary>
        /// Gets or Sets Config
        /// </summary>
        [DataMember(Name="config", EmitDefaultValue=false)]
        public Object Config { get; set; }

        /// <summary>
        /// Gets or Sets Active
        /// </summary>
        [DataMember(Name="active", EmitDefaultValue=false)]
        public bool? Active { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Options3 {\n");
            sb.Append("  HookType: ").Append(HookType).Append("\n");
            sb.Append("  Events: ").Append(Events).Append("\n");
            sb.Append("  Config: ").Append(Config).Append("\n");
            sb.Append("  Active: ").Append(Active).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as Options3);
        }

        /// <summary>
        /// Returns true if Options3 instances are equal
        /// </summary>
        /// <param name="input">Instance of Options3 to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Options3 input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.HookType == input.HookType ||
                    (this.HookType != null &&
                    this.HookType.Equals(input.HookType))
                ) && 
                (
                    this.Events == input.Events ||
                    this.Events != null &&
                    this.Events.SequenceEqual(input.Events)
                ) && 
                (
                    this.Config == input.Config ||
                    (this.Config != null &&
                    this.Config.Equals(input.Config))
                ) && 
                (
                    this.Active == input.Active ||
                    (this.Active != null &&
                    this.Active.Equals(input.Active))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.HookType != null)
                    hashCode = hashCode * 59 + this.HookType.GetHashCode();
                if (this.Events != null)
                    hashCode = hashCode * 59 + this.Events.GetHashCode();
                if (this.Config != null)
                    hashCode = hashCode * 59 + this.Config.GetHashCode();
                if (this.Active != null)
                    hashCode = hashCode * 59 + this.Active.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}