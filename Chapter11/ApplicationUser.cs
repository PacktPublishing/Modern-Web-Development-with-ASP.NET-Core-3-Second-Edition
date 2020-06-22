using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace chapter07
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [MaxLength(50)]
        public string FullName { get; set; }

        [DataType(DataType.Date)]
        [PersonalData]
        public DateTime? Birthday { get; set; }
    }
}