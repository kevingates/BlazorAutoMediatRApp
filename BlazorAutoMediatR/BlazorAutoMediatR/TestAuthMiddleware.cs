using System.Security.Claims;

namespace BlazorAutoMediatR
{
	public class TestAuthMiddleware
	{
		private readonly RequestDelegate _next;

		public TestAuthMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, "username"),
			new Claim(ClaimTypes.Role, "Admin") // Add a role claim
        };

			var identity = new ClaimsIdentity(claims, "Test");
			var principal = new ClaimsPrincipal(identity);

			context.User = principal; // Set the user of the current HTTP context

			await _next(context);
		}
	}
}
