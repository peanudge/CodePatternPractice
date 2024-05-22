using System.ComponentModel.DataAnnotations;

namespace WebDatabaseProject;


public interface IIdentityEntity
{
    long Id { get; set; }
}

public abstract class IdentityEntity : IIdentityEntity
{
    [Key]
    public long Id { get; set; }
}


public class Blog : IdentityEntity
{
    public string Name { get; set; } = default!;
    public ICollection<BlogConfig> Configs { get; set; } = new List<BlogConfig>();
    public int? Count => GetConfigValue<int>("Count");
    public DateTime? CreatedAt => GetConfigValue<DateTime>("CreatedAt");
    public DateTime? UpdatedAt => GetConfigValue<DateTime>("UpdatedAt");
    public User? User => GetConfigValue<User>("User");


    private T? GetConfigValue<T>(string key)
    {
        var config = Configs.Where(c => c.Key == key).FirstOrDefault();
        if (config != null)
        {
            var isParsed = config.TryParse(out T value);
            if (isParsed)
            {
                return value;
            }
            else
            {
                return default;
            }
        }
        return default;
    }
}

