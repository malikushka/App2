using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App2.Платья;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using Xamarin.Forms.Xaml;
using Prism.Navigation;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Платья : ContentPage
    {

        public Платья()
        {
            InitializeComponent();
            var images = new List<string> { "gk.png", "gkfnmt.png" };
            carouselView1.ItemsSource = images;
            NavigationPage.SetHasNavigationBar(this, false);
            UpdateDressQuantity();
            CheckAndUpdateCartQuantity();
          
        }

        private void RefreshData()
        {
            UpdateDressQuantity();
        }

        public void UpdateDressQuantity()
        {
            entry.Text = SelectedProduct.Количество.ToString();
        }
      

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshData();
            CheckAndUpdateCartQuantity();
            UpdateHeartButtonImage();

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

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    carouselView1.Position = (carouselView1.Position + 1) % ((IList)carouselView1.ItemsSource).Count;
                    break;
                case SwipeDirection.Right:
                    carouselView1.Position = Math.Max(0, carouselView1.Position - 1);
                    break;
            }
            int currentIndex = carouselView1.Position;

        }



        public void GetLabelTextFromDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection("Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;"))
            {


                connection.Open();


                string query = "SELECT Название, Описание, Цена, Размер FROM одежда WHERE idОдежда = 1;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string Название = reader["Название"].ToString();
                        string Описание = reader["Описание"].ToString();
                        int Цена = Convert.ToInt32(reader["Цена"]);
                        string Размер = reader["Размер"].ToString();

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            labelDressName.Text = Название;
                            labelDress.Text = Описание;
                            labelPrice.Text = Цена.ToString() + "₽";
                            labelSize.Text = Размер;


                            SelectedProduct.Название = Название;
                            SelectedProduct.Описание = Описание;
                            SelectedProduct.Цена = Цена;
                            SelectedProduct.Размер = Размер;
                        }); ;
                    }
                }
            }


        }

        private void ToggleInfoVisibility_Clicked(object sender, EventArgs e)
        {
            InfoLabel.IsVisible = !InfoLabel.IsVisible;
            labelSize.IsVisible = !InfoLabel.IsVisible;
            labelPrice.IsVisible = !InfoLabel.IsVisible;



        }

        private void DecreaseButton_Clicked(object sender, EventArgs e)
        {
            int productId = 1;
            int userId = 1;

            string connectionString = "Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string getCartIdQuery = $"SELECT idКорзина FROM корзина WHERE Одежда_idОдежда = {productId} AND Покупатель_idПокупатель = {userId}";

                using (MySqlCommand getCartIdCommand = new MySqlCommand(getCartIdQuery, connection))
                {
                    object result = getCartIdCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int cartId = Convert.ToInt32(result);


                        int currentQuantity;
                        string getCurrentQuantityQuery = $"SELECT Количество FROM корзина WHERE idКорзина = {cartId}";

                        using (MySqlCommand getCurrentQuantityCommand = new MySqlCommand(getCurrentQuantityQuery, connection))
                        {
                            currentQuantity = Convert.ToInt32(getCurrentQuantityCommand.ExecuteScalar());
                        }


                        int newQuantity = currentQuantity - 1;

                        if (newQuantity >= 0)
                        {
                            SelectedProduct.Количество = newQuantity;
                            entry.Text = newQuantity.ToString();
                            string updateQuantityQuery = $"UPDATE корзина SET Количество = {newQuantity} WHERE idКорзина = {cartId}";
                            using (MySqlCommand updateQuantityCommand = new MySqlCommand(updateQuantityQuery, connection))
                            {
                                updateQuantityCommand.ExecuteNonQuery();
                            }
                            UpdateDressQuantity();

                            if (newQuantity == 0)
                            {
                                string deleteCartItemQuery = $"DELETE FROM корзина WHERE idКорзина = {cartId}";
                                using (MySqlCommand deleteCartItemCommand = new MySqlCommand(deleteCartItemQuery, connection))
                                {
                                    deleteCartItemCommand.ExecuteNonQuery();
                                }
                                UpdateDressQuantity();
                            }
                        }
                    }
                }
            }

        }
        private void IncreaseButton_Clicked(object sender, EventArgs e)
        {

            int productId = 1;
            int userId = 1;
            int initialQuantity = 0;

            string connectionString = "Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string getCartIdQuery = $"SELECT idКорзина FROM корзина WHERE Одежда_idОдежда = {productId} AND Покупатель_idПокупатель = {userId}";
                int cartId;

                using (MySqlCommand getCartIdCommand = new MySqlCommand(getCartIdQuery, connection))
                {
                    object result = getCartIdCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        cartId = Convert.ToInt32(result);
                    }
                    else
                    {
                        string createCartQuery = $"INSERT INTO корзина (Количество, Покупатель_idПокупатель, Одежда_idОдежда, Одежда_Классификация_idКлассификация, Одежда_Одежда_в_каталоге_idОдежда_в_каталоге, Одежда_Лайки_idЛайки) " +
                         $"VALUES (0, {userId},{productId},1,1,1)";
                        using (MySqlCommand createCartCommand = new MySqlCommand(createCartQuery, connection))
                        {
                            createCartCommand.ExecuteNonQuery();
                        }
                        string getLastInsertedCartIdQuery = "SELECT LAST_INSERT_ID()";
                        using (MySqlCommand getLastInsertedCartIdCommand = new MySqlCommand(getLastInsertedCartIdQuery, connection))
                        {
                            cartId = Convert.ToInt32(getLastInsertedCartIdCommand.ExecuteScalar());
                        }
                    }
                }



                string getCurrentQuantityQuery = $"SELECT Количество FROM корзина WHERE idКорзина = {cartId}";

                using (MySqlCommand getCurrentQuantityCommand = new MySqlCommand(getCurrentQuantityQuery, connection))
                {
                    initialQuantity = Convert.ToInt32(getCurrentQuantityCommand.ExecuteScalar());
                    entry.Text = initialQuantity.ToString();
                }

                int currentQuantity = initialQuantity;
                int newQuantity = currentQuantity + 1;
                if (newQuantity <= 9)
                {
                    SelectedProduct.Количество = newQuantity;
                    entry.Text = newQuantity.ToString();
                    string updateQuantityQuery = $"UPDATE корзина SET Количество = {newQuantity} WHERE idКорзина = {cartId}";

                    using (MySqlCommand updateQuantityCommand = new MySqlCommand(updateQuantityQuery, connection))
                    {
                        updateQuantityCommand.ExecuteNonQuery();
                    }

                    UpdateDressQuantity();
                }

            }
        }

        public void CheckAndUpdateCartQuantity()
        {
            int userId = 1;
            int productId = 1;

            string connectionString = "Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string checkCartItemQuery = $"SELECT COUNT(*) FROM корзина WHERE Покупатель_idПокупатель = {userId} AND Одежда_idОдежда = {productId}";

                using (MySqlCommand checkCartItemCommand = new MySqlCommand(checkCartItemQuery, connection))
                {
                    int count = Convert.ToInt32(checkCartItemCommand.ExecuteScalar());

                    {

                        string getCurrentQuantityQuery = $"SELECT Количество FROM корзина WHERE Покупатель_idПокупатель = {userId} AND Одежда_idОдежда = {productId}";

                        using (MySqlCommand getCurrentQuantityCommand = new MySqlCommand(getCurrentQuantityQuery, connection))
                        {
                            int currentQuantity = Convert.ToInt32(getCurrentQuantityCommand.ExecuteScalar());
                            SelectedProduct.Количество = currentQuantity;
                            entry.Text = currentQuantity.ToString();
                        }
                    }
                }
            }

        }

        public static class SelectedProduct
        {
            public static string Название { get; set; }
            public static string Описание { get; set; }
            public static int Цена { get; set; }
            public static string Размер { get; set; }
            public static int Количество { get; set; }

        }


        private void Button_Clicked_1(object sender, EventArgs e)
        {
            var newPage = new Корзина();
            Navigation.PushAsync(newPage);

        }

        bool isRedHeart = false;
        private void Button_Clicked(object sender, EventArgs e)
        {

            int productId = 1;
            int userId = 1;

            string connectionString = "Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

              
                string checkLikeQuery = $"SELECT idЛайки FROM лайки WHERE Одежда_idОдежда = {productId} AND Покупатель_idПокупатель = {userId}";

                using (MySqlCommand checkLikeCommand = new MySqlCommand(checkLikeQuery, connection))
                {
                    object result = checkLikeCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                       
                        int likeId = Convert.ToInt32(result);
                        string deleteLikeQuery = $"DELETE FROM лайки WHERE idЛайки = {likeId}";
                        using (MySqlCommand deleteLikeCommand = new MySqlCommand(deleteLikeQuery, connection))
                        {
                            deleteLikeCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                       
                        string createCartQuery = $"INSERT INTO лайки (Одежда_Корзина_idКорзина, Покупатель_idПокупатель, Одежда_idОдежда, Одежда_Классификация_idКлассификация, Одежда_Одежда_в_каталоге_idОдежда_в_каталоге, Одежда_Лайки_idЛайки) " +
                            $"VALUES (1, {userId},{productId},1,1,1)";
                        using (MySqlCommand createCartCommand = new MySqlCommand(createCartQuery, connection))
                        {
                            createCartCommand.ExecuteNonQuery();
                        }

                    }
                }
                UpdateHeartButtonImage();
                

            }
        }

        public void UpdateHeartButtonImage()
        {
            int productId = 1;
            int userId = 1;

            string connectionString = "Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

               
                string checkLikeQuery = $"SELECT idЛайки FROM лайки WHERE Одежда_idОдежда = {productId} AND Покупатель_idПокупатель = {userId}";

                using (MySqlCommand checkLikeCommand = new MySqlCommand(checkLikeQuery, connection))
                {
                    object result = checkLikeCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        
                        heartButton.ImageSource = "web_14363790red.png"; 
                    }
                    else
                    {
                       
                        heartButton.ImageSource = "web_14363790.png"; 
                    }
                }
            }
        }


        private void Button_Clicked_2(object sender, EventArgs e)
        {
            var newPage = new Избранное();
            Navigation.PushAsync(newPage);

        }

       
    }



}

       


       
