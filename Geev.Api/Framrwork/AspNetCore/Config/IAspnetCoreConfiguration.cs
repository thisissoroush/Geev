using Geev.Api.Framrwork.Models;

namespace Geev.Api.Framrwork.AspNetCore.Config;

public interface IAspnetCoreConfiguration
{
    WrapResultAttribute DefaultWrapResultAttribute { get; }
}
