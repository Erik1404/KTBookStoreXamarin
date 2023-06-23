using KTBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KTBookStore.Views.Admin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CartItemUserChosen : ContentPage
	{
		public CartItemUserChosen (List<CartItem> userItems)
		{
			InitializeComponent ();
            ItemsListView.ItemsSource = userItems;
        }
	}
}