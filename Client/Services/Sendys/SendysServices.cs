using System.Net.Http;
using System.Net.Http.Json;

namespace BlazorAuth.Client.Services.Sendys;
public class SendysServices : ISendysServices
{
    private readonly HttpClient httpClient;

    public SendysServices(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task SaveStrings()
    {
        await httpClient.PostAsJsonAsync("/api/employees", "");
    }
}

public interface ISendysServices
{
    Task SaveStrings();
}