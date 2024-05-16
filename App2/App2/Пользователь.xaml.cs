using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Пользователь : ContentPage
    {
        public Пользователь(string orderNumber)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            labelOrderNumber.Text = $"Номер вашего заказа: {orderNumber}";
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Избранное());

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Корзина());

        }
    }
   
}