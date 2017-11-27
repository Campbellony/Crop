using System;
using System.Collections.Generic;
using System.Text;

namespace Crop.Models
{
    public class BuyerProduct
    {
        public Buyer Buyer { get; set; }
        public Product Product { get; set; }
        public Price Price { get; set; }
    }
}
