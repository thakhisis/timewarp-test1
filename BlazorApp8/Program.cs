using BlazorApp8.Components;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using MediatR;
using MediatR.Pipeline;
using System.Reflection;
using TimeWarp.Features.Persistence;
using TimeWarp.State;
using TimeWarp.State.Plus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddTimeWarpState(options =>
{
	options.UseReduxDevTools
	(
		reduxDevToolsOptions =>
		{
			reduxDevToolsOptions.Name = "BlazorApp8";
			reduxDevToolsOptions.Trace = true;
		}
	);
	options.Assemblies = [
		typeof(Program).GetTypeInfo().Assembly,
		typeof(TimeWarp.State.Plus.AssemblyMarker).GetTypeInfo().Assembly
	];
});

builder.Services.AddTransient(typeof(IRequestPostProcessor<,>), typeof(PersistentStatePostProcessor<,>));
builder.Services.AddScoped<IPersistenceService, PersistenceService>();
builder.Services.AddSingleton(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
