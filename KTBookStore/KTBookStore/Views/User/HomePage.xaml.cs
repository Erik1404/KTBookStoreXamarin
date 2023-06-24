using KTBookStore.Services;
using KTBookStore.Models;
using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.ObjectModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace KTBookStore.Views.User
    
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
    {
        BookRepository bookRepository = new BookRepository();
        public ObservableRangeCollection<BookModel> Book { get; set; }
        public ObservableRangeCollection<Grouping<string, BookModel>> BookGroups { get; }
        public HomePage ()
		{
			InitializeComponent ();
            string emailuser = Preferences.Get("userEmail", "default");
            BookListView.RefreshCommand = new Xamarin.Forms.Command(() =>
            {
                OnAppearing();
            });

            //Carousel - Begin
            List<SliderModel> slider= new List<SliderModel>()
            {
                new SliderModel(){Title="Title 1", Url="https://media-cdn-v2.laodong.vn/Storage/NewsPortal/2021/11/7/971682/Top-5-Cuon-Sach-Hay-.jpg"},
                new SliderModel(){Title="Title 2", Url="https://admarket.vn/blog/uploads/images/sach-hay-ve-kinh-doanh-ne-doc-de-thanh-cong.jpg"},
            };
            Carousel.ItemsSource = slider;

            Device.StartTimer(TimeSpan.FromSeconds(4), (Func<bool>)(() =>
            {
                Carousel.Position=(Carousel.Position+1)% slider.Count;
                return true;
            }));
            //Carousel - End

            Book = new ObservableRangeCollection<BookModel>();
            BookGroups = new ObservableRangeCollection<Grouping<string, BookModel>>();
        }

        protected override async void OnAppearing()
        {
            Load();
            BookGroups.Count();
        }
        private async void Load()
        {
            var books = await bookRepository.GetAll();

            var typeBook = from b in books
                           orderby b.Type
                           group b by b.Type into grp
                           select new { count = grp.Count() };

            int countType = typeBook.Count();

            if (BookGroups.Count != countType)
            {
                for (int i = 0; i < countType; i++)
                {
                    var book = books[i];
                    BookGroups.Add(new Grouping<string, BookModel>(book.Type, books.Where(c => c.Type == book.Type)));
                }
            }
            BookListView.ItemsSource = BookGroups;
            BookListView.IsRefreshing = false;
        }

        private void BookListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            var bookcheck = e.Item as BookModel;
            string emailuser = Preferences.Get("userEmail", "default"); // lấy từ thằng SignIn qua á
            Navigation.PushModalAsync(new DetailPage(bookcheck, emailuser));
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
    }
}