using System;
using System.Collections.Generic;

namespace gajabi.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string? Catgory { get; set; }
        public string? Namep { get; set; }
        public int? Price { get; set; }
        public string? Patimg { get; set; }
        public int? Stait { get; set; }
    }
}
