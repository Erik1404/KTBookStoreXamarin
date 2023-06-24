using System;
using System.Collections.Generic;
using System.Text;

namespace KTBookStore.Models
{
    public class BookModel
    {
        public string IdBook { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }

    }
}
