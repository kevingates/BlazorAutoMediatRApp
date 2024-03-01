namespace BlazorAutoMediatR
{
	public static class AuthorizationPolicies
	{
		public static void AddAuthorizationPolicies(this IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				options.AddPolicy("Admin", policy =>
					policy.RequireRole("Admin"));

				options.AddPolicy("CEO", policy =>
					policy.RequireRole("CEO"));
			});
		}
	}
}
