using System.Text.Json.Serialization;

namespace GraphDataStructure;

public class NodeGraph
{
    public List<Node> Nodes { get; set; } = new();
    public List<NodeLink> Links { get; set; } = new();

    [JsonConstructor]
    public NodeGraph()
    {
    }

    public NodeGraph(IList<Node> nodes, IList<NodeLink> links)
    {
        Nodes.AddRange(nodes);
        Links.AddRange(links);
    }

    public List<NodeLink> FindOutgoingLinksFrom(Guid srcNodeId, string srcPortName)
    {
        return Links.Where(link => link.SrcNodeId == srcNodeId && link.SrcPortName == srcPortName)
            .ToList();
    }

    public List<NodeLink> FindIncomingLinksTo(Guid destNodeId, string destPortName)
    {
        return Links.Where(link => link.DestNodeId == destNodeId && link.DestPortName == destPortName)
            .ToList();
    }
}

public enum NodeType
{
    Entry = 0,
    Exit = 1,
    Action = 2,
    Conditional = 3,
}

public class Node
{
    public Guid Id { get; set; }
    public NodeType Type { get; set; } = NodeType.Action;
    public string Name { get; set; } = default!;
    public List<InputPort> InputPorts { get; set; } = new();
    public List<OutputPort> OutputPorts { get; set; } = new();

    public bool AddInputPort(InputPort newPort)
    {
        if (InputPorts.Where(port => port.Name == newPort.Name).Any())
        {
            return false;
        }
        InputPorts.Add(newPort);
        return true;
    }

    public bool AddOutputPort(OutputPort newPort)
    {
        if (OutputPorts.Where(port => port.Name == newPort.Name).Any())
        {
            return false;
        }
        OutputPorts.Add(newPort);
        return true;
    }
}

public enum InputPortType
{
    DataFlow = 0,
    Parameter = 1,
}

public class InputPort
{
    public string Name { get; set; } = null!;
    public InputPortType Type { get; set; } = InputPortType.DataFlow;
}

public class OutputPort
{
    public string Name { get; set; } = null!;
}

public class NodeLink
{
    public Guid SrcNodeId { get; set; }
    public string SrcPortName { get; set; } = null!;
    public Guid DestNodeId { get; set; }
    public string DestPortName { get; set; } = null!;
}


// TODO: Persistence DB -> Object instance: Serialization or Factory
// TODO: Object instance -> JSON or XML: Serialization
