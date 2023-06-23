using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using KTBookStore.Services;
using KTBookStore.Models;
using System.Diagnostics;


namespace KTBookStore.Views.Book
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookEdit : ContentPage
    {
        MediaFile file;
        BookRepository bookRepository = new BookRepository();
        public BookEdit(BookModel book)
        {
            InitializeComponent();
            this.BindingContext = book;
        }
        private async void ButtonUpdate_Clicked(object sender, EventArgs e)
        {
            string name = TxtName.Text;
            string author = TxtAuthor.Text;
            string desc = TxtDesc.Text;
            string type = TxtType.Text;
            int price = int.Parse(TxtPrice.Text);
            

            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Cảnh báo", "Vui lòng nhập tên sách", "Quay lại");
            }
            if (string.IsNullOrEmpty(author))
            {
                await DisplayAlert("Cảnh báo", "Vui lòng nhập tác giả", "Quay lại");
            }
            if (string.IsNullOrEmpty(desc))
            {
                await DisplayAlert("Cảnh báo", "Vui lòng nhập mô tả", "Quay lại");
            }
            if (string.IsNullOrEmpty(type))
            {
                await DisplayAlert("Cảnh báo", "Vui lòng nhập mô tả", "Quay lại");
            }
           

            BookModel book = new BookModel();
            book.IdBook = TxtIdBook.Text;
            book.BookName = name;
            book.Author = author;
            book.Desc = desc;
            book.Type = type;
            book.Price = price;

            if (file != null)
            {
                string image = await bookRepository.Upload(file.GetStream(), Path.GetFileName(file.Path));
                book.Image = image;
            }
            bool isUpdated = await bookRepository.Update(book);
            if (isUpdated)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Update failed.", "Cancel");
            }

        }

        private async void ImageTap_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null)
                {
                    return;
                }
                // BookImage là đặt tên ở dòng số 10 bên view x:Name="BookImage"
                BookImage.Source = ImageSource.FromStream(() =>
                {
                    return file.GetStream();
                });
            }
            catch (Exception ex)
            {

            }
        }
    }
}