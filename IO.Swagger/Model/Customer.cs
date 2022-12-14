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
    /// Customer
    /// </summary>
    [DataContract]
        public partial class Customer :  IEquatable<Customer>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Customer" /> class.
        /// </summary>
        /// <param name="customerId">customerId.</param>
        /// <param name="companyName">companyName.</param>
        /// <param name="contactName">contactName.</param>
        /// <param name="contactTitle">contactTitle.</param>
        /// <param name="address">address.</param>
        /// <param name="city">city.</param>
        /// <param name="region">region.</param>
        /// <param name="postalCode">postalCode.</param>
        /// <param name="country">country.</param>
        /// <param name="phone">phone.</param>
        /// <param name="fax">fax.</param>
        /// <param name="orders">orders.</param>
        /// <param name="customerTypes">customerTypes.</param>
        public Customer(string customerId = default(string), string companyName = default(string), string contactName = default(string), string contactTitle = default(string), string address = default(string), string city = default(string), string region = default(string), string postalCode = default(string), string country = default(string), string phone = default(string), string fax = default(string), List<Order> orders = default(List<Order>), List<CustomerDemographic> customerTypes = default(List<CustomerDemographic>))
        {
            this.CustomerId = customerId;
            this.CompanyName = companyName;
            this.ContactName = contactName;
            this.ContactTitle = contactTitle;
            this.Address = address;
            this.City = city;
            this.Region = region;
            this.PostalCode = postalCode;
            this.Country = country;
            this.Phone = phone;
            this.Fax = fax;
            this.Orders = orders;
            this.CustomerTypes = customerTypes;
        }
        
        /// <summary>
        /// Gets or Sets CustomerId
        /// </summary>
        [DataMember(Name="customerId", EmitDefaultValue=false)]
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or Sets CompanyName
        /// </summary>
        [DataMember(Name="companyName", EmitDefaultValue=false)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or Sets ContactName
        /// </summary>
        [DataMember(Name="contactName", EmitDefaultValue=false)]
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or Sets ContactTitle
        /// </summary>
        [DataMember(Name="contactTitle", EmitDefaultValue=false)]
        public string ContactTitle { get; set; }

        /// <summary>
        /// Gets or Sets Address
        /// </summary>
        [DataMember(Name="address", EmitDefaultValue=false)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or Sets City
        /// </summary>
        [DataMember(Name="city", EmitDefaultValue=false)]
        public string City { get; set; }

        /// <summary>
        /// Gets or Sets Region
        /// </summary>
        [DataMember(Name="region", EmitDefaultValue=false)]
        public string Region { get; set; }

        /// <summary>
        /// Gets or Sets PostalCode
        /// </summary>
        [DataMember(Name="postalCode", EmitDefaultValue=false)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or Sets Country
        /// </summary>
        [DataMember(Name="country", EmitDefaultValue=false)]
        public string Country { get; set; }

        /// <summary>
        /// Gets or Sets Phone
        /// </summary>
        [DataMember(Name="phone", EmitDefaultValue=false)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or Sets Fax
        /// </summary>
        [DataMember(Name="fax", EmitDefaultValue=false)]
        public string Fax { get; set; }

        /// <summary>
        /// Gets or Sets Orders
        /// </summary>
        [DataMember(Name="orders", EmitDefaultValue=false)]
        public List<Order> Orders { get; set; }

        /// <summary>
        /// Gets or Sets CustomerTypes
        /// </summary>
        [DataMember(Name="customerTypes", EmitDefaultValue=false)]
        public List<CustomerDemographic> CustomerTypes { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Customer {\n");
            sb.Append("  CustomerId: ").Append(CustomerId).Append("\n");
            sb.Append("  CompanyName: ").Append(CompanyName).Append("\n");
            sb.Append("  ContactName: ").Append(ContactName).Append("\n");
            sb.Append("  ContactTitle: ").Append(ContactTitle).Append("\n");
            sb.Append("  Address: ").Append(Address).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  Region: ").Append(Region).Append("\n");
            sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
            sb.Append("  Country: ").Append(Country).Append("\n");
            sb.Append("  Phone: ").Append(Phone).Append("\n");
            sb.Append("  Fax: ").Append(Fax).Append("\n");
            sb.Append("  Orders: ").Append(Orders).Append("\n");
            sb.Append("  CustomerTypes: ").Append(CustomerTypes).Append("\n");
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
            return this.Equals(input as Customer);
        }

        /// <summary>
        /// Returns true if Customer instances are equal
        /// </summary>
        /// <param name="input">Instance of Customer to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Customer input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.CustomerId == input.CustomerId ||
                    (this.CustomerId != null &&
                    this.CustomerId.Equals(input.CustomerId))
                ) && 
                (
                    this.CompanyName == input.CompanyName ||
                    (this.CompanyName != null &&
                    this.CompanyName.Equals(input.CompanyName))
                ) && 
                (
                    this.ContactName == input.ContactName ||
                    (this.ContactName != null &&
                    this.ContactName.Equals(input.ContactName))
                ) && 
                (
                    this.ContactTitle == input.ContactTitle ||
                    (this.ContactTitle != null &&
                    this.ContactTitle.Equals(input.ContactTitle))
                ) && 
                (
                    this.Address == input.Address ||
                    (this.Address != null &&
                    this.Address.Equals(input.Address))
                ) && 
                (
                    this.City == input.City ||
                    (this.City != null &&
                    this.City.Equals(input.City))
                ) && 
                (
                    this.Region == input.Region ||
                    (this.Region != null &&
                    this.Region.Equals(input.Region))
                ) && 
                (
                    this.PostalCode == input.PostalCode ||
                    (this.PostalCode != null &&
                    this.PostalCode.Equals(input.PostalCode))
                ) && 
                (
                    this.Country == input.Country ||
                    (this.Country != null &&
                    this.Country.Equals(input.Country))
                ) && 
                (
                    this.Phone == input.Phone ||
                    (this.Phone != null &&
                    this.Phone.Equals(input.Phone))
                ) && 
                (
                    this.Fax == input.Fax ||
                    (this.Fax != null &&
                    this.Fax.Equals(input.Fax))
                ) && 
                (
                    this.Orders == input.Orders ||
                    this.Orders != null &&
                    input.Orders != null &&
                    this.Orders.SequenceEqual(input.Orders)
                ) && 
                (
                    this.CustomerTypes == input.CustomerTypes ||
                    this.CustomerTypes != null &&
                    input.CustomerTypes != null &&
                    this.CustomerTypes.SequenceEqual(input.CustomerTypes)
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
                if (this.CustomerId != null)
                    hashCode = hashCode * 59 + this.CustomerId.GetHashCode();
                if (this.CompanyName != null)
                    hashCode = hashCode * 59 + this.CompanyName.GetHashCode();
                if (this.ContactName != null)
                    hashCode = hashCode * 59 + this.ContactName.GetHashCode();
                if (this.ContactTitle != null)
                    hashCode = hashCode * 59 + this.ContactTitle.GetHashCode();
                if (this.Address != null)
                    hashCode = hashCode * 59 + this.Address.GetHashCode();
                if (this.City != null)
                    hashCode = hashCode * 59 + this.City.GetHashCode();
                if (this.Region != null)
                    hashCode = hashCode * 59 + this.Region.GetHashCode();
                if (this.PostalCode != null)
                    hashCode = hashCode * 59 + this.PostalCode.GetHashCode();
                if (this.Country != null)
                    hashCode = hashCode * 59 + this.Country.GetHashCode();
                if (this.Phone != null)
                    hashCode = hashCode * 59 + this.Phone.GetHashCode();
                if (this.Fax != null)
                    hashCode = hashCode * 59 + this.Fax.GetHashCode();
                if (this.Orders != null)
                    hashCode = hashCode * 59 + this.Orders.GetHashCode();
                if (this.CustomerTypes != null)
                    hashCode = hashCode * 59 + this.CustomerTypes.GetHashCode();
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
