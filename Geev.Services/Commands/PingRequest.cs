using MediatR;
using Microsoft.EntityFrameworkCore;
using Geev.Data;
using System.ComponentModel.DataAnnotations;

namespace Geev.Services.Commands;

public class PongResponse
{
    public string Message { get; set; }
}
public class PingCommand : IRequest<PongResponse>
{
    [Required]
    public string UserInput { get; set; }
}


public class PingCommandHandler : IRequestHandler<PingCommand, PongResponse>
{
    private readonly GeevDbContext _dbContext;

    public PingCommandHandler(GeevDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PongResponse> Handle(PingCommand request, CancellationToken cancellationToken)
    {
        
        return new PongResponse { Message = $"Up and Running!" };
    }
}
