using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Weather
{
    public partial class MainPage : ContentPage
    {
        private Entry cityEntry;
        private Button getWeatherButton;
        private Label temperatureLabel;
        private Label weatherDescriptionLabel;

        public MainPage()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            cityEntry = new Entry { Placeholder = "Enter city name" };
            getWeatherButton = new Button { Text = "Get Weather" };
            temperatureLabel = new Label { Text = "Temperature: N/A", FontSize = 20 };
            weatherDescriptionLabel = new Label { Text = "Condition: N/A", FontSize = 20 };

            getWeatherButton.Clicked += async (sender, e) => await GetWeatherAsync(cityEntry.Text);

            Content = new VerticalStackLayout
            {
                Padding = 20,
                Children = { cityEntry, getWeatherButton, temperatureLabel, weatherDescriptionLabel }
            };
        }

        public async Task GetWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                await DisplayAlert("Error", "Please enter a city name", "OK");
                return;
            }

            string apiKey = "1a4ac04beec1444cd333892ca69d38ec"; // Replace with your OpenWeather API Key
            string weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(weatherUrl);
                    var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(response);

                    temperatureLabel.Text = $"Temperature: {weatherData.Main.Temp}°C";
                    weatherDescriptionLabel.Text = $"Condition: {weatherData.Weather[0].Description}";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unable to retrieve weather data. Check city name and try again.", "OK");
                Console.WriteLine($"Weather API Error: {ex.Message}");
            }
        }
    }

    public class WeatherResponse
    {
        public Main Main { get; set; }
        public List<Weather> Weather { get; set; }
    }

    public class Main
    {
        public float Temp { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
    }
}
