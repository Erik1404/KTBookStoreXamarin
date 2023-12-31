﻿using KTBookStore.Models;
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



namespace KTBookStore.Views.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage

    {
        UserRepository userRepository = new UserRepository();
        string emailuser = Preferences.Get("userEmail", "default");
        public AccountPage()
        {
            InitializeComponent();
            LoadUser(emailuser);

        }
        private async void LoadUser(string email)
        {

            var userManager = await userRepository.GetUserByEmail(email);
            if (userManager != null)
            {
                _userName.Text = userManager.UserName;
            }
        }

        private void AccountInfoPage_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AccountInfoPage());
        }
    }
}