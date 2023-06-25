using Firebase.Auth;
using KTBookStore.Models;
using KTBookStore.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KTBookStore.Views.User
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CartPage : ContentPage
    {
        UserRepository userRepository = new UserRepository();
        CartItemRepository cartItemRepository = new CartItemRepository();
        string emailuser = Preferences.Get("userEmail", "default");
        private string userId;
      
        public CartPage()
		{
			InitializeComponent ();
            LoadUser(emailuser);
        }

        private async void LoadUser(string email)
        {
            var userManager = await userRepository.GetUserByEmail(email);
            if (userManager != null)
            {
                _userName.Text = userManager.UserName;
                userId = userManager.UserId;
            }
            LoadUserItems();
        }



        private async void LoadUserItems()
        {
            var userItems = await cartItemRepository.GetUserItems(userId);

            // Tính tổng tiền
            double totalAmount = userItems.Sum(item => item.TotalAmount);

            // Cập nhật giá trị cho TotalAmountLabel
            TotalAmountLabel.Text = totalAmount.ToString("N2");

            UserListView.ItemsSource = userItems;
        }

        private void BuyCart_Clicked(object sender, EventArgs e)
        {

        }
    }
	
}