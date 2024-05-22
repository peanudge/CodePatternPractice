# Entity Attribute Value

Tables: Entity, Attribute, Value

- `Entity` table: data about Item with attribute
- `Attribute` table: shared attributes. it is important when register attribute (if software decide & assign, it is good)
  - AttributeMetadata
- `AttributeValue` table: Interaction table with Entity, Attribute table

Shortly, we can say like `Entity have Attribute as Value`.

# Use case

"Blog" have "Visits" Attribute(type: `int`) as `10` Value.

Client code for EAV data model know what type.

or

Check and decide what to do after check data type

```csharp
var visitConfig = blog.Configs.Where(config => config.Attribute.Name).FirstOrDefault();

var isValidatedConfig = config.IsValid();

var isParsed = config.TryParse<int>(out var value);

if(!isParsed) {
    // Handle exception about existed invalid data .
    return;
}

Console.WriteLIne(value);

```
