using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WattWatcher.Data
{
    public class AverageService
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

                var data = JsonConvert.DeserializeObject<Dictionary<string, RecordData>>(responseBody);

                return data?.Values.ToList() ?? new List<RecordData>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
                return new List<RecordData>();
            }
        }
    }
}
