using BlazorAutoMediatR.MediatRPipelines;
using MediatR;

namespace BlazorAutoMediatR.MediatRPipelines
{
	public static class MediatrCustomPipelineConfiguration
	{
		public static void AddMediatRPipelines(this IServiceCollection services)
		{
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		}
	}
}
