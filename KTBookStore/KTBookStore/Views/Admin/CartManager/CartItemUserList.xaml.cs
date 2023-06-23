using KTBookStore.Models;
using KTBookStore.Services;
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
	public partial class CartItemList : ContentPage
	{
        CartItemRepository cartItemRepository = new CartItemRepository();
        List<CartItem> selectedUserItems = new List<CartItem>(); // Danh sách các mục người dùng đã chọn
        public CartItemList ()
		{
			InitializeComponent ();
            UserListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }


        protected override async void OnAppearing()
        {
            var cartusers = await cartItemRepository.GetAllUserCart();
            UserListView.ItemsSource = null;
            UserListView.ItemsSource = cartusers;
            UserListView.IsRefreshing = false;
        }

        private async void UserListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Lấy người dùng đã chọn
            var selectedUser = e.Item as CartItem;

            // Kiểm tra xem người dùng đã chọn có hợp lệ không
            if (selectedUser != null)
            {
                // Lấy các mục của người dùng đã chọn
                selectedUserItems = await cartItemRepository.GetUserItems(selectedUser.UserId);

                // Chuyển hướng sang trang hiển thị các mục của người dùng
                await Navigation.PushAsync(new CartItemUserChosen(selectedUserItems));
            }
        }

    }
}