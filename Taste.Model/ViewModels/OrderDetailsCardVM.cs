using System;
using System.Collections.Generic;
using System.Text;

namespace Taste.Models.ViewModels
{
    public class OrderDetailsCardVM
    {
        public List<ShoppingCard> ListCard { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
