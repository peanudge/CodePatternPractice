namespace WebDatabaseProject.EAV;

public class DecimalValueType : PrimitiveValueType
{
    public override ValueTypeId TypeId { get; set; } = ValueTypeId.Decimal;
    public override string Name { get; set; } = "Real Number";

    protected override bool IsParsableType<T>()
    {
        return typeof(decimal) == typeof(T);
    }
}
