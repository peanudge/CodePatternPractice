namespace GraphDataStructure;

public class RelayNode : Node
{
    public InputPort StartPort { get; init; }

    public OutputPort EndPort { get; init; }

    public RelayNode(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        StartPort = new InputPort { Name = "Start" };
        EndPort = new OutputPort { Name = "End" };
        InputPorts.Add(StartPort);
        OutputPorts.Add(EndPort);
    }
}
