using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WattWatcher.Data
{
    public class UIDataService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _baseUri = "https://wattwatcher-pro-default-rtdb.firebaseio.com/";
        public event Action<List<ElectricModel>> OnDataUpdated;

        public UIDataService()
        {
            FetchDataPeriodically(); // Start fetching data immediately upon initialization
        }

        public async void FetchDataPeriodically()
        {
            while (true) // Continuously fetch data
            {
                var circuits = new List<ElectricModel>();
                try
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        string response = await _httpClient.GetStringAsync($"{_baseUri}circuit{i}.json");
                        var data = JsonConvert.DeserializeObject<ElectricModel>(response);
                        if (data != null)
                        {
                            circuits.Add(data);
                        }
                    }
                    OnDataUpdated?.Invoke(circuits); // Invoke the updated data event
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in fetching data: {ex.Message}");
                }
                await Task.Delay(1000); // Wait for 10 seconds before fetching the data again
            }
        }
    }
}
