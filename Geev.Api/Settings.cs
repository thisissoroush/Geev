namespace Geev.Api;

public class Settings
{
    public IdentityConfig Identity { get; set; }
}

public class BusConfig
{
    public string ConnectionString { get; set; }
}

public class IdentityConfig
{
    public string Authority { get; set; }
    public bool RequireHttpsMetadata { get; set; }
    public string ApiName { get; set; }
}
