using System.Text.Json;

public class UserService
{
    private readonly string userFile = "users.json";

    public UserService()
    {
        if (!File.Exists(userFile))
        {
            File.WriteAllText(userFile, "[]");
        }
    }

    public List<User> LoadUsers()
    {
        var json = File.ReadAllText(userFile);
        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }

    public void SaveUsers(List<User> users)
    {
        var json = JsonSerializer.Serialize(users, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(userFile, json);
    }
}