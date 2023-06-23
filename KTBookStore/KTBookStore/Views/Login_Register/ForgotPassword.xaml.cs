using KTBookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KTBookStore.Views.Login_Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        public ForgotPassword()
        {
            InitializeComponent();
        }
        private async void ButtonSendLink_Clicked(object sender, EventArgs e)
        {
            string email = TxtEmail.Text;
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Yêu cầu", "Vui lòng nhập Email tài khoản bạn đã đăng ký", "Ok");
                return;
            }

            bool isSend = await _userRepository.ResetPassword(email);
            if (isSend)
            {
                await DisplayAlert("Yêu cầu thay đổi mật khẩu", "Gửi đường dẫn, vui lòng đợi và kiểm tra thư", "Ok");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Yêu cầu thay đổi mật khẩu", "Yêu cầu thất bại", "Ok");
            }
        }
    }
}