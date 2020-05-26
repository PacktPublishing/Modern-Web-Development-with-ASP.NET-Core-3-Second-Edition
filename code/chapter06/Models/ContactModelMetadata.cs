using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace chapter06.Models
{
    internal abstract class ContactModelMetadata
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [EmailAddress]
        [Required]
        [CustomValidation(typeof(ValidationMethods), "ValidateEmail")]
        public string Email { get; set; }
        [Display(Name = "Work Email", Description = "The work email", Prompt = "Please enter the work email")]
        [EmailAddress]
        public string WorkEmail { get; set; }
        [DisplayFormat(NullDisplayText = "No birthday supplied", DataFormatString = "{0:yyyyMMdd}")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
        [ModelBinder(typeof(GenderModelBinder), Name = "Gender")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [HiddenInput(DisplayValue = false)]
        public bool IsActive { get; set; }
        [UIHint("Location")]
        public Location Location { get; set; }
    }
}