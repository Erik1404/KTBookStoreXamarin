using Firebase.Auth;
using KTBookStore.Models;
using KTBookStore.Services;
using KTBookStore.Views.Admin;
using KTBookStore.Views.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Firebase.Database;
using System.Data;


namespace KTBookStore.Views.Login_Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignIn : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        public ICommand TapCommand => new Command(async () => await Navigation.PushModalAsync(new Register()));
        public SignIn()
        {
            InitializeComponent();
            bool hasKey = Preferences.ContainsKey("token");
            if (hasKey)
            {
                string token = Preferences.Get("token", "");
                if (!string.IsNullOrEmpty(token))
                {
                    Navigation.PushAsync(new BookList());
                }
            }
        }
        private async void BtnSignIn_Clicked(object sender, EventArgs e)
        {
            UserManager user = new UserManager();
            try
            {
                string email = TxtEmail.Text;
                string password = TxtPassword.Text;
               
                // Truy xuất thông tin người dùng từ Realtime Database

                if (String.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Warning", "Vui lòng nhập Email", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Warning", "Vui lòng nhập password", "Ok");
                    return;
                }
                string token = await _userRepository.SignIn(email, password);
                if (!string.IsNullOrEmpty(token))
                {
                    Preferences.Set("token", token);
                    Preferences.Set("userEmail", email);
                    
                   
                    
                    if (email.EndsWith("@KTStar.com"))
                    {
                        await Navigation.PushAsync(new AdminManager());
                    }
                    else
                    {
                        await Navigation.PushAsync(new HomePage());
                    } 
                }
                else
                    {
                        await DisplayAlert("Thông báo", "Lỗi đăng nhập", "Kiểm tra lại thông tin");
                    }
                }
            catch (Exception exception)
            {
                if (exception.Message.Contains("EMAIL_NOT_FOUND"))
                {
                    await DisplayAlert("Unauthorized", "Email not found", "ok");
                }
                else if (exception.Message.Contains("INVALID_PASSWORD"))
                {
                    await DisplayAlert("Unauthorized", "Password incorrect", "ok");
                }
                else
                {
                    await DisplayAlert("Error", exception.Message, "ok");
                }
            }

        }

        private async void RegisterTap_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Register());
        }

        private async void ForgotTap_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ForgotPassword());
        }
    }
}