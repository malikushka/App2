using MySqlConnector;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    public partial class App : Application
    {

        public MySqlConnection connections = new MySqlConnection(@"Server=192.168.42.62;Port=3307;Database=cherri;Uid=Malika;Pwd=@Aot_261941;");
        public MySqlCommand command;
        public MySqlDataReader datareader;
        internal object Корзина;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

        }

        protected override void OnStart()
        {
           
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
