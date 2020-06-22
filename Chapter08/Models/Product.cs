using System.Collections.Generic;

namespace chapter08.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Order> Orders { get; set; }
        public decimal Price { get; set; }
    }
}
