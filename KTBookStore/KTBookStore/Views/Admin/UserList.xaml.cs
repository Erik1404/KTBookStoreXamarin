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
	public partial class UserList : ContentPage
	{
        UserRepository userRepository = new UserRepository();
        public UserList ()
		{
			InitializeComponent ();
            UserListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }

        protected override async void OnAppearing()
        {
            var users = await userRepository.GetAllUser();
            UserListView.ItemsSource = null;
            UserListView.ItemsSource = users;
            UserListView.IsRefreshing = false;

        }

        private void UserListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            var user = e.Item as UserManager;
            //Navigation.PushModalAsync(new BookDetail(book));
            ((ListView)sender).SelectedItem = null;
        }


    }
}