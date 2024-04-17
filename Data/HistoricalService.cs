using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WattWatcher.Data
{
    public class HistoricalService
    {
        private readonly HttpClient client = new HttpClient();

        public async Task<List<RecordData>> GetHistoricalDataAsync()
        {
            try
            {
                string apiUrl = "https://wattwatcher-pro-default-rtdb.firebaseio.com/averages.json";
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var rawData = JsonConvert.DeserializeObject<Dictionary<string, CircuitDataWrapper>>(responseBody);
                var processedData = rawData.Select(kvp => new RecordData
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(kvp.Key)).DateTime,
                    Circuit1 = kvp.Value.Circuit1,
                    Circuit2 = kvp.Value.Circuit2
                }).ToList();

                return processedData;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
                return new List<RecordData>();
            }
        }

        private class CircuitDataWrapper
        {
            public CircuitData Circuit1 { get; set; }
            public CircuitData Circuit2 { get; set; }
        }
    }
}
