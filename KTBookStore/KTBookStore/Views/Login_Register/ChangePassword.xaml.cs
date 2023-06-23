using KTBookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KTBookStore.Views.Login_Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePassword : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        public ChangePassword()
        {
            InitializeComponent();
        }

        private async void BtnChangePassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                string password = TxtPassword.Text;
                string confirmPass = TxtConfirm.Text;
                if(string.IsNullOrEmpty(password))
                {
                   await DisplayAlert("Thay đổi mật khẩu", "Vui lòng nhập mật khẩu mới","Ok");
                    return;
                }
                if (string.IsNullOrEmpty(confirmPass))
                {
                  await  DisplayAlert("Thay đổi mật khẩu", "Vui lòng nhập lại mật khẩu mới", "Ok");
                    return;
                }
                if(password!=confirmPass)
                {
                    await DisplayAlert("Thay đổi mật khẩu", "Mật khẩu không khớp nhau", "Ok");
                    return;
                }
                string token = Preferences.Get("token","");
                string newToken =await _userRepository.ChangePassword(token, password);
                if(!string.IsNullOrEmpty(newToken))
                {
                    await DisplayAlert("Thay đổi mật khẩu", "Mật khẩu được thay đổi.", "Ok");
                    Preferences.Set("token", newToken);
                    //Preferences.Clear();
                    //await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    await DisplayAlert("Thay đổi mật khẩu", "Không thay đổi thành công.", "Ok");
                }
            }
            catch(Exception exception)
            {

            }
        }
    }
}