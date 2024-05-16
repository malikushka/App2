using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static App2.Платья;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Корзина : ContentPage
	{
        private string randomOrderNumber;
        public Корзина ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            AddToCart();
            UpdateDressQuantity();
            DrawPage();
            CheckAndUpdateCartQuantity();


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
         
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

        private bool IsCartEmpty()
        {
            bool isEmpty = true;

            
            using (MySqlConnection connection = new MySqlConnection("Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;"))
            {
                connection.Open();

                string checkCartQuery = "SELECT COUNT(*) FROM корзина WHERE Покупатель_idПокупатель = 1"; 

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


        public void UpdateDressQuantity()
        {
            entry.Text = SelectedProduct.Количество.ToString();
        }
        public void AddToCart()
        {
            int productId = 1;

            
            string getProductInfoQuery = $"SELECT Одежда.* FROM Корзина JOIN Одежда ON Корзина.Одежда_idОдежда = Одежда.idОдежда WHERE Корзина.Одежда_idОдежда = {productId}";

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

    
        private void DecreaseButton_Clicked(object sender, EventArgs e)
        {
            int productId = 1;
            int userId = 1;

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

                int currentQuantity;
                string getCurrentQuantityQuery = $"SELECT Количество FROM корзина WHERE idКорзина = {cartId}";

                using (MySqlCommand getCurrentQuantityCommand = new MySqlCommand(getCurrentQuantityQuery, connection))
                {
                    currentQuantity = Convert.ToInt32(getCurrentQuantityCommand.ExecuteScalar());
                }

                int newQuantity = currentQuantity - 1;
                if (newQuantity >= 1)
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

        private void IncreaseButton_Clicked(object sender, EventArgs e)
        {
            int productId = 1;
            int userId = 1;

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

                int currentQuantity;
                string getCurrentQuantityQuery = $"SELECT Количество FROM корзина WHERE idКорзина = {cartId}";

                using (MySqlCommand getCurrentQuantityCommand = new MySqlCommand(getCurrentQuantityQuery, connection))
                {
                    currentQuantity = Convert.ToInt32(getCurrentQuantityCommand.ExecuteScalar());
                }

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

        public void UpdateButton()
        {
            int productId = 1;
            int userId = 1;

            string connectionString = "Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();


                string checkLikeQuery = $"SELECT idКорзина FROM корзина WHERE Одежда_idОдежда = {productId} AND Покупатель_idПокупатель = {userId}";

                using (MySqlCommand checkLikeCommand = new MySqlCommand(checkLikeQuery, connection))
                {
                    object result = checkLikeCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        Заказ.IsVisible = true; 
                    }
                    else
                    {
                        Заказ.IsVisible = false; 
                    }
                }
            }
        }

        public void UpdateDressQuantityy(int newQuantity)
        {
            entry.Text = newQuantity.ToString();
        }

        private void RemoveButton_Clicked(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;"))
            {
                connection.Open();

                string deleteCartQuery = $"DELETE FROM корзина WHERE Покупатель_idПокупатель = 1";

                using (MySqlCommand deleteCartCommand = new MySqlCommand(deleteCartQuery, connection))
                {
                    deleteCartCommand.ExecuteNonQuery();
                }
                    DrawPage();
                  UpdateDressQuantityy(0);


            }

           

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Избранное());

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new Пользователь(randomOrderNumber));

        }
        private string GenerateRandomOrderNumber()
        {
           
            Random random = new Random();

          
            int orderNumber = random.Next(100000, 999999);

           
            return orderNumber.ToString();
        }

        private void Заказ_Clicked(object sender, EventArgs e)
        {
            string randomOrderNumber = GenerateRandomOrderNumber();
            
            Navigation.PushAsync(new Пользователь(randomOrderNumber));

        }
    }
}