using System.Text.Json;

public static class AuthRoutes
{
    public static void MapRoutes(WebApplication app)
    {
        app.MapPost("/api/register", async (HttpContext context) =>
        {
            var request = await context.Request.ReadFromJsonAsync<RegisterRequest>();

            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return Results.Json(new { success = false, message = "Invalid input" });
            }

            var users = UserStorage.LoadUsers();

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
            UserStorage.SaveUsers(users);

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

            var users = UserStorage.LoadUsers();

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
    }
}