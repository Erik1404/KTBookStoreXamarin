using KTBookStore.Models;
using KTBookStore.Services;
using KTBookStore.Views.Login_Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KTBookStore.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminManager : ContentPage
    {
        UserRepository userRepository = new UserRepository();
        public AdminManager()
        {
            InitializeComponent();
            string emailuser = Preferences.Get("userEmail", "default");
            LoadUser(emailuser);
            
        }
        private async void LoadUser(string email)
        {
            var userManager = await userRepository.GetUserByEmail(email);
            if (userManager != null)
            {
                LbNameUser.Text = userManager.UserName;
                NameUser.Text = userManager.UserName;
                imgUserLogin.Source = userManager.Image;
            }
        }

        private void ShowListBook_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BookList());
        }

        private void ShowListCart_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CartItemList());
        }

        private void ShowListUser_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserList());
        }
    }
}