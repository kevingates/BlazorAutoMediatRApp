using BlazorAutoMediatR.MediatRPipelines.CustomExceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

namespace BlazorAutoMediatR.MediatRPipelines
{
	public partial class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IAuthorizationService _authorizationService;

		public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
		{
			_httpContextAccessor = httpContextAccessor;
			_authorizationService = authorizationService;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var httpContext = _httpContextAccessor.HttpContext;

			if (httpContext != null)
			{
				var user = httpContext.User;

				var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>(true);

				foreach (var attribute in authorizeAttributes)
				{
					var authorizationResult = await _authorizationService.AuthorizeAsync(user, attribute.Policy);
					if (!authorizationResult.Succeeded)
					{
						// User is not authorized, return default response
						throw new AuthorizationException("User is not authorized to perform this action.");
					}
				}
			}
			else
			{
				return default(TResponse);
			}

			// User is authorized or HttpContext is not available (pre-rendering), continue with request handling
			return await next();
		}
	}
}