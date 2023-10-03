using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SBShared.Models
{
    public class PersonModel
    {
        [Required]
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        //[Required]
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        //[Required]
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
    }
}
