using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KTBookStore
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Đặt trang gốc là một NavigationPage bao bọc trang Main
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
