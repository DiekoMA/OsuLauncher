namespace OsuLauncher.Services;

public interface IOsuService
{
    OsuClient Client { get; set; }
}

public partial class OsuAPIService : ObservableObject, IOsuService
{
    public OsuClient Client { get; set; }

    public OsuAPIService()
    {
        GetClient();
    }

    private async void GetClient()
    {
        Client = new OsuClient();
        Client = await ApiHelper.Instance.RetrieveClient();
    }
}