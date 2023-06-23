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
    public class BookRepository 
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://ktlibrary-aab91-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage firebaseStorage = new FirebaseStorage("ktlibrary-aab91.appspot.com");
        public async Task<bool> AddBook(BookModel book)
        {
            var data = await firebaseClient.Child(nameof(BookModel)).PostAsync(JsonConvert.SerializeObject(book));
            if (!string.IsNullOrEmpty(data.Key))
            {
                return true;
            }
            return false;
        }

        // Custom Key theo ý , khi add mới kiểu Book + 001 (số tăng dần)
        public async Task<bool> AddBookRuleKey(BookModel book)
        {
            var currentCount = await GetBookCount();
            var bookKey = $"Book{currentCount + 1:D3}"; // Sử dụng "D3" để đảm bảo giá trị tăng dần có đủ ba chữ số (ex: Book001 , chứ không phải Book1)

            var bookRef = firebaseClient.Child(nameof(BookModel)).Child(bookKey);
            await bookRef.PutAsync(book);

            var snapshot = await bookRef.OnceSingleAsync<FirebaseObject<BookModel>>();
            if (snapshot.Object != null)
            {
                return true;
            }

            return false;
        }

        private async Task<int> GetBookCount()
        {
            var bookModelRef = firebaseClient.Child("BookModel");
            var snapshot = await bookModelRef.OnceAsync<BookModel>();
            return snapshot.Count;
        }




        public async Task<List<BookModel>> GetAll()
        {
            return (await firebaseClient.Child(nameof(BookModel)).OnceAsync<BookModel>()).Select(item => new BookModel
            {
                BookName = item.Object.BookName,
                Desc = item.Object.Desc,
                Image = item.Object.Image,
                Author = item.Object.Author,
                Type = item.Object.Type,
                Price = item.Object.Price,
                IdBook = item.Key,
            }).ToList();
        }

        public async Task<List<BookModel>> GetAllByName(string name)
        {
            return (await firebaseClient.Child(nameof(BookModel)).OnceAsync<BookModel>()).Select(item => new BookModel
            {
                BookName = item.Object.BookName,
                Desc = item.Object.Desc,
                Image = item.Object.Image,
                Author = item.Object.Author,
                Type = item.Object.Type,
                Price = item.Object.Price,
                IdBook = item.Key,
            }).Where(c => c.BookName.ToLower().Contains(name.ToLower())).ToList();
        }

        public async Task<BookModel> GetById(string id)
        {
            return (await firebaseClient.Child(nameof(BookModel) + "/" + id).OnceSingleAsync<BookModel>());
        }

        public async Task<bool> Update(BookModel book)
        {
            await firebaseClient.Child(nameof(BookModel) + "/" + book.IdBook).PutAsync(JsonConvert.SerializeObject(book));
            return true;
        }


        public async Task<bool> Delete(string id)
        {
            await firebaseClient.Child(nameof(BookModel) + "/" + id).DeleteAsync();
            return true;
        }

        public async Task<string> Upload(Stream img, string fileName)
        {
            var image = await firebaseStorage.Child("Images").Child(fileName).PutAsync(img);
            return image;
            
        }
    }
}
