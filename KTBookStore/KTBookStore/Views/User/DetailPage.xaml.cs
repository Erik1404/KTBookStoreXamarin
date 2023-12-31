﻿using Firebase.Auth;
using KTBookStore.Models;
using KTBookStore.Services;
using KTBookStore.Views.Login_Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KTBookStore.Views.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        BookRepository bookRepository = new BookRepository();
        UserRepository userRepository = new UserRepository();
        CartItemRepository cartItemRepository = new CartItemRepository();
        private string _userEmail;
        private string _UserId;
        //Comment
        public DetailPage(BookModel book, string userEmail)
        {
            InitializeComponent();
            BindingContext = book;
            _userEmail = userEmail;
            LoadUser(_userEmail);
        }

        private async void LoadUser(string email)
        {
            var userManager = await userRepository.GetUserByEmail(email);
            if (userManager != null)
            {
                _UserId = userManager.UserId;
                //userEmailLBL.Text = _UserId;
            }
        }
        void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            double value = e.NewValue;
            _displayLabel.Text = string.Format("{0}", value);
        }
        private async void AddToCart_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var book = button.BindingContext as BookModel;
            var idbook = book.IdBook;
            CartItem cartItem = new CartItem
            {
                BookId = book.IdBook,
                BookName = book.BookName,
                Price = book.Price,
                Image = book.Image,

            };


            var userManager = await userRepository.GetUserByEmail(_userEmail);
            if (userManager != null)
            {
                _UserId = userManager.UserId;
            }
            if (!string.IsNullOrEmpty(_UserId))
            {
                await cartItemRepository.AddToCart(_UserId, idbook, cartItem);

                await DisplayAlert("Thông báo", "Sản phẩm đã được thêm vào giỏ hàng", "OK");
            }
        }
        
    }
}