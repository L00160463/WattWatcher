using Firebase.Database;
using Newtonsoft.Json;
using WattWatcher.Data; 

public class ElectricService
{
    private readonly HttpClient client = new HttpClient();
    private readonly FirebaseClient firebaseClient;
    private Timer timer;

    // Define an event to notify subscribers of new data
    public event Action<int, ElectricModel> OnDataUpdated;

    public ElectricService()
    {
        // Initialize and start the timer to fetch data every second from both circuits
        timer = new Timer(async _ =>
        {
            await FetchDataPeriodically("https://wattwatcher-pro-default-rtdb.firebaseio.com/circuit1.json", 1);
            await FetchDataPeriodically("https://wattwatcher-pro-default-rtdb.firebaseio.com/circuit2.json", 2);
            await FetchDataPeriodically("https://wattwatcher-pro-default-rtdb.firebaseio.com/circuit3.json", 3);
            await FetchDataPeriodically("https://wattwatcher-pro-default-rtdb.firebaseio.com/circuit4.json", 4);

        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }


    private async Task FetchDataPeriodically(string apiUrl, int circuitId)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            ElectricModel electricData = JsonConvert.DeserializeObject<ElectricModel>(responseBody);
            // Invoke the event with circuit ID and data
            OnDataUpdated?.Invoke(circuitId, electricData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching data for circuit {circuitId}: {ex.Message}");
        }
    }

    public async Task<ElectricModel> GetElectricDataAsync()
    {
        try
        {
            string apiUrl = "https://wattwatcher-pro-default-rtdb.firebaseio.com/circuit1.json";
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            ElectricModel electricData = JsonConvert.DeserializeObject<ElectricModel>(responseBody);
            return electricData;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            return null;
        }
    }

    // Call this method to stop the timer, for example, when the application is closing or when you no longer need to fetch data
    public void StopFetchingData()
    {
        timer?.Change(Timeout.Infinite, 0);
        timer?.Dispose();
    }


    










}
