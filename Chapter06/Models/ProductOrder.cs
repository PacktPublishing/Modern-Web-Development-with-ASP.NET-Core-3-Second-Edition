using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace chapter06.Models
{
    public class ProductOrder : IValidatableObject
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (this.Id <= 0)
            {
                yield return new ValidationResult("Missing id", new[] { "Id" });
            }

            if (this.ProductId <= 0)
            {
                yield return new ValidationResult("Invalid product", new[] { "ProductId" });
            }

            if (this.Quantity <= 0)
            {
                yield return new ValidationResult("Invalid quantity", new[] { "Quantity" });
            }

            if (this.Timestamp > DateTime.Now)
            {
                yield return new ValidationResult("Order date is in the future", new[] { "Timestamp" });
            }
        }
    }
}
