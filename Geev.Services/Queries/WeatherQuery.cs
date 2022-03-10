using MediatR;
using Microsoft.EntityFrameworkCore;
using Geev.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Geev.Core.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Geev.Services.Queries;




public class WeatherQuery : IRequest<WeatherResponse>
{


}
public class WeatherResponse
{
    public string Title { get; set; }
    public int Value { get; set; }
}

public class WeatherQueryHandler : IRequestHandler<WeatherQuery, WeatherResponse>
{
    private readonly GeevDbContext _dbContext;
    private readonly string _weatherEndPoint;
    private IMemoryCache _cache;
    private readonly string _weatherCacheKey = "weather";

    public WeatherQueryHandler(
        GeevDbContext GeevDbContext,
        IConfiguration configuration,
        IMemoryCache cache)
    {
        _dbContext = GeevDbContext;
        _weatherEndPoint = configuration["WeatherApiEndPoint"];
        _cache = cache;
    }

    public async Task<WeatherResponse> Handle(WeatherQuery request, CancellationToken cancellationToken)
    {
        return await GetWeatherAsync(request, cancellationToken);
    }

    private async Task<WeatherResponse> GetWeatherAsync(WeatherQuery request, CancellationToken cancellationToken)
    {
        var data = await GetWeatherResponseFromAPI(cancellationToken);
        var response = new WeatherResponse();
        if (data != null)
        {
            try
            {
                var weatherType = _dbContext.WeatherType.FirstOrDefault(e => e.Title.Equals(data.Title));

                if (weatherType == null)
                    weatherType = new WeatherType() { Title = data.Title };

                var WeatherStatus = new WeatherStatus()
                {
                    TypeId = weatherType.Id,
                    Value = data.Value,
                    CreatedOnUTC = DateTime.UtcNow,
                    WeatherType = weatherType
                };

                await _dbContext.WeatherStatus.AddAsync(WeatherStatus);
                await _dbContext.SaveChangesAsync();

                response.Value = data.Value;
                response.Title = weatherType.Title;
            }
            catch (Exception)
            {
                Console.WriteLine("Db Failure!");
            }
        }
        else
        {
            _cache.TryGetValue<WeatherResponse>(_weatherCacheKey, out response);

            if (response.Title == null)
                try
                {
                    response = _dbContext.WeatherStatus
                   .Include(w => w.WeatherType)
                   .AsNoTracking()
                   .OrderByDescending(c => c.CreatedOnUTC)
                   .Select(c => new WeatherResponse() { Title = c.WeatherType.Title, Value = c.Value })
                   .FirstOrDefault();
                }
                catch (Exception)
                {
                    Console.WriteLine("Db Failure");
                }
               

        }

        if (response != null)
            _cache.Set("weather", response); //better to store as binary

        return response;
    }

    private async Task<WeatherResponse> GetWeatherResponseFromAPI(CancellationToken cancellationToken)
    {
        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);

            var apiResponse = await client.GetAsync(_weatherEndPoint, cancellationToken);

            if (apiResponse.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<WeatherResponse>(apiResponse.Content.ReadAsStringAsync().Result.ToString());
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Connection to EndPoint not established.");
        }

        return null;
    }



}