using System.Collections.Generic;

namespace AppleShowcase.Data.Models
{
    public class WatchViewModel
    {
        public FilterViewModel Filter { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}