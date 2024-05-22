using System.Diagnostics;

namespace WebDatabaseProject.EAV;

/// <summary>
/// https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible?view=net-6.0
/// Attribute for type derived IConvertible interface is PrimitiveAttribute
/// </summary>
public abstract class PrimitiveValueType : ValueTypeBase
{
    protected abstract bool IsParsableType<T>();

    public override bool TryParse<T>(string rawValue, out T value)
    {
        var isValidType = IsParsableType<T>();
        if (!isValidType)
        {
            value = default!;
            return false;
        }

        try
        {
            value = (T)Convert.ChangeType(rawValue, typeof(T));
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
