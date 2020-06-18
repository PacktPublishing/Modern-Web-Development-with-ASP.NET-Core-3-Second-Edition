using System.ComponentModel.DataAnnotations;

namespace chapter04.Models
{
    public class Model
    {
        [Required]
        public string RequiredProperty { get; set; }
    }
}
