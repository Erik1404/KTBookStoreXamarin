using KTBookStore.Models;
using KTBookStore.Services;
using KTBookStore.Views.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KTBookStore.Views.Admin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BookList : ContentPage
    {
        BookRepository bookRepository = new BookRepository();
        public BookList ()
		{
			InitializeComponent ();
            BookListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }
        protected override async void OnAppearing()
        {
            var books = await bookRepository.GetAll();
            BookListView.ItemsSource = null;
            BookListView.ItemsSource = books;
            BookListView.IsRefreshing = false;

        }

        private void AddToolBarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new BookAdd());
        }

        private void BookListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            var book = e.Item as BookModel;
            Navigation.PushModalAsync(new BookDetail(book));
            ((ListView)sender).SelectedItem = null;

        }
        private async void TxtSearch_SearchButtonPressed(object sender, EventArgs e)
        {
            string searchValue = TxtSearch.Text;
            if (!String.IsNullOrEmpty(searchValue))
            {
                var books = await bookRepository.GetAllByName(searchValue);
                BookListView.ItemsSource = null;
                BookListView.ItemsSource = books;
            }
            else
            {
                OnAppearing();
            }
        }

        private async void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchValue = TxtSearch.Text;
            if (!String.IsNullOrEmpty(searchValue))
            {
                var books = await bookRepository.GetAllByName(searchValue);
                BookListView.ItemsSource = null;
                BookListView.ItemsSource = books;
            }
            else
            {
                OnAppearing();
            }
        }

        private async void EditMenuItem_Clicked(object sender, EventArgs e)
        {
            string id = ((MenuItem)sender).CommandParameter.ToString();
            var book = await bookRepository.GetById(id);
            if (book == null)
            {
                await DisplayAlert("Warning", "Data not found.", "Ok");
            }
            book.IdBook = id;
            await Navigation.PushModalAsync(new BookEdit(book));
        }

        private async void DeleteMenuItem_Clicked(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Delete", "Do you want to delete?", "Yes", "No");
            if (response)
            {
                string id = ((MenuItem)sender).CommandParameter.ToString();
                bool isDelete = await bookRepository.Delete(id);
                if (isDelete)
                {
                    await DisplayAlert("Information", "Book has been deleted.", "Ok");
                    OnAppearing();
                }
                else
                {
                    await DisplayAlert("Error", "Book deleted failed.", "Ok");
                }
            }
        }
    }
}