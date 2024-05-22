namespace WebDatabaseProject;

using System.ComponentModel.DataAnnotations.Schema;
using WebDatabaseProject.EAV;



public sealed class BlogConfig : IIdentityEntity
{
    public long Id { get; set; }

    public long BlogId { get; set; }

    public string Key { get; set; } = default!;

    public ValueTypeId ValueTypeId { get; set; } = default!;

    public ValueTypeBase ValueType => ValueTypeFactory.CreateValueType(ValueTypeId);

    public string Value { get; set; } = default!;

    public bool TryParse<T>(out T parsedValue)
    {
        return ValueType.TryParse(Value, out parsedValue);
    }
}

