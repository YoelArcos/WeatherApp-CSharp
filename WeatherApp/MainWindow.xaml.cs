using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;


namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string apiKey = "your_api_Key";
        private string requestUrl = "https://api.openweathermap.org/data/2.5/weather";
        public MainWindow()
        {
            InitializeComponent();
            UpdateData("Dortmund");
        }

        public void UpdateData(string city)
        {

            WeatherMapResponse result = GetWeatherData(city);

            string finalImage = "Sun.png";
            string currentWeather = result.weather[0].main.ToLower();
            string weatherDescription = result.weather[0].description;

            Console.WriteLine("Current weather: " + currentWeather);
            

            if (currentWeather.Contains("clouds"))
            {
                finalImage = "Cloud.png";
            }
            else if (currentWeather.Contains("rain"))
            {
                finalImage = "Rain.png";
            }
            else if (currentWeather.Contains("snow"))
            {
                finalImage = "Snow.png";
            }

            backgroundImage.ImageSource = new BitmapImage(new Uri("Images/WeatherApp_Bilder/" + finalImage, UriKind.Relative));

            labelTemperature.Content = result.main.temp.ToString("F1") + "°C";
            labelInfo.Content = result.weather[0].main;
            labelDescription.Content = weatherDescription;
        

    }
        public WeatherMapResponse GetWeatherData(string city)
        {
            HttpClient httpClient = new HttpClient();

            
            var finalUri = requestUrl + "?q=" + city + "&appid=" + apiKey + "&units=metric";


            // Holt vom HttpClient eine Antwort
            HttpResponseMessage httpResponse = httpClient.GetAsync(finalUri).Result;



            // Die Antwort wird vorübergehend hier gespeichert
            string response = httpResponse.Content.ReadAsStringAsync().Result;

            WeatherMapResponse weatherMapResponse = JsonConvert.DeserializeObject<WeatherMapResponse>(response);

            return weatherMapResponse;
            Console.WriteLine("API response: " + response);
        }

        public void TextBox_TextChanged(object sender, EventArgs e)
        {

        }
        public void Button_Click (object sender, RoutedEventArgs e)
        {
            string query = textBoxQuery.Text;
            UpdateData(query);
        }
    }
}
