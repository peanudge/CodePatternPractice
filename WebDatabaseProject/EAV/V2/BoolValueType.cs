namespace WebDatabaseProject.EAV;

public class BoolValueType : PrimitiveValueType
{
    public override ValueTypeId TypeId { get; set; } = ValueTypeId.Bool;
    public override string Name { get; set; } = "True/False";

    protected override bool IsParsableType<T>()
    {
        return typeof(bool) == typeof(T);
    }
}
