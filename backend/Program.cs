using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

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

var app = builder.Build();

app.UseCors("FrontendOnly");
app.UseRateLimiter();
app.UseDefaultFiles();
app.UseStaticFiles();

string userFile = "users.json";

if (!File.Exists(userFile))
{
    File.WriteAllText(userFile, "[]");
}

List<User> LoadUsers()
{
    var json = File.ReadAllText(userFile);
    return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
}

void SaveUsers(List<User> users)
{
    var json = JsonSerializer.Serialize(users, new JsonSerializerOptions
    {
        WriteIndented = true
    });

    File.WriteAllText(userFile, json);
}

app.MapPost("/api/register", async (HttpContext context) =>
{
    var request = await context.Request.ReadFromJsonAsync<RegisterRequest>();

    if (request == null ||
        string.IsNullOrWhiteSpace(request.Username) ||
        string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.Json(new { success = false, message = "Invalid input" });
    }

    var users = LoadUsers();

    if (users.Any(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase)))
    {
        return Results.Json(new { success = false, message = "Username already exists" });
    }

    var newUser = new User
    {
        Username = request.Username,
        Email = request.Email,
        Password = request.Password,
        Redirect = "buy.html"
    };

    users.Add(newUser);
    SaveUsers(users);

    Console.WriteLine($"New user registered: {newUser.Username}");

    return Results.Json(new
    {
        success = true,
        message = "Registration successful"
    });
});

app.MapPost("/api/login", async (HttpContext context) =>
{
    var request = await context.Request.ReadFromJsonAsync<LoginRequest>();

    if (request == null ||
        string.IsNullOrWhiteSpace(request.Username) ||
        string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.Json(new { success = false, message = "Invalid credentials" });
    }

if (request.Username.Equals("admin", StringComparison.OrdinalIgnoreCase) &&
    request.Password == "admin123")
{
    return Results.Json(new
    {
        success = true,
        message = "Admin login successful",
        redirect = "admin.html"
    });
}

    var users = LoadUsers();

    var user = users.FirstOrDefault(u =>
        u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase) &&
        u.Password == request.Password);

    if (user != null)
    {
        Console.WriteLine($"Login success: {user.Username}");

        return Results.Json(new
        {
            success = true,
            message = "Login successful",
            redirect = user.Redirect
        });
    }

    return Results.Json(new { success = false, message = "Username or password incorrect" });
});

app.MapPost("/api/chat", async (HttpContext context) =>
{
    var request = await context.Request.ReadFromJsonAsync<UserMessage>();

    if (request == null || string.IsNullOrWhiteSpace(request.Message))
        return Results.BadRequest(new { reply = "Invalid message" });

    if (request.Message.Length > 300)
        return Results.BadRequest(new { reply = "Message too long" });

    string reply = GenerateReply(request.Message);

    return Results.Json(new { reply });

}).RequireRateLimiting("chatLimiter");

app.Run();

string GenerateReply(string message)
{
    message = message.Trim().ToLower();

    if (IsGibberish(message))
        return "Hmm, I didn’t quite understand that Could you rephrase?";

    if (message.Contains("buy"))
        return "We have properties available <a href='/buy.html'>Buy here</a>";

    if (message.Contains("hi") || message.Contains("hello") || message.Contains("hey"))
        return "Hello! Welcome to Haven Real Estate How can I assist you today?";

    if (message.Contains("sell"))
        return "List your property <a href='/sell.html'>Sell here</a>";

    if (message.Contains("rent"))
        return "Rental options available <a href='/rent.html'>Rent here</a>";

    if (message.Contains("contact"))
        return "Contact us at Haven@gmail.com or 092312321";

    if (message.Contains("about"))
        return "Our website helps clients find dream homes efficiently";

    if (message.Contains("help"))
        return "I can assist with buying, selling, or renting properties";

    return "I’m here to help with real estate questions Could you clarify?";
}

bool IsGibberish(string input)
{
    if (string.IsNullOrWhiteSpace(input)) return true;

    int letters = input.Count(char.IsLetter);
    double ratio = (double)letters / input.Length;

    return ratio < 0.6;
}

public class User
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Redirect { get; set; } = null!;
}

public class RegisterRequest
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
}
public class LoginRequest
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
}

public class UserMessage
{
    public string Message { get; set; } = null!;
}