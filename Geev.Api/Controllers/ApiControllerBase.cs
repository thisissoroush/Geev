using MediatR;

using Microsoft.AspNetCore.Mvc;
using Geev.Api.Framrwork.AspNetCore.Mvc.Validation;

namespace Geev.Api.Controllers;


[ValidateModel]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{

    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
