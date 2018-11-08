using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Get();
            
            //Save();
        }
        public async void Send()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://student.sps-prosek.cz/~vileton15/api.php");

            Person person = new Person
            {
                name = "Alfred",
                surname = "Rýgr"
            };
            var myContent = JsonConvert.SerializeObject(person);
            var f = new Person
            {
                name = "Alfred",
                surname = "Rýgr"
            };
            // Data, která se přidají k POST dotazu -> klíč je typu string a data jsou typu string
            var keyValues = new List<KeyValuePair<string, string>>();
            string somedata = txtbox_krestni.Text;
            string somedata2 = txtbox_prijmeni.Text;
            string somedata3 = txtbox_cena.Text;
            string somedata4 = txtbox_adresa.Text;
            string somedata5 = txtbox_telefon.Text;
            keyValues.Add(new KeyValuePair<string, string>("krestni", somedata));
            keyValues.Add(new KeyValuePair<string, string>("prijmeni", somedata2));
            keyValues.Add(new KeyValuePair<string, string>("cena", somedata3));
            keyValues.Add(new KeyValuePair<string, string>("adresa", somedata4));
            keyValues.Add(new KeyValuePair<string, string>("telefon", somedata5));

            // Přidání dat do dotazu
            request.Content = new FormUrlEncodedContent(keyValues);

            // Zaslání POST dotazu
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
        }
        public async void Get()
        {
            // Vytvoření klienta
            HttpClient client = new HttpClient();

            // Odeslání dotazu na API + pamaretr pro výpis z kategorie dev
            var response = await client.GetAsync("https://student.sps-prosek.cz/~vileton15/json.json");

            // Získání odpovědi v Json
            string json = await response.Content.ReadAsStringAsync();

            // Deserializace na dynamic objekt
            dynamic c = JsonConvert.DeserializeObject<Person>(json);

            // Vypsání z Dynamic
            string joke = c.name;

            //Text.Content = joke;
        }
        public async void Save()
        {
            Person person = new Person
            {
                name = "Petr",
                surname = "Rýgr"
            };
            // serialize JSON to a string and then write string to a file
            File.WriteAllText(@"D:\person.json", JsonConvert.SerializeObject(person));

            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(@"D:\person.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, person);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Send();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            listview.Items.Clear();
            HttpClient client = new HttpClient();

            // Odeslání dotazu na API + pamaretr pro výpis z kategorie dev
            var response = await client.GetAsync("https://student.sps-prosek.cz/~vileton15/api.php");

            // Získání odpovědi v Json
            string json = await response.Content.ReadAsStringAsync();
            List<Objednavka> obj = JsonConvert.DeserializeObject<List<Objednavka>>(json);
            
            //dynamic c = JsonConvert.DeserializeObject<Person>(json);
            foreach(var item in obj)
            {
                listview.Items.Add(item);
            }      
        }
    }
}
