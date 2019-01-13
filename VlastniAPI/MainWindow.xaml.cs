using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace VlastniAPI
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Uzivatel Uzivatel = new Uzivatel();
        List<Produkt> kosik = new List<Produkt>();
        List<string> kosikcisla = new List<string>();
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
                tabEdit.Visibility = Visibility.Visible;
                tabKosik.Visibility = Visibility.Visible;
                tabRegistrace.Width = 0;
                tabRegistrace.Visibility = Visibility.Hidden;
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
            keyValues.Add(new KeyValuePair<string, string>("heslo", eHeslo.Text));
            Uzivatel.krestni = eKrestni.Text;
            Uzivatel.prijmeni = ePrijmeni.Text;
            Uzivatel.telefon = eTelefon.Text;
            Uzivatel.email = eEmail.Text;
            // Přidání dat do dotazu
            request.Content = new FormUrlEncodedContent(keyValues);
            // Zaslání POST dotazu
            var response = await client.SendAsync(request);
            // Získání odpovědi
            string responseContent = await response.Content.ReadAsStringAsync();
        }
        private void Button_ChangePrihlaseni(object sender, RoutedEventArgs e)
        {
            tabLogin.IsSelected = true;
        }
        private void Button_ChangeRegistrace(object sender, RoutedEventArgs e)
        {
            tabRegistrace.IsSelected = true;
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
            tabLogin.IsSelected = true;


        }
        private async void Button_Click_Kosik(object sender, RoutedEventArgs e) // Objednat věci z košíku
        {
            int hodnotaKosik = 0;
            foreach (var item in kosik)
            {
                hodnotaKosik += item.cena;
            }
            string cislaitemu="";
            foreach(var item in kosikcisla)
            {
                cislaitemu = cislaitemu  + item + ";";
            }
            Console.WriteLine(cislaitemu + "  CENA:"+ hodnotaKosik);
            string dnesniDatum = DateTime.Now.ToString("M/d/yyyy");
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://student.sps-prosek.cz/~vileton15/api.php");
            // Data, která se přidají k POST dotazu -> klíč je typu string a data jsou typu string
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("typ", "kosik"));
            keyValues.Add(new KeyValuePair<string, string>("iduser", Uzivatel.ID));
            keyValues.Add(new KeyValuePair<string, string>("polozky", cislaitemu));
            keyValues.Add(new KeyValuePair<string, string>("datum", dnesniDatum));
            keyValues.Add(new KeyValuePair<string, string>("cena", hodnotaKosik.ToString()));
            // Přidání dat do dotazu
            request.Content = new FormUrlEncodedContent(keyValues);
            // Zaslání POST dotazu
            var response = await client.SendAsync(request);
            // Získání odpovědi
            string responseContent = await response.Content.ReadAsStringAsync();
            listviewKosik.Items.Clear();
            hodnotaKosik = 0;
            kosik.Clear();
            kosikCena.Text = hodnotaKosik + " korun českých";
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
            kosikCena.Text = hodnotaKosik + " korun českých";
        }
        private void Button_Click_AddToKosik(object sender, RoutedEventArgs e) // vypis kosiku
        {
            Produkt produkt = new Produkt();
            if((sender as Button).CommandParameter.ToString() == "1")
            {
                produkt.nazev = "Apple iPhone X";
                produkt.cena = 32000;
                kosikcisla.Add("1");
            }
            else if((sender as Button).CommandParameter.ToString() == "2")
            {
                produkt.nazev = "Samsung Galaxy S9";
                produkt.cena = 30000;
                kosikcisla.Add("2");
            }
            else if ((sender as Button).CommandParameter.ToString() == "3")
            {
                produkt.nazev = "God of War PS4";
                produkt.cena = 1200;
                kosikcisla.Add("3");
            }
            else if ((sender as Button).CommandParameter.ToString() == "4")
            {
                produkt.nazev = "MSI GeForce RTX 2070";
                produkt.cena = 13990;
                kosikcisla.Add("4");
            }
            else if ((sender as Button).CommandParameter.ToString() == "5")
            {
                produkt.nazev = "Sony Sluchátka";
                produkt.cena = 800;
                kosikcisla.Add("5");
            }
            else if ((sender as Button).CommandParameter.ToString() == "6")
            {
                produkt.nazev = "Zowie Myš";
                produkt.cena = 1699;
                kosikcisla.Add("6");
            }
            else if ((sender as Button).CommandParameter.ToString() == "7")
            {
                produkt.nazev = "16GB DDR4 RAM";
                produkt.cena = 3899;
                kosikcisla.Add("7");
            }
            else if ((sender as Button).CommandParameter.ToString() == "8")
            {
                produkt.nazev = "Studiový mikrofon";
                produkt.cena = 2699;
                kosikcisla.Add("8");
            }
            kosik.Add(produkt);

        }
        public async void vypisObjednavky()
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
            List<Objednavka2> obj = JsonConvert.DeserializeObject<List<Objednavka2>>(json);

            //dynamic c = JsonConvert.DeserializeObject<Person>(json);
            foreach (var item in obj)
            {
                
                string[] strArray = item.polozky.Split(';');
                string PolozkyVypis = "";
                string newNazev="";
                int pocetProjeti = 1;
                foreach(var polozka in strArray)
                {
                    if(strArray.Length == pocetProjeti)
                    {
                        break;
                    }
                    if(polozka=="1")
                    {
                        newNazev = "Apple iPhone X";
                    }
                    else if(polozka =="2")
                    {
                        newNazev = "Samsung Galaxy S9";
                    }
                    else if (polozka == "3")
                    {
                        newNazev = "God of War PS4";
                    }
                    else if (polozka == "4")
                    {
                        newNazev = "MSI GeForce RTX 2070";
                    }
                    else if (polozka == "5")
                    {
                        newNazev = "Sony Sluchátka";
                    }
                    else if (polozka == "6")
                    {
                        newNazev = "Zowie Myš";
                    }
                    else if (polozka == "7")
                    {
                        newNazev = "16GB DDR4 RAM";
                    }
                    else if (polozka == "8")
                    {
                        newNazev = "Studiový mikrofon";
                    }
                    PolozkyVypis = PolozkyVypis + newNazev + ", ";
                    Console.WriteLine("druhy for");
                    pocetProjeti++;
                }
                
                item.polozky = PolozkyVypis;
                listview.Items.Add(item);
            }
            
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("Tab changed");
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
            else if(vypisTab.IsSelected)
            {
                listview.Items.Clear();
                Console.WriteLine("vypistab selected");
                vypisObjednavky();
                listview.Items.Clear();
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
