/* 
 * Simple Api
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1.2
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
    /// Category
    /// </summary>
    [DataContract]
        public partial class Category :  IEquatable<Category>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Category" /> class.
        /// </summary>
        /// <param name="categoryId">categoryId (required).</param>
        /// <param name="categoryName">categoryName (required).</param>
        /// <param name="description">description.</param>
        /// <param name="picture">picture.</param>
        /// <param name="products">products.</param>
        public Category(int? categoryId = default(int?), string categoryName = default(string), string description = default(string), byte[] picture = default(byte[]), List<Product> products = default(List<Product>))
        {
            // to ensure "categoryId" is required (not null)
            if (categoryId == null)
            {
                throw new InvalidDataException("categoryId is a required property for Category and cannot be null");
            }
            else
            {
                this.CategoryId = categoryId;
            }
            // to ensure "categoryName" is required (not null)
            if (categoryName == null)
            {
                throw new InvalidDataException("categoryName is a required property for Category and cannot be null");
            }
            else
            {
                this.CategoryName = categoryName;
            }
            this.Description = description;
            this.Picture = picture;
            this.Products = products;
        }
        
        /// <summary>
        /// Gets or Sets CategoryId
        /// </summary>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets CategoryName
        /// </summary>
        [DataMember(Name="categoryName", EmitDefaultValue=false)]
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets Picture
        /// </summary>
        [DataMember(Name="picture", EmitDefaultValue=false)]
        public byte[] Picture { get; set; }

        /// <summary>
        /// Gets or Sets Products
        /// </summary>
        [DataMember(Name="products", EmitDefaultValue=false)]
        public List<Product> Products { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Category {\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  CategoryName: ").Append(CategoryName).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Picture: ").Append(Picture).Append("\n");
            sb.Append("  Products: ").Append(Products).Append("\n");
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
            return this.Equals(input as Category);
        }

        /// <summary>
        /// Returns true if Category instances are equal
        /// </summary>
        /// <param name="input">Instance of Category to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Category input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.CategoryId == input.CategoryId ||
                    (this.CategoryId != null &&
                    this.CategoryId.Equals(input.CategoryId))
                ) && 
                (
                    this.CategoryName == input.CategoryName ||
                    (this.CategoryName != null &&
                    this.CategoryName.Equals(input.CategoryName))
                ) && 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.Picture == input.Picture ||
                    (this.Picture != null &&
                    this.Picture.Equals(input.Picture))
                ) && 
                (
                    this.Products == input.Products ||
                    this.Products != null &&
                    input.Products != null &&
                    this.Products.SequenceEqual(input.Products)
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
                if (this.CategoryId != null)
                    hashCode = hashCode * 59 + this.CategoryId.GetHashCode();
                if (this.CategoryName != null)
                    hashCode = hashCode * 59 + this.CategoryName.GetHashCode();
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.Picture != null)
                    hashCode = hashCode * 59 + this.Picture.GetHashCode();
                if (this.Products != null)
                    hashCode = hashCode * 59 + this.Products.GetHashCode();
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
