using System.Text.Json;

public static class UserStorage
{
    static string userFile = "users.json";

    static UserStorage()
    {
        if (!File.Exists(userFile))
        {
            File.WriteAllText(userFile, "[]");
        }
    }

    public static List<User> LoadUsers()
    {
        var json = File.ReadAllText(userFile);
        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }

    public static void SaveUsers(List<User> users)
    {
        var json = JsonSerializer.Serialize(users, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(userFile, json);
    }
}