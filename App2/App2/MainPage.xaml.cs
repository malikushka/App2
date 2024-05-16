using MySqlConnector;
using Prism.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace App2
{
    public partial class MainPage : ContentPage
    {
        public ICommand ButtonClickedCommand1 { get; }
        public ICommand ButtonClickedCommand2 { get; }
        public ICommand ButtonClickedCommand3 { get; }
        public ICommand ButtonClickedCommand4 { get; }
        public ICommand ButtonClickedCommand5 { get; }
        public ICommand ButtonClickedCommand6 { get; }
        public ICommand ButtonClickedCommand7 { get; }
        public ICommand ButtonClickedCommand8 { get; }

        private string randomOrderNumber;

        public MainPage()
        {
            InitializeComponent();
            var images = new List<string> { "image1.jpg", "image2.jpg", "image3.jpg" };
            carouselView.ItemsSource = images;
            ButtonClickedCommand1 = new Command(ButtonClicked1);
            ButtonClickedCommand2 = new Command(ButtonClicked2);
            ButtonClickedCommand3 = new Command(ButtonClicked3);
            ButtonClickedCommand4 = new Command(ButtonClicked4);
            ButtonClickedCommand5 = new Command(ButtonClicked5);
            ButtonClickedCommand6 = new Command(ButtonClicked6);
            ButtonClickedCommand7 = new Command(ButtonClicked7);
            ButtonClickedCommand8 = new Command(ButtonClicked8);
          
            BindingContext = this;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void ButtonClicked1()
        {
            var newPage = new Платья();
            App app = new App();
            newPage.GetLabelTextFromDatabase();
            newPage.UpdateDressQuantity();
            Navigation.PushAsync(newPage);
        }

        private void ButtonClicked2()
        {
            var newPage = new Верхняя_одежда();
            Navigation.PushAsync(newPage);
        }

        private void ButtonClicked3()
        {
            var newPage = new Блузка();
            Navigation.PushAsync(newPage);
        }

        private void ButtonClicked4()
        {
            var newPage = new Брюки();
            Navigation.PushAsync(newPage);
        }

        private void ButtonClicked5()
        {
            var newPage = new Джинсы();
            Navigation.PushAsync(newPage);
        }

        private void ButtonClicked6()
        {
            var newPage = new Свитера();
            Navigation.PushAsync(newPage);
        }

        private void ButtonClicked7()
        {
            var newPage = new Сумки();
            Navigation.PushAsync(newPage);
        }

        private void ButtonClicked8()
        {
            var newPage = new Портфели();
            Navigation.PushAsync(newPage);
        }

        





        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Left:

                    carouselView.Position = (carouselView.Position + 1) % ((IList)carouselView.ItemsSource).Count;
                    break;
                case SwipeDirection.Right:

                    carouselView.Position = (carouselView.Position == 0) ? ((IList)carouselView.ItemsSource).Count - 1 : carouselView.Position - 1;
                    break;
            }
        }

        [Obsolete]
        private void vk_Clicked(object sender, EventArgs e)
        {
            string vkProfileUrl = "https://vk.com/malikaa234";

            try
            {
                Device.OpenUri(new Uri(vkProfileUrl));
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to open VK profile: " + ex.Message, "OK");
            }

        }

        [Obsolete]
        private void teleg_Clicked(object sender, EventArgs e)
        {
            string vkProfileUrl = "https://t.me/@A6xxx20";

            try
            {
                Device.OpenUri(new Uri(vkProfileUrl));
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to open VK profile: " + ex.Message, "OK");
            }


        }

        [Obsolete]
        private void insta_Clicked(object sender, EventArgs e)
        {
            string vkProfileUrl = "https://instagram.com/kahramanmalika";

            try
            {
                Device.OpenUri(new Uri(vkProfileUrl));
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to open VK profile: " + ex.Message, "OK");
            }



        }

        [Obsolete]
        private void pin_Clicked(object sender, EventArgs e)
        {
            string vkProfileUrl = "https://www.pinterest.com/kahramanmalika";

            try
            {
                Device.OpenUri(new Uri(vkProfileUrl));
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to open VK profile: " + ex.Message, "OK");
            }


        }

        private void user_Clicked(object sender, EventArgs e)
        {
            
        }

        private void Избранное_Clicked(object sender, EventArgs e)
        {
            var newPage = new Избранное();
            Navigation.PushAsync(newPage);

        }

        private void Корзина_Clicked(object sender, EventArgs e)
        {
            var newPage = new Корзина();
            Navigation.PushAsync(newPage);


        }

        private async void Пользователь_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Пользователь(randomOrderNumber));

        }
    }
}