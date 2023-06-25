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
                        // Tăng giá trị số nguyên lên 1 , đã thử kiểu  existingCartItem.Quantity += 1; nhưng bất ổn
                        quantity++;

                        // Chuyển đổi lại thành chuỗi và gán vào cartItem.Quantity
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










        // Get all này cần : Tên 1 User >> Tên hàng User đã mua , chú thích hết mình rồi đó
        //public async Task<List<CartItem>> GetAllCartItems()
        //{
        //    // Lấy tất cả các CartItem từ Firebase Realtime Database
        //    var cartItems = await firebaseClient.Child(nameof(CartItem)).OnceAsync<CartItem>();

        //    // Sử dụng phương thức Select để biến đổi mỗi CartItem thành một Task<CartItem>
        //    var result = cartItems.Select(async item =>
        //    {
        //        // Lấy userId từ khóa của CartItem
        //        var userId = item.Key;

        //        // Lấy thông tin người dùng (UserManager) từ Firebase Realtime Database
        //        var user = await firebaseClient.Child(nameof(UserManager)).Child(userId).OnceSingleAsync<UserManager>();

        //        // Tạo một đối tượng CartItem mới và gán các thuộc tính từ CartItem trong Firebase Realtime Database
        //        return (await firebaseClient.Child(nameof(CartItem)).Child(userId).OnceAsync<CartItem>()).Select(itemuserchoose => new CartItem
        //        {
        //            BookName = itemuserchoose.Object.BookName,
        //            Image = itemuserchoose.Object.Image,
        //            Price = itemuserchoose.Object.Price,
        //            BookId = itemuserchoose.Key,
        //            CustomerName = user?.UserName // Lấy tên khách hàng từ thông tin người dùng
        //        });
        //    }).ToList();

        //    // Chờ cho tất cả các tác vụ hoàn thành và chuyển đổi kết quả thành danh sách List<CartItem>
        //    return (await Task.WhenAll(result)).ToList();
        //}


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
                        BookId =  item.Key,
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

    }
}
