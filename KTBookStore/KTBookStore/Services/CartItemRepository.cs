using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using KTBookStore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTBookStore.Services
{
    public class CartItemRepository
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://ktlibrary-aab91-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage firebaseStorage = new FirebaseStorage("ktlibrary-aab91.appspot.com");


        public async Task AddCartItem(string userId, CartItem cartItem)
        {
            await firebaseClient.Child(nameof(CartItem)).Child(userId).PostAsync(JsonConvert.SerializeObject(cartItem));
            
        }

        public async Task AddToCart(string userId, string bookId, CartItem cartItem)
        {
            try
            {
                var existingCartItem = await firebaseClient.Child(nameof(CartItem)).Child(userId).Child(bookId).OnceSingleAsync<CartItem>();

                if (existingCartItem != null)
                {
                    // Nếu cartItem đã tồn tại trong giỏ hàng, tăng số lượng lên 1

                    // Chuyển đổi giá trị của Quantity thành kiểu số nguyên
                    if (int.TryParse(existingCartItem.Quantity, out int quantity))
                    {
                        // Tăng giá trị số nguyên lên 1
                        quantity++;

                        // Cập nhật lại giá trị của Quantity
                        existingCartItem.Quantity = quantity.ToString();

                        // Cập nhật lại giá trị cartItem lên Firebase Realtime Database
                        await firebaseClient.Child(nameof(CartItem)).Child(userId).Child(bookId).PutAsync(existingCartItem);
                    }
                }
                else
                {
                    // Nếu cartItem chưa tồn tại, tạo mới và đặt số lượng là 1
                    cartItem.Quantity = "1";

                    // Lưu cartItem vào Firebase Realtime Database
                    await firebaseClient.Child(nameof(CartItem)).Child(userId).Child(bookId).PutAsync(cartItem);
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khi thêm vào giỏ hàng
                // ...
            }
        }




        // Lấy 2 Key quá khó , xé nhỏ ra , lấy All User đã có giỏ hàng trước , Chọn chỉ định User >> Get Item sau
        public async Task<List<CartItem>> GetAllUserCart()
        {
            var cartItems = await firebaseClient.Child(nameof(CartItem)).OnceAsync<CartItem>();

            var userCartList = new List<CartItem>();

            foreach (var item in cartItems)
            {
                var userId = item.Key;
                var user = await firebaseClient.Child(nameof(UserManager)).Child(userId).OnceSingleAsync<UserManager>();

                var cartItemUser = new CartItem
                {
                    UserId = userId,
                    CustomerName = user?.UserName,
                    UserImage = user?.Image,
                };

                userCartList.Add(cartItemUser);
            }

            return userCartList;
        }

        // Sau khi chỉ định User thông qua userId >> Get Item user đã chọn
        public async Task<List<CartItem>> GetUserItems(string userId)
        {
            var userItems = new List<CartItem>();
            try
            {
                // Lấy danh sách các CartItem của người dùng từ Firebase Realtime Database
                var userCartItems = await firebaseClient.Child(nameof(CartItem)).Child(userId).OnceAsync<CartItem>();

                // Duyệt qua từng CartItem và thêm vào danh sách userItems
                foreach (var item in userCartItems)
                {
                    // Tạo một đối tượng CartItem mới và gán các thuộc tính từ CartItem trong Firebase Realtime Database
                    var cartItem = new CartItem
                    {
                        BookName = item.Object.BookName,
                        Image = item.Object.Image,
                        Price = item.Object.Price,
                        BookId = item.Key,
                        Quantity = item.Object.Quantity,

                        // Các thuộc tính khác của CartItem
                        TotalAmount = item.Object.Price * Convert.ToDouble(item.Object.Quantity), // Tính toán tổng tiền dựa trên giá và số lượng của sản phẩm
                        
                    };

                    userItems.Add(cartItem);

                    
                   
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khi lấy dữ liệu từ Firebase Realtime Database
                // ...
            }

            return userItems;
        }


        // Tính tổng tiền 

    }
}
