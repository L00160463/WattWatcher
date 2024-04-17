using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WattWatcher.Data
{
    public class AverageService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _baseUri = "https://wattwatcher-pro-default-rtdb.firebaseio.com/";

        public async Task<Dictionary<DateTime, (double AvgWattsCircuit1, double AvgWattsCircuit2, double AvgAmpsCircuit1, double AvgAmpsCircuit2, double AvgKwhCircuit1, double AvgKwhCircuit2)>> GetDailyAveragesAsync()
        {
            string apiUrl = $"{_baseUri}averages.json";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode) return null;

            string json = await response.Content.ReadAsStringAsync();
            var rawData = JsonConvert.DeserializeObject<Dictionary<string, CircuitDataWrapper>>(json);

            var dailyAverages = rawData
                .GroupBy(
                    kvp => DateTimeOffset.FromUnixTimeSeconds(long.Parse(kvp.Key)).UtcDateTime.Date,
                    kvp => kvp.Value,
                    (key, group) => new
                    {
                        Date = key,
                        AverageCircuit1 = group.Average(x => x.Circuit1.AvgWatts),
                        AverageCircuit2 = group.Average(x => x.Circuit2.AvgWatts),
                        AvgAmpsCircuit1 = group.Average(x => x.Circuit1.AvgAmps),
                        AvgAmpsCircuit2 = group.Average(x => x.Circuit2.AvgAmps),
                        AvgKwhCircuit1 = group.Average(x => x.Circuit1.AvgKwh),
                        AvgKwhCircuit2 = group.Average(x => x.Circuit2.AvgKwh)
                    })
                .ToDictionary(
                    x => x.Date,
                    x => (x.AverageCircuit1, x.AverageCircuit2, x.AvgAmpsCircuit1, x.AvgAmpsCircuit2, x.AvgKwhCircuit1, x.AvgKwhCircuit2)
                );

            return dailyAverages;
        }

        private class CircuitDataWrapper
        {
            public CircuitData Circuit1 { get; set; }
            public CircuitData Circuit2 { get; set; }
        }

        private class CircuitData
        {
            public double AvgWatts { get; set; }
            public double AvgAmps { get; set; }
            public double AvgKwh { get; set; }
        }
    }
}

