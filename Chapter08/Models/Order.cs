using System;
using System.Collections.Generic;

namespace chapter08.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}
