using BlazorAutoMediatR;
using BlazorAutoMediatR.Client.Pages;
using BlazorAutoMediatR.Components;
using Domain;
using Domain.Core;
using Domain.DataAccess;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();


// Demo Data. Otherwise never use Singleton
builder.Services.AddSingleton<IDataAccess, DemoDataAccess>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DomainLibraryMediatREntryPoint>());
builder.Services.AddScoped<IMediatorService, MediatorService>();

//builder.Services.AddMediatREndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
});

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
	});
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAntiforgery();


app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorAutoMediatR.Client._Imports).Assembly);

app.Run();
