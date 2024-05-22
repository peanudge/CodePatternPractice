namespace WebDatabaseProject.EAV;

public class StringValueType : PrimitiveValueType
{
    public override ValueTypeId TypeId { get; set; } = ValueTypeId.String;
    public override string Name { get; set; } = "String";

    protected override bool IsParsableType<T>()
    {
        return typeof(string) == typeof(T);
    }
}
