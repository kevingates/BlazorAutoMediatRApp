using Domain.Core;
using MediatR;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
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
			var nameWithoutPeriods = type.FullName?.Replace(".", "_");
			var serializedRequest = JsonConvert.SerializeObject(request, new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			});
			var requestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

			// Log or print the serialized request
			Console.WriteLine(serializedRequest);

			// Include the requestName in the URL
			var response = await _httpClient.PostAsync($"MediatR/{nameWithoutPeriods}", requestContent, cancellationToken);

			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<TResponse>(responseContent);
		}
	}
    /*
     * 
     * 
     * OR
     * public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var requestType = request.GetType();
        var namespaceParts = requestType.Namespace?.Split('.') ?? Array.Empty<string>();

        if (namespaceParts.Length < 3 || !namespaceParts[1].EndsWith("Core"))
        {
            throw new ArgumentException("Request must be a command or query from a project with a Core namespace", nameof(request));
        }

        var projectName = namespaceParts[0];
        var endpointName = requestType.Name;
        var route = $"api/{projectName}/{endpointName}";

        var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(route, requestContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request to {route} failed with status code {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseContent);
    }
}
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */
}
