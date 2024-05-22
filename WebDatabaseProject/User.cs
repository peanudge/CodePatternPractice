using System.Text.Json;

namespace WebDatabaseProject;

public class User
{
    public string Name { get; set; } = default!;

    public override string ToString()
    {
        return $"User(name: {Name})";
    }
}
