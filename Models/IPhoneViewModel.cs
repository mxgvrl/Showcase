using System.Collections.Generic;

namespace AppleShowcase.Data.Models
{
    public class IPhoneViewModel
    {
        public FilterViewModel Filter { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}