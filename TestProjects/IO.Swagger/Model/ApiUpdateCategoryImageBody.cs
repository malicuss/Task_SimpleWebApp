/* 
 * Simple Api
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1.1
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
    /// ApiUpdateCategoryImageBody
    /// </summary>
    [DataContract]
        public partial class ApiUpdateCategoryImageBody :  IEquatable<ApiUpdateCategoryImageBody>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiUpdateCategoryImageBody" /> class.
        /// </summary>
        /// <param name="contentType">contentType.</param>
        /// <param name="contentDisposition">contentDisposition.</param>
        /// <param name="headers">headers.</param>
        /// <param name="length">length.</param>
        /// <param name="name">name.</param>
        /// <param name="fileName">fileName.</param>
        public ApiUpdateCategoryImageBody(string contentType = default(string), string contentDisposition = default(string), Dictionary<string, List<string>> headers = default(Dictionary<string, List<string>>), long? length = default(long?), string name = default(string), string fileName = default(string))
        {
            this.ContentType = contentType;
            this.ContentDisposition = contentDisposition;
            this.Headers = headers;
            this.Length = length;
            this.Name = name;
            this.FileName = fileName;
        }
        
        /// <summary>
        /// Gets or Sets ContentType
        /// </summary>
        [DataMember(Name="ContentType", EmitDefaultValue=false)]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or Sets ContentDisposition
        /// </summary>
        [DataMember(Name="ContentDisposition", EmitDefaultValue=false)]
        public string ContentDisposition { get; set; }

        /// <summary>
        /// Gets or Sets Headers
        /// </summary>
        [DataMember(Name="Headers", EmitDefaultValue=false)]
        public Dictionary<string, List<string>> Headers { get; set; }

        /// <summary>
        /// Gets or Sets Length
        /// </summary>
        [DataMember(Name="Length", EmitDefaultValue=false)]
        public long? Length { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="Name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets FileName
        /// </summary>
        [DataMember(Name="FileName", EmitDefaultValue=false)]
        public string FileName { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiUpdateCategoryImageBody {\n");
            sb.Append("  ContentType: ").Append(ContentType).Append("\n");
            sb.Append("  ContentDisposition: ").Append(ContentDisposition).Append("\n");
            sb.Append("  Headers: ").Append(Headers).Append("\n");
            sb.Append("  Length: ").Append(Length).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  FileName: ").Append(FileName).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
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
            return this.Equals(input as ApiUpdateCategoryImageBody);
        }

        /// <summary>
        /// Returns true if ApiUpdateCategoryImageBody instances are equal
        /// </summary>
        /// <param name="input">Instance of ApiUpdateCategoryImageBody to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiUpdateCategoryImageBody input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.ContentType == input.ContentType ||
                    (this.ContentType != null &&
                    this.ContentType.Equals(input.ContentType))
                ) && 
                (
                    this.ContentDisposition == input.ContentDisposition ||
                    (this.ContentDisposition != null &&
                    this.ContentDisposition.Equals(input.ContentDisposition))
                ) && 
                (
                    this.Headers == input.Headers ||
                    this.Headers != null &&
                    input.Headers != null &&
                    this.Headers.SequenceEqual(input.Headers)
                ) && 
                (
                    this.Length == input.Length ||
                    (this.Length != null &&
                    this.Length.Equals(input.Length))
                ) && 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.FileName == input.FileName ||
                    (this.FileName != null &&
                    this.FileName.Equals(input.FileName))
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
                if (this.ContentType != null)
                    hashCode = hashCode * 59 + this.ContentType.GetHashCode();
                if (this.ContentDisposition != null)
                    hashCode = hashCode * 59 + this.ContentDisposition.GetHashCode();
                if (this.Headers != null)
                    hashCode = hashCode * 59 + this.Headers.GetHashCode();
                if (this.Length != null)
                    hashCode = hashCode * 59 + this.Length.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.FileName != null)
                    hashCode = hashCode * 59 + this.FileName.GetHashCode();
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
