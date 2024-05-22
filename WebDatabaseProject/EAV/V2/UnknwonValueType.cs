namespace WebDatabaseProject.EAV;

public class UnknownValueType : ValueTypeBase
{
    public override ValueTypeId TypeId { get; set; } = ValueTypeId.Undefined;
    public override string Name { get; set; } = "Unknown";

    public override bool TryParse<T>(string rawValue, out T value)
    {
        value = default!;
        return false;
    }
}
