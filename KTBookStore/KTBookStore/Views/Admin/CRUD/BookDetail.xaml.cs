using KTBookStore.Models;
using KTBookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KTBookStore.Views.Book
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookDetail : ContentPage
    {
        BookRepository bookRepository = new BookRepository();
        public BookDetail(BookModel book)
        {
            InitializeComponent();
            BindingContext = book;
        }

        private async void DeleteTapp_Tapped(object sender, EventArgs e)
        {

            var response = await DisplayAlert("Delete", "Do you want to delete?", "Yes", "No");
            if (response)
            {
                string id = ((TappedEventArgs)e).Parameter.ToString();
                bool isDelete = await bookRepository.Delete(id);
                if (isDelete)
                {
                    await DisplayAlert("Information", "Book has been deleted.", "Ok");
                    OnAppearing();
                }
                else
                {
                    await DisplayAlert("Error", "Student deleted failed.", "Ok");
                }
            }
        }

        private async void EditTap_Tapped(object sender, EventArgs e)
        {
            //DisplayAlert("Edit", "Do you want to Edit?", "Ok");

            string id = ((TappedEventArgs)e).Parameter.ToString();
            var book = await bookRepository.GetById(id);
            if (book == null)
            {
                await DisplayAlert("Warning", "Data not found.", "Ok");
            }
            book.IdBook = id;
            await Navigation.PushModalAsync(new BookEdit(book));
        }
    }
}