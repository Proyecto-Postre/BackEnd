using DulceFe.API.IAM.Domain.Services;
using System.Security.Claims;
using System.Collections.Generic;
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
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        var token = authHeader?.Split(" ").Last();

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
                    
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };
                    
                    var identity = new ClaimsIdentity(claims, "Bearer");
                    context.User = new ClaimsPrincipal(identity);
                    Console.WriteLine($"[AUTH] Successfully authenticated user: {user.Username} (ID: {user.Id}) with role {user.Role}");
                }
                else
                {
                    Console.WriteLine($"[AUTH] Token valid for ID {userId}, but user not found in database.");
                }
            }
            else
            {
                Console.WriteLine($"[AUTH] Failed to validate token: {token.Substring(0, Math.Min(token.Length, 15))}...");
            }
        }
        else if (!string.IsNullOrEmpty(authHeader))
        {
             Console.WriteLine("[AUTH] Authorization header found but token could not be extracted.");
        }

        await _next(context);
    }
}
