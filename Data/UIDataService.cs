using Newtonsoft.Json;
using WattWatcher.Data;

public class UIDataService
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _baseUri = "https://wattwatcher-pro-default-rtdb.firebaseio.com/";
    private Timer _timer;
    public event Action<List<ElectricModel>> OnDataUpdated;

    public UIDataService()
    {
        _timer = new Timer(Callback, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
    }

    private async void Callback(object state)
    {
        await FetchDataPeriodically();
    }

    private async Task FetchDataPeriodically()
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
            OnDataUpdated?.Invoke(circuits);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in fetching data: {ex.Message}");
        }
    }

    public void StopTimer()
    {
        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();
    }
}
