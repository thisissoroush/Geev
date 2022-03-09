using Geev.Api.Framrwork.Models;

namespace Geev.Api.Framrwork.AspNetCore.Config;

public class AspnetCoreConfiguration : IAspnetCoreConfiguration
{
    public WrapResultAttribute DefaultWrapResultAttribute { get; }

    public AspnetCoreConfiguration()
    {
        DefaultWrapResultAttribute = new WrapResultAttribute();
    }
}
