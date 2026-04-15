using DulceFe.API.IAM.Domain.Services;
using System.Security.Claims;
using DulceFe.API.IAM.Domain.Model.Queries;

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
                var user = await userQueryService.Handle(new GetUserByIdQuery(userId.Value));
                if (user != null)
                {
                    // attach user to context items
                    context.Items["User"] = user;
                    
                    // CRITICAL: attach user to ClaimsPrincipal so Controller.User works
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username)
                    };
                    var identity = new ClaimsIdentity(claims, "Bearer");
                    context.User = new ClaimsPrincipal(identity);
                }
            }
        }
        await _next(context);
    }
}
