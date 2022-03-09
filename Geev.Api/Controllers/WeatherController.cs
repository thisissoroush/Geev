using Microsoft.AspNetCore.Mvc;
using Geev.Services.Queries;

namespace Geev.Api.Controllers;

public class WeatherController : ApiControllerBase
{
    [HttpGet]
    public async Task<WeatherResponse> GetWeather(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new WeatherQuery(),cancellationToken);
        return result;
    }
   
}
