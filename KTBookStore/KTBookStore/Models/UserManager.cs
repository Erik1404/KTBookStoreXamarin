using System;
using System.Collections.Generic;
using System.Text;

namespace KTBookStore.Models
{
    public class UserManager
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Image { get; set; }


        // Cart Item
        public List<CartItem> CartItems { get; set; } 
    }
}
