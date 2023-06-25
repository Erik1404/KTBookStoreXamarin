using KTBookStore.Models;
using KTBookStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KTBookStore.Views.Book
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookAdd : ContentPage
    {
        BookRepository bookRepository = new BookRepository();
        public BookAdd()
        {
            InitializeComponent();
        }

        private async void ButtonSave_Clicked(object sender, EventArgs e)
        {
            string name = TxtName.Text;
            string author = TxtAuthor.Text;
            string desc = TxtDesc.Text;
            string type = TxtType.Text;
            int quantity = int.Parse(TxtQuantity.Text);
            int price = int.Parse(TxtPrice.Text);

            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Cảnh báo", "Vui lòng nhập tên sách", "Quay lại");
            }
            if (string.IsNullOrEmpty(author))
            {
                await DisplayAlert("Cảnh báo", "Vui lòng nhập tác giả", "Quay lại");
            }
            if (string.IsNullOrEmpty(desc))
            {
                await DisplayAlert("Cảnh báo", "Vui lòng nhập mô tả", "Quay lại");
            }
            if (string.IsNullOrEmpty(type))
            {
                await DisplayAlert("Cảnh báo", "Vui lòng nhập thể loại", "Quay lại");
            }
            if (string.IsNullOrEmpty(desc))
            {
                await DisplayAlert("Cảnh báo", "Vui lòng nhập giá sách", "Quay lại");
            }


            BookModel book = new BookModel();
            
            book.BookName = name;
            book.Author = author;
            book.Desc = desc;
            book.Quantity = quantity;
            book.Type = type;
            book.Price = price;
            

            var isSaved = await bookRepository.AddBookRuleKey(book);
            if (isSaved)
            {
                await DisplayAlert("Thông tin", "Thêm thành công.", "Ok");
                Clear();
            }
            else
            {
                await DisplayAlert("Cảnh báo","Lỗi thêm", "Ok");
            }        
        }


        // nhập xong xóa sạch textbox
        public void Clear()
        {
            TxtName.Text = string.Empty;
            TxtAuthor.Text = string.Empty;
            TxtDesc.Text = string.Empty;
            TxtType.Text = string.Empty;
            TxtQuantity.Text = string.Empty;
            TxtPrice.Text = string.Empty;
        }
    }
}