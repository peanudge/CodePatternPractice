using System.Diagnostics;
using System.Text.Json;

namespace WebDatabaseProject.EAV;



public class UserValueType : ValueTypeBase
{
    public override ValueTypeId TypeId { get; set; } = ValueTypeId.User;
    public override string Name { get; set; } = "User";
    public override bool TryParse<T>(string rawValue, out T value)
    {
        if (typeof(T) != typeof(User))
        {
            value = default!;
            return false;
        }
        else
        {
            try
            {
                var blog = JsonSerializer.Deserialize<T>(rawValue);
                value = blog!;
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                value = default!;
                return false;
            }
        }
    }
}
