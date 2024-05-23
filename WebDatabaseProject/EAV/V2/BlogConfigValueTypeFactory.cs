namespace WebDatabaseProject.EAV;

public class BlogConfigValueTypeFactory
{
    public static ValueTypeBase CreateValueType(ValueTypeId valueTypeId)
    {
        return valueTypeId switch
        {
            ValueTypeId.Int => new IntegerValueType(),
            ValueTypeId.String => new StringValueType(),
            ValueTypeId.Decimal => new DecimalValueType(),
            ValueTypeId.DateTime => new DateTimeValueType(),
            ValueTypeId.Bool => new BoolValueType(),
            ValueTypeId.User => new UserValueType(),
            _ => new UnknownValueType()
        };
    }
}
