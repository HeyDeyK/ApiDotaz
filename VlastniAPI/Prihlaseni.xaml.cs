using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VlastniAPI
{
    /// <summary>
    /// Interakční logika pro Prihlaseni.xaml
    /// </summary>
    public partial class Prihlaseni : Window
    {
        public Prihlaseni()
        {
            InitializeComponent();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://student.sps-prosek.cz/~vileton15/api.php");
            // Data, která se přidají k POST dotazu -> klíč je typu string a data jsou typu string
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("typ", "prihlaseni"));
            keyValues.Add(new KeyValuePair<string, string>("email", txtEmail.Text));
            keyValues.Add(new KeyValuePair<string, string>("heslo", txtHeslo.Password));
            // Přidání dat do dotazu
            request.Content = new FormUrlEncodedContent(keyValues);
            // Zaslání POST dotazu
            var response = await client.SendAsync(request);
            // Získání odpovědi
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            if(responseContent=="[]")
            {

            }
            else
            {
                Prihlaseni page = new Prihlaseni();
                NavigationService.
            }
        }
    }
}
