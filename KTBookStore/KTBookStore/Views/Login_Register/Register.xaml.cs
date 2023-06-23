using KTBookStore.Services;
using Plugin.Media.Abstractions;
using Plugin.Media;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KTBookStore.Models;
using System.Diagnostics;
using System.Security.Cryptography;

namespace KTBookStore.Views.Login_Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        MediaFile file;
        
        public Register()
        {
            InitializeComponent();
        }
        private async void ButtonRegister_Clicked(object sender, EventArgs e)
        {
            try
            {

                string name = TxtName.Text;
                string email = TxtEmail.Text;
                string password = TxtPassword.Text;
                string confirmPassword = TxtConfirmPass.Text;
                string image = await _userRepository.UploadAvarta(file.GetStream(), Path.GetFileName(file.Path));
                string role = string.Empty;
                if (String.IsNullOrEmpty(name))
                {
                    await DisplayAlert("Warning", "Thiếu tên", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Warning", "Thiếu email", "Ok");
                    return;
                }
                if (password.Length < 6)
                {
                    await DisplayAlert("Warning", "Mật khẩu ít nhất 6 ký tự.", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Warning", "Nhập mật khẩu", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(confirmPassword))
                {
                    await DisplayAlert("Warning", "Nhập lại mật khẩu", "Ok");
                    return;
                }
                if (password != confirmPassword)
                {
                    await DisplayAlert("Warning", "Mật khẩu không khớp.", "Ok");
                    return;
                }
                if (email.EndsWith("@KTStar.com"))
                {
                    // Vai trò admin
                    role = "admin";
                }
                else
                {
                    role = "customer";
                }


                UserManager user = new UserManager();
                
                user.UserName = name;
                user.ConfirmPassword = confirmPassword;
                user.Password = password;
                user.Email = email;
                user.Role = role;

                if (file != null)
                {
                    string imageuser = await _userRepository.UploadAvarta(file.GetStream(), Path.GetFileName(file.Path));
                    user.Image = imageuser;
                }
                

                bool isSave = await _userRepository.Register(email, name, password,user);
                if (isSave)
                {
                    await DisplayAlert("Thông báo", "Đăng ký thành công", "Ok");
                    await Navigation.PopModalAsync();
                    Clear();
                }
                else
                {
                    await DisplayAlert("Thông báo", "Đăng ký thất bại", "Ok");
                }
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("EMAIL_EXISTS"))
                {
                    await DisplayAlert("Warning", "Email này đã được đăng ký", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", exception.Message, "Ok");
                }
            }

        }
        // nhập xong xóa sạch textbox
        public void Clear()
        {
            TxtName.Text = string.Empty;
            TxtPassword.Text = string.Empty;
            TxtEmail.Text = string.Empty;
            TxtConfirmPass.Text = string.Empty;
        }

        private async void AvaUserTap_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null)
                {
                    return;
                }

                AvaUser.Source = ImageSource.FromStream(() =>
                {
                    return file.GetStream();
                });
            }
            catch (Exception ex)
            {

            }
        }

    }
}