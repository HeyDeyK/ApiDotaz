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
        Uzivatel Uzivatel = new Uzivatel();
        List<Produkt> kosik = new List<Produkt>();
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
            keyValues.Add(new KeyValuePair<string, string>("typ", "zapis"));
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
        public void Save()
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
        private async void Button_Click_1(object sender, RoutedEventArgs e) // Výpis objednávek
        {
            listview.Items.Clear();
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://student.sps-prosek.cz/~vileton15/api.php");
            // Data, která se přidají k POST dotazu -> klíč je typu string a data jsou typu string
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("typ", "vypis"));
            keyValues.Add(new KeyValuePair<string, string>("iduser", Uzivatel.ID));
            // Přidání dat do dotazu
            request.Content = new FormUrlEncodedContent(keyValues);
            // Zaslání POST dotazu
            var response = await client.SendAsync(request);
            // Získání odpovědi
            string responseContent = await response.Content.ReadAsStringAsync();

            // Získání odpovědi v Json
            string json = await response.Content.ReadAsStringAsync();
            List<Objednavka> obj = JsonConvert.DeserializeObject<List<Objednavka>>(json);
            
            //dynamic c = JsonConvert.DeserializeObject<Person>(json);
            foreach(var item in obj)
            {
                listview.Items.Add(item);
            }      

        }
        private async void Button_ClickPrihlaseni(object sender, RoutedEventArgs e) // Prihlaseni 
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://student.sps-prosek.cz/~vileton15/api.php");

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("typ", "prihlaseni"));
            keyValues.Add(new KeyValuePair<string, string>("email", txtEmail.Text));
            keyValues.Add(new KeyValuePair<string, string>("heslo", txtHeslo.Password));

            request.Content = new FormUrlEncodedContent(keyValues);
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            
            if (responseContent != "[]")
            {
                
                Console.WriteLine("Prihlasen");
                //addTab.Visibility = Visibility.Visible;
                vypisTab.Visibility = Visibility.Visible;
                add2Tab.Visibility = Visibility.Visible;
                List<Uzivatel> osoba = JsonConvert.DeserializeObject<List<Uzivatel>>(responseContent);
                Console.WriteLine(osoba[0].ID);
                Uzivatel.email = osoba[0].email;
                Uzivatel.ID = osoba[0].ID;
                Uzivatel.krestni = osoba[0].krestni;
                Uzivatel.prijmeni = osoba[0].prijmeni;
                Uzivatel.telefon = osoba[0].telefon;
            }
            else // žádný výsledek při prihlášení
            {

            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://student.sps-prosek.cz/~vileton15/api.php");
            // Data, která se přidají k POST dotazu -> klíč je typu string a data jsou typu string
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("typ", "objednat"));
            keyValues.Add(new KeyValuePair<string, string>("krestni", Uzivatel.krestni));
            keyValues.Add(new KeyValuePair<string, string>("prijmeni", Uzivatel.prijmeni));
            keyValues.Add(new KeyValuePair<string, string>("iduser", Uzivatel.ID));
            keyValues.Add(new KeyValuePair<string, string>("telefon", Uzivatel.telefon));
            keyValues.Add(new KeyValuePair<string, string>("cena", "20 000"));
            // Přidání dat do dotazu
            request.Content = new FormUrlEncodedContent(keyValues);
            // Zaslání POST dotazu
            var response = await client.SendAsync(request);
            // Získání odpovědi
            string responseContent = await response.Content.ReadAsStringAsync();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://student.sps-prosek.cz/~vileton15/api.php");
            // Data, která se přidají k POST dotazu -> klíč je typu string a data jsou typu string
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("typ", "edit"));
            keyValues.Add(new KeyValuePair<string, string>("id", Uzivatel.ID));
            keyValues.Add(new KeyValuePair<string, string>("krestni", eKrestni.Text));
            keyValues.Add(new KeyValuePair<string, string>("prijmeni", ePrijmeni.Text));
            keyValues.Add(new KeyValuePair<string, string>("telefon", eTelefon.Text));
            keyValues.Add(new KeyValuePair<string, string>("email", eEmail.Text));
            // Přidání dat do dotazu
            request.Content = new FormUrlEncodedContent(keyValues);
            // Zaslání POST dotazu
            var response = await client.SendAsync(request);
            // Získání odpovědi
            string responseContent = await response.Content.ReadAsStringAsync();
        }
        private async void Button_Click_Registrace(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://student.sps-prosek.cz/~vileton15/api.php");
            // Data, která se přidají k POST dotazu -> klíč je typu string a data jsou typu string
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("typ", "registrace"));
            keyValues.Add(new KeyValuePair<string, string>("krestni", regKrestni.Text));
            keyValues.Add(new KeyValuePair<string, string>("prijmeni", regPrijmeni.Text));
            keyValues.Add(new KeyValuePair<string, string>("telefon", regTelefon.Text));
            keyValues.Add(new KeyValuePair<string, string>("email", regEmail.Text));
            keyValues.Add(new KeyValuePair<string, string>("heslo", regHeslo.Text));
            // Přidání dat do dotazu
            request.Content = new FormUrlEncodedContent(keyValues);
            // Zaslání POST dotazu
            var response = await client.SendAsync(request);
            // Získání odpovědi
            string responseContent = await response.Content.ReadAsStringAsync();
        }
        private void Button_Click_Kosik(object sender, RoutedEventArgs e) // vypis kosiku
        {
            
        }
        public void OdeberKosik()
        {

        }
        public void CalKosik()
        {
            int hodnotaKosik = 0;
            listviewKosik.Items.Clear();
            foreach (var item in kosik)
            {
                listviewKosik.Items.Add(item);
                hodnotaKosik += item.cena;
            }
            kosikCena.Text = hodnotaKosik + " korun českoniggerských";
        }
        private void Button_Click_AddToKosik(object sender, RoutedEventArgs e) // vypis kosiku
        {
            Produkt produkt = new Produkt();
            if((sender as Button).CommandParameter.ToString() == "1")
            {
                produkt.nazev = "Apple iPhone X";
                produkt.cena = 32000;
            }
            else if((sender as Button).CommandParameter.ToString() == "2")
            {
                produkt.nazev = "Samsung Galaxy S9";
                produkt.cena = 30000;
            }
            else if ((sender as Button).CommandParameter.ToString() == "3")
            {
                produkt.nazev = "God of War PS4";
                produkt.cena = 1200;
            }
            else if ((sender as Button).CommandParameter.ToString() == "4")
            {
                produkt.nazev = "MSI GeForce RTX 2070";
                produkt.cena = 13990;
            }
            else if ((sender as Button).CommandParameter.ToString() == "5")
            {
                produkt.nazev = "";
                produkt.cena = 32000;
            }
            else if ((sender as Button).CommandParameter.ToString() == "6")
            {
                produkt.nazev = "";
                produkt.cena = 32000;
            }
            else if ((sender as Button).CommandParameter.ToString() == "7")
            {
                produkt.nazev = "";
                produkt.cena = 32000;
            }
            else if ((sender as Button).CommandParameter.ToString() == "8")
            {
                produkt.nazev = "";
                produkt.cena = 32000;
            }
            kosik.Add(produkt);

        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabEdit.IsSelected)
            {
                eKrestni.Text = Uzivatel.krestni;
                ePrijmeni.Text = Uzivatel.prijmeni;
                eTelefon.Text = Uzivatel.telefon;
                eEmail.Text = Uzivatel.email;
            }
            else if(tabKosik.IsSelected)
            {
                listviewKosik.Items.Clear();
                CalKosik();
            }
        }

        private void ListviewKosik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Console.WriteLine((sender as ListView).SelectedItem);
            Console.WriteLine(listviewKosik.SelectedItem);
            kosik.Remove(listviewKosik.SelectedItem as Produkt);
            CalKosik();
            
        }
    }
}
