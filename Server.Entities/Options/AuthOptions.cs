namespace Server.Entities.Options;
#nullable disable
public class AuthOptions
{
    public ProviderOptions Google { get; set; }
}

public class ProviderOptions
{
    public string Name { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

