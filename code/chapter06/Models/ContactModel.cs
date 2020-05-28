using Microsoft.AspNetCore.Mvc;
using System;

namespace chapter06.Models
{
    [ModelMetadataType(typeof(ContactModelMetadata))]
    [Bind("Email", "WorkEmail", "Birthday", "Gender")]
    public class ContactModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string WorkEmail { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; } = true;
        public Location Location { get; set; }
    }
}
