namespace Geev.Api.Framrwork.Metadata;

/// <summary>
/// This class represents the information of a service (or API, App, whatever you call it).
/// More precisely, every piece of code that is RUNNABLE must provide this information internally and externally to outside world.
/// To see how to obtain an instance of this class, see the Framework (TODO)
/// </summary>
public class ServiceInformation
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }

    public string NameVersion => $"{Name},{Version}";

    public DateTime ServerDateTime => DateTime.Now;
}
