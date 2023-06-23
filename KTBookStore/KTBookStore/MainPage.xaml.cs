using KTBookStore.Views.Admin;
using KTBookStore.Views.Book;
using KTBookStore.Views.Login_Register;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KTBookStore
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void SignInBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignIn());
        }

    }
}
