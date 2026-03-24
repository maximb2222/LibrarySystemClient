using LibrarySystem.BlazorClient.Services;
using LibrarySystem.BlazorClient.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton(new LibraryApiClient(
    "http://localhost:5000/LibraryService.svc",
    "net.tcp://localhost:8090/LibraryService.svc"));

builder.Services.AddScoped<SessionService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
