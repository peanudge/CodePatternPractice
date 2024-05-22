namespace WebDatabaseProject.EAV;

public class IntegerValueType : PrimitiveValueType
{
    public override ValueTypeId TypeId { get; set; } = ValueTypeId.Int;
    public override string Name { get; set; } = "Number";

    protected override bool IsParsableType<T>()
    {
        return typeof(int) == typeof(T) || typeof(long) == typeof(T);
    }
}
