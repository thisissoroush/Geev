namespace Geev.Api.Framrwork.Tracing;

public static class OpenTelemetryDefaultSettings
{
    public static List<string> GetDefaultExcludedPaths()
    {
        return new List<string>
            {
                "/health",
                "/index.html",
                "/swagger/v1/swagger.json",
                "/_framework/aspnetcore-browser-refresh.js",
                "/swagger-ui-bundle.js",
                "/swagger-ui-standalone-preset.js",
                "/favicon-32x32.png",
                "/swagger/index.html",
                "/swagger/v1.0/swagger.json",
                "/swagger/v1/swagger.json"
            };
    }


    public static List<List<KeyValuePair<string, string>>> GetFilteredTagGroupsWithoutParent()
    {
        return new()
        {
            new List<KeyValuePair<string, string>>()
               {
                   new("db.system", "redis"),
                   new("db.statement", "SETEX")
               }
        };
    }

    public static List<KeyValuePair<string, string>> GetFilteredTags()
    {
        return new List<KeyValuePair<string, string>>
           {
               new KeyValuePair<string, string>("db.statement", "SELECT 1;")
           };
    }

}
