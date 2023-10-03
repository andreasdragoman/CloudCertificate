using System;
using System.ComponentModel.DataAnnotations;

namespace SBShared.Models
{
    public class PersonModel
    {
        [Required]
        public Guid Id { get; set; }
        //[Required]
        public string FirstName { get; set; }
        //[Required]
        public string LastName { get; set; }
    }
}
