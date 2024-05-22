namespace WebDatabaseProject.EAV;


public abstract class AttributeV1
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public ValueTypeId Type { get; set; }

    public virtual bool TryParse<T>(string rawValue, out T value)
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
        catch (Exception)
        {
            value = default!;
            return false;
        }
    }

    protected virtual bool IsParsableType<T>()
    {
        Type[] compatibleTypes = Type switch
        {
            ValueTypeId.String => new Type[] { typeof(string) },
            ValueTypeId.Int => new Type[]{
                typeof(int),
                typeof(long)
            },
            ValueTypeId.Decimal => new Type[]{
                typeof(decimal)
            },
            ValueTypeId.DateTime => new Type[]{
                typeof(DateTime),
                typeof(DateTimeOffset)
            },
            ValueTypeId.Bool => new Type[]{
                typeof(bool)
            },
            _ => new Type[] { }
        };

        return compatibleTypes.Contains(typeof(T));
    }
}


