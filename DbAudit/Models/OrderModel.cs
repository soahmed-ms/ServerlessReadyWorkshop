using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeDB
{
    class OrderModel
    {
        public string CustomerName { get; set; }
        public long CustomerContactNumber { get; set; }
        public string BaristaId { get; set; }

        public string CustomerEmail { get; set; }

        public IList<ItemModel> OrderItems { get; set; }
        
    }
}
