using System;

namespace chapter08.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual Product Products { get; set; }
    }
}