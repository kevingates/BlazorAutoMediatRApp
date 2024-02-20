using Domain.Core;
using MediatR;
using System.Text;
using System.Text.Json;
namespace BlazorAutoMediatR.Client
{
	public class MediatorApiService : IMediatorService
	{
		private readonly HttpClient _httpClient;

		public MediatorApiService(IHttpClientFactory clientFactory)
		{
			_httpClient = clientFactory.CreateClient("MyHttpClient");
		}

		public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
		{
			var type = request.GetType();
			var apiRequest = new ApiRequest() { TypeFullName = type.FullName, Request = (IRequest<dynamic>)request };
			var payload = new { Type = type.FullName, Request = request };
			var requestContent = new StringContent( JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync("Mediator", requestContent, cancellationToken);

			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<TResponse>(responseContent);
		}
	}

}
