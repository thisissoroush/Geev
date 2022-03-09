
namespace Geev.Core.Entities;

public partial class WeatherStatus
{
    public long Id { get; set; }
    public int TypeId { get; set; }
    public int Value { get; set; }
    public DateTime CreatedOnUTC { get; set; }
    public virtual WeatherType WeatherType { get; set; }
}