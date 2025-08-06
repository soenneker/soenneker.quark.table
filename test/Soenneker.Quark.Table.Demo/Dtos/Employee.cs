using System;
using System.Text.Json.Serialization;
using Soenneker.Attributes.MapTo;

namespace Soenneker.Quark.Table.Demo.Dtos
{
    public class Employee
    {
        [JsonPropertyName("Id")]
        [MapTo("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("Name")]
        [MapTo("contact.firstName")]
        public string Name { get; set; } = "";
        
        [JsonPropertyName("Email")]
        [MapTo("Email")]
        public string Email { get; set; } = "";
        
        [JsonPropertyName("Department")]
        [MapTo("Department")]
        public string Department { get; set; } = "";
        
        [JsonPropertyName("Salary")]
        [MapTo("Salary")]
        public decimal Salary { get; set; }
        
        [JsonPropertyName("HireDate")]
        [MapTo("HireDate")]
        public DateTime HireDate { get; set; }
        
        [JsonPropertyName("Status")]
        [MapTo("Status")]
        public string Status { get; set; } = "";
    }
}
