using BlazorAutoMediatR.Api;
using Domain;
using MediatR;

namespace BlazorAutoMediatR
{
	public static class MediatRControllerSetup
	{

		public static void AddMediatREndpoints(this IServiceCollection services)
		{
			var mediatRAssembly = typeof(DomainLibraryMediatREntryPoint).Assembly;
				
			var requestTypes = mediatRAssembly.GetTypes()
				.Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>)));

			foreach (var requestType in requestTypes)
			{
				var responseType = requestType.GetInterfaces()
					.Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>))
					.GetGenericArguments()[0];

				var controllerType = typeof(MediatRController<,>).MakeGenericType(requestType, responseType);
				services.AddTransient(controllerType);
			}
		}

	}
}
