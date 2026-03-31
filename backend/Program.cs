using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendOnly", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("chatLimiter", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueLimit = 0;
    });
});

VladSetup.AddVladServices(builder);

var app = builder.Build();

app.UseCors("FrontendOnly");
app.UseRateLimiter();
app.UseDefaultFiles();
app.UseStaticFiles();

AuthRoutes.MapRoutes(app);
ChatRoutes.MapRoutes(app);
RequestRoutes.MapRoutes(app);   
PropertyRoutes.MapRoutes(app);

VladSetup.MapVladEndpoints(app);

string usersFile = Path.Combine(builder.Environment.ContentRootPath, "users.json");
app.MapGet("/api/users", () =>
{
    Console.WriteLine($"PATH: {usersFile}");

    if (!File.Exists(usersFile))
    {
        Console.WriteLine("FILE NOT FOUND");
        return Results.Json(new List<object>());
    }

    var json = File.ReadAllText(usersFile);
    return Results.Content(json, "application/json");
});

app.Run();