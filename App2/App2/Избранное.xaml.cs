using MySqlConnector;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static App2.Платья;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Избранное : ContentPage
	{
        private string randomOrderNumber;
        public Избранное ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            AddToCart();
            DrawPage();
            UpdateDressQuantity();
         
        }
        private void RefreshData()
        {
            UpdateDressQuantity();
        }

        public void UpdateDressQuantity()
        {
           
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshData();
           
            bool isDressInFavorites = CheckIfDressIsInFavorites();

            if (isDressInFavorites)
            {
                heartButton.ImageSource = "web_14363790red.png";
            }
            else
            {
                heartButton.ImageSource = "web_14363790.png";
            }

        }
        private void DrawPage()
        {
            bool isCartEmpty = IsCartEmpty();

            if (isCartEmpty)
            {
                stackLayout.IsVisible = false;
               
                scrollView1.IsVisible = true;


                stackLayout.Children.Clear();
            }
            else
            {
                stackLayout.IsVisible = true;
              


                AddToCart();
            }
        }

        private bool IsCartEmpty()
        {
            bool isEmpty = true;


            using (MySqlConnection connection = new MySqlConnection("Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;"))
            {
                connection.Open();

                string checkCartQuery = "SELECT COUNT(*) FROM лайки WHERE Покупатель_idПокупатель = 1";

                using (MySqlCommand checkCartCommand = new MySqlCommand(checkCartQuery, connection))
                {
                    int cartItemCount = Convert.ToInt32(checkCartCommand.ExecuteScalar());


                    if (cartItemCount > 0)
                    {
                        isEmpty = false;
                    }
                }
            }

            return isEmpty;
        }


       
        public void AddToCart()
        {
            int productId = 1;


            string getProductInfoQuery = $"SELECT Одежда.* FROM Лайки JOIN Одежда ON Лайки.Одежда_idОдежда = Одежда.idОдежда WHERE Лайки.Одежда_idОдежда = {productId}";

            using (MySqlConnection connection = new MySqlConnection("Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;"))
            {
                connection.Open();

                MySqlCommand getProductInfoCommand = new MySqlCommand(getProductInfoQuery, connection);

                using (MySqlDataReader reader = getProductInfoCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        string productName = reader["Название"].ToString();
                        string productDescription = reader["Описание"].ToString();
                        decimal productPrice = Convert.ToDecimal(reader["Цена"]);
                        string productSize = reader["Размер"].ToString();

                        labelDressName.Text = productName;
                        labelDress.Text = productDescription;
                        labelPrice.Text = productPrice.ToString() + "₽";
                        labelSize.Text = productSize;
                    }
                }
            }
        }


        private void heartButton_Clicked(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;"))
            {
                connection.Open();

                string deleteCartQuery = $"DELETE FROM лайки WHERE Покупатель_idПокупатель = 1";

                using (MySqlCommand deleteCartCommand = new MySqlCommand(deleteCartQuery, connection))
                {
                    deleteCartCommand.ExecuteNonQuery();
                }
                DrawPage();
            }
            if (heartButton != null)
            {
                heartButton.ImageSource = "web_14363790.png";
            }


        }

        public bool CheckIfDressIsInFavorites()
        {
            int productId = 1;
            int userId = 1;

            string connectionString = "Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;";
            string checkFavoriteQuery = $"SELECT COUNT(*) FROM лайки WHERE Одежда_idОдежда = {productId} AND Покупатель_idПокупатель = {userId}";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand checkFavoriteCommand = new MySqlCommand(checkFavoriteQuery, connection))
                {
                    int count = Convert.ToInt32(checkFavoriteCommand.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Корзина());

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Пользователь(randomOrderNumber));

        }
    }
}