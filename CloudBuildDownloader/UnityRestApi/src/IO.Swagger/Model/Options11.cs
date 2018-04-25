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
    /// Options11
    /// </summary>
    [DataContract]
    public partial class Options11 :  IEquatable<Options11>, IValidatableObject
    {
        /// <summary>
        /// Only valid for local builds 
        /// </summary>
        /// <value>Only valid for local builds </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PlatformEnum
        {
            
            /// <summary>
            /// Enum Ios for value: ios
            /// </summary>
            [EnumMember(Value = "ios")]
            Ios = 1,
            
            /// <summary>
            /// Enum Android for value: android
            /// </summary>
            [EnumMember(Value = "android")]
            Android = 2,
            
            /// <summary>
            /// Enum Webplayer for value: webplayer
            /// </summary>
            [EnumMember(Value = "webplayer")]
            Webplayer = 3,
            
            /// <summary>
            /// Enum Webgl for value: webgl
            /// </summary>
            [EnumMember(Value = "webgl")]
            Webgl = 4,
            
            /// <summary>
            /// Enum Standaloneosxintel for value: standaloneosxintel
            /// </summary>
            [EnumMember(Value = "standaloneosxintel")]
            Standaloneosxintel = 5,
            
            /// <summary>
            /// Enum Standaloneosxintel64 for value: standaloneosxintel64
            /// </summary>
            [EnumMember(Value = "standaloneosxintel64")]
            Standaloneosxintel64 = 6,
            
            /// <summary>
            /// Enum Standaloneosxuniversal for value: standaloneosxuniversal
            /// </summary>
            [EnumMember(Value = "standaloneosxuniversal")]
            Standaloneosxuniversal = 7,
            
            /// <summary>
            /// Enum Standalonewindows for value: standalonewindows
            /// </summary>
            [EnumMember(Value = "standalonewindows")]
            Standalonewindows = 8,
            
            /// <summary>
            /// Enum Standalonewindows64 for value: standalonewindows64
            /// </summary>
            [EnumMember(Value = "standalonewindows64")]
            Standalonewindows64 = 9,
            
            /// <summary>
            /// Enum Standalonelinux for value: standalonelinux
            /// </summary>
            [EnumMember(Value = "standalonelinux")]
            Standalonelinux = 10,
            
            /// <summary>
            /// Enum Standalonelinux64 for value: standalonelinux64
            /// </summary>
            [EnumMember(Value = "standalonelinux64")]
            Standalonelinux64 = 11,
            
            /// <summary>
            /// Enum Standalonelinuxuniversal for value: standalonelinuxuniversal
            /// </summary>
            [EnumMember(Value = "standalonelinuxuniversal")]
            Standalonelinuxuniversal = 12
        }

        /// <summary>
        /// Only valid for local builds 
        /// </summary>
        /// <value>Only valid for local builds </value>
        [DataMember(Name="platform", EmitDefaultValue=false)]
        public PlatformEnum? Platform { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Options11" /> class.
        /// </summary>
        /// <param name="Clean">Clean.</param>
        /// <param name="Delay">Delay.</param>
        /// <param name="Commit">Commit.</param>
        /// <param name="Label">Only valid for local builds .</param>
        /// <param name="Platform">Only valid for local builds .</param>
        public Options11(bool? Clean = default(bool?), decimal? Delay = default(decimal?), string Commit = default(string), string Label = default(string), PlatformEnum? Platform = default(PlatformEnum?))
        {
            this.Clean = Clean;
            this.Delay = Delay;
            this.Commit = Commit;
            this.Label = Label;
            this.Platform = Platform;
        }
        
        /// <summary>
        /// Gets or Sets Clean
        /// </summary>
        [DataMember(Name="clean", EmitDefaultValue=false)]
        public bool? Clean { get; set; }

        /// <summary>
        /// Gets or Sets Delay
        /// </summary>
        [DataMember(Name="delay", EmitDefaultValue=false)]
        public decimal? Delay { get; set; }

        /// <summary>
        /// Gets or Sets Commit
        /// </summary>
        [DataMember(Name="commit", EmitDefaultValue=false)]
        public string Commit { get; set; }

        /// <summary>
        /// Only valid for local builds 
        /// </summary>
        /// <value>Only valid for local builds </value>
        [DataMember(Name="label", EmitDefaultValue=false)]
        public string Label { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Options11 {\n");
            sb.Append("  Clean: ").Append(Clean).Append("\n");
            sb.Append("  Delay: ").Append(Delay).Append("\n");
            sb.Append("  Commit: ").Append(Commit).Append("\n");
            sb.Append("  Label: ").Append(Label).Append("\n");
            sb.Append("  Platform: ").Append(Platform).Append("\n");
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
            return this.Equals(input as Options11);
        }

        /// <summary>
        /// Returns true if Options11 instances are equal
        /// </summary>
        /// <param name="input">Instance of Options11 to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Options11 input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Clean == input.Clean ||
                    (this.Clean != null &&
                    this.Clean.Equals(input.Clean))
                ) && 
                (
                    this.Delay == input.Delay ||
                    (this.Delay != null &&
                    this.Delay.Equals(input.Delay))
                ) && 
                (
                    this.Commit == input.Commit ||
                    (this.Commit != null &&
                    this.Commit.Equals(input.Commit))
                ) && 
                (
                    this.Label == input.Label ||
                    (this.Label != null &&
                    this.Label.Equals(input.Label))
                ) && 
                (
                    this.Platform == input.Platform ||
                    (this.Platform != null &&
                    this.Platform.Equals(input.Platform))
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
                if (this.Clean != null)
                    hashCode = hashCode * 59 + this.Clean.GetHashCode();
                if (this.Delay != null)
                    hashCode = hashCode * 59 + this.Delay.GetHashCode();
                if (this.Commit != null)
                    hashCode = hashCode * 59 + this.Commit.GetHashCode();
                if (this.Label != null)
                    hashCode = hashCode * 59 + this.Label.GetHashCode();
                if (this.Platform != null)
                    hashCode = hashCode * 59 + this.Platform.GetHashCode();
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