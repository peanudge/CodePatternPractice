namespace WebDatabaseProject.EAV;


public abstract class ValueTypeBase
{
    public abstract ValueTypeId TypeId { get; set; }
    public abstract string Name { get; set; }
    public abstract bool TryParse<T>(string rawValue, out T value);

    // TODO: Add Type Metadata
    // for validation
    // for parsing
    // for presentation
    // for range of normal values
    // for grouping
}
