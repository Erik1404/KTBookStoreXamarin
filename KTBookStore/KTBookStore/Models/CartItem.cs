using System;
using System.Collections.Generic;
using System.Text;

namespace KTBookStore.Models
{
    public class CartItem
    {
        // Book
        public string BookId { get; set; }
        public string BookName { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }


        // Customer
        public string CustomerName { get; set; }
        public string UserId { get; set; }
        public string UserImage { get; set; }
    }
}
