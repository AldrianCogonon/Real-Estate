public static class ChatRoutes
{
    public static void MapRoutes(WebApplication app)
    {
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
    }

    static string GenerateReply(string message)
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

    static bool IsGibberish(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return true;

        int letters = input.Count(char.IsLetter);
        double ratio = (double)letters / input.Length;

        return ratio < 0.6;
    }
}