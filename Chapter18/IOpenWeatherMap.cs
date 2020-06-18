using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace chapter18
{
    public interface IOpenWeatherMap
    {
        Task<OpenWeatherData> GetByCity(int id);
    }

    public class OpenWeatherMap : IOpenWeatherMap
    {
        //this is for sample data only
        //get your own developer key from https://openweathermap.org/
        private const string _key = "439d4b804bc8187953eb36d2a8c26a02";

        private readonly HttpClient _client;

        public OpenWeatherMap(HttpClient client)
        {
            this._client = client;
        }

        public async Task<OpenWeatherData> GetByCity(int id)
        {
            var response = await this._client.GetStringAsync($"/data/2.5/weather?id=${id}&appid=${_key}");

            var data = JsonSerializer.Deserialize<OpenWeatherData>(response);

            return data;
        }
    }

    public class OpenWeatherCoordinates
    {
        public float lon { get; set; }
        public float lat { get; set; }
    }

    public class OpenWeatherWeather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class OpenWeatherMain
    {
        public float temp { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
    }

    public class OpenWeatherData
    {
        public OpenWeatherCoordinates coord { get; set; }
        public OpenWeatherWeather[] weather { get; set; }
        public string @base { get; set; }
        public OpenWeatherMain main { get; set; }
        public int visibility { get; set; }
        public OpenWeatherWind wind { get; set; }
        public OpenWeatherClouds clouds { get; set; }
        public OpenWeatherSys sys { get; set; }
        public int dt { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }

    public class OpenWeatherSys
    {
        public int type { get; set; }
        public int id { get; set; }
        public float message { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class OpenWeatherClouds
    {
        public int all { get; set; }
    }

    public class OpenWeatherWind
    {
        public float wind { get; set; }
        public int deg { get; set; }
    }
}
