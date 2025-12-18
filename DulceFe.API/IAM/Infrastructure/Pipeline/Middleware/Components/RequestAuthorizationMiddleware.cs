using DulceFe.API.IAM.Application.Internal.OutboundServices;
using DulceFe.API.IAM.Domain.Services;

namespace DulceFe.API.IAM.Infrastructure.Pipeline.Middleware.Components;

public class RequestAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public RequestAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserQueryService userQueryService, ITokenService tokenService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            var userId = tokenService.ValidateToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = await userQueryService.Handle(new Domain.Model.Queries.GetUserByIdQuery(userId.Value));
            }
        }
        await _next(context);
    }
}
