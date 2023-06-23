using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using KTBookStore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTBookStore.Services
{
    public class UserRepository
    {
        string webAPIKey = "AIzaSyBQ4hTdvxSDnh0L9NUPFDiRiZoA8NHoB9o";
        FirebaseStorage firebaseStorage = new FirebaseStorage("ktlibrary-aab91.appspot.com");
        FirebaseClient firebaseClient = new FirebaseClient("https://ktlibrary-aab91-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseAuthProvider authProvider;

        public UserRepository()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        }

        //public async Task<bool> Register(string email, string name, string password, UserManager user)
        //{
        //    var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password, name);
        //    var data = await firebaseClient.Child(nameof(UserManager)).PostAsync(JsonConvert.SerializeObject(user));
        //    if (!string.IsNullOrEmpty(token.FirebaseToken) && !string.IsNullOrEmpty(data.Key))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        // Custom Key theo ý , khi add mới kiểu User + 001 (số tăng dần)
        public async Task<bool> Register(string email, string name, string password, UserManager user)
        {
            var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password, name);
            var currentCount = await GetCurrentUserCount();
            var userKey = $"User{currentCount + 1:D3}"; // Sử dụng "D3" để đảm bảo giá trị tăng dần có đủ ba chữ số (ex: Book001 , chứ không phải Book1)

            var userRef = firebaseClient.Child(nameof(UserManager)).Child(userKey);
            await userRef.PutAsync(user);

            var snapshot = await userRef.OnceSingleAsync<FirebaseObject<UserManager>>();
            if (snapshot.Object != null && !string.IsNullOrEmpty(token.FirebaseToken))
            {
                return true;
            }

            return false;
        }

        private async Task<int> GetCurrentUserCount()
        {
            var UserManagerRef = firebaseClient.Child("UserManager");
            var snapshot = await UserManagerRef.OnceAsync<UserManager>();
            return snapshot.Count;
        }

        public async Task<string> SignIn(string email, string password)
        {
            var token = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return token.FirebaseToken;
            }
            return "";
        }

        public async Task<bool> ResetPassword(string email)
        {
            await authProvider.SendPasswordResetEmailAsync(email);
            return true;
        }

        public async Task<string> ChangePassword(string token, string password)
        {
            var auth = await authProvider.ChangeUserPassword(token, password);
            //await firebaseClient.Child(nameof(UserManager) + "/" + user.UserId).PutAsync(JsonConvert.SerializeObject(user));
            return auth.FirebaseToken;
        }


        // Upload avatar User
        public async Task<string> UploadAvarta(Stream img, string fileName)
        {
            var image = await firebaseStorage.Child("AvartaUser").Child(fileName).PutAsync(img);
            return image;

        }


        // Create 1 cái Realtime database cho User
        public async Task<List<UserManager>> GetAllUser()
        {
            return (await firebaseClient.Child(nameof(UserManager)).OnceAsync<UserManager>()).Select(item => new UserManager
            {
                UserName = item.Object.UserName,
                Email = item.Object.Email,
                Image = item.Object.Image,
                Password = item.Object.Password,
                Role = item.Object.Role,
                ConfirmPassword = item.Object.ConfirmPassword,
                UserId = item.Key,
            }).ToList();
        }

        // Lấy 1 user chỉ định
        public async Task<UserManager> GetUserByEmail(string email)
        {
            var users = await firebaseClient.Child(nameof(UserManager)).OnceAsync<UserManager>();

            var user = users
                .Select(item => new UserManager
                {
                    UserName = item.Object.UserName,
                    Email = item.Object.Email,
                    Image = item.Object.Image,
                    Role = item.Object.Role,
                    Password = item.Object.Password,
                    UserId = item.Key,
                })
                .FirstOrDefault(c => c.Email == email);
            return user;
        }


        public async Task<List<UserManager>> GetUserByRole(string name)
        {
            return (await firebaseClient.Child(nameof(UserManager)).OnceAsync<UserManager>()).Select(item => new UserManager
            {
                UserName = item.Object.UserName,
                Email = item.Object.Email,
                Image = item.Object.Image,
                Role = item.Object.Role,
                Password = item.Object.Password,
                UserId = item.Key,
            }).Where(c => c.Role.ToLower().Contains(name.ToLower())).ToList();
        }

        public async Task<UserManager> GetById(string id)
        {
            return (await firebaseClient.Child(nameof(UserManager) + "/" + id).OnceSingleAsync<UserManager>());
        }



        // KHU VỰC LÀM GIỎ HÀNG
        public async Task AddToCart(string userId, BookModel book)
        {
            var userCartRef = firebaseClient.Child(nameof(CartItem)).Child(userId);
            var userCartSnapshot = await userCartRef.OnceSingleAsync<UserManager>();

            if (userCartSnapshot != null)
            {
                if (userCartSnapshot.CartItems == null)
                {
                    userCartSnapshot.CartItems = new List<CartItem>();
                }

                var cartItem = new CartItem
                {
                    BookId = book.IdBook,
                    BookName = book.BookName,
                    Price = book.Price
                    // Thêm các thông tin khác của sản phẩm vào cartItem
                };

                userCartSnapshot.CartItems.Add(cartItem);

                await userCartRef.PutAsync(userCartSnapshot);
            }
        }

    }
}
