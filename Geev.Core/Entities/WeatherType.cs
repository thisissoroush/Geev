namespace Geev.Core.Entities;

public partial class WeatherType
{

    public int Id { get; set; }

    public string Title { get; set; }
    public ICollection<WeatherStatus> WeatherStatuses { get; set; }
}