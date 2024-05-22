namespace WebDatabaseProject.EAV;

public class DateTimeValueType : PrimitiveValueType
{
    public override ValueTypeId TypeId { get; set; } = ValueTypeId.DateTime;
    public override string Name { get; set; } = "DateTime";

    protected override bool IsParsableType<T>()
    {
        return typeof(DateTime) == typeof(T);
    }
}
