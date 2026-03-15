public class ChatService
{
    public string GenerateReply(string message)
    {
        message = message.Trim().ToLower();

        if (message.Contains("buy"))
            return "We have properties available <a href='/buy.html'>Buy here</a>";

        if (message.Contains("hi") || message.Contains("hello"))
            return "Hello! Welcome to Haven Real Estate.";

        if (message.Contains("sell"))
            return "List your property <a href='/sell.html'>Sell here</a>";

        if (message.Contains("rent"))
            return "Rental options available <a href='/rent.html'>Rent here</a>";

        return "I’m here to help with real estate questions.";
    }
}