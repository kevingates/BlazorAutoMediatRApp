using BlazorAutoMediatR.Client;
using Domain.Core;
using MediatR;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("MyHttpClient", client =>
{
	client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/");
});

builder.Services.AddScoped<IMediatorService, MediatorApiService>();

await builder.Build().RunAsync();
