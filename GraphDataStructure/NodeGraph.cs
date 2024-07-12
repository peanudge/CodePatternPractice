using System.Text.Json.Serialization;

namespace GraphDataStructure;

// Factory NodeGraph
// 1. Create Nodes
// 2. Create Edges
// 3. Link Node's Ports with Edges

public class NodeGraph
{
    public List<Node> Nodes { get; set; } = new();
    public List<NodeLink> Links { get; set; } = new();

    public NodeGraph(IList<Node> nodes, IList<NodeLink> links)
    {
        Nodes.AddRange(nodes);
        Links.AddRange(links);
    }

    public void Build()
    {
        var nodeDict = Nodes.ToDictionary(node => node.Id);

        // Link Node's Ports with Edges
        foreach (var link in Links)
        {
            var targetOutputPort = nodeDict[link.SrcNodeId].OutputPorts
                .Where(port => port.Name == link.SrcPortName)
                .FirstOrDefault();

            if (targetOutputPort is not null) { targetOutputPort.Link = link; }


            var targetInputPort = nodeDict[link.DestNodeId].InputPorts
                .Where(port => port.Name == link.DestPortName)
                .FirstOrDefault();

            if (targetInputPort is not null) targetInputPort.Link = link;
        }
    }
}

public class Node
{
    public Guid Id { get; set; }
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

public class InputPort
{
    public string Name { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NodeLink? Link { get; set; } = null;
}

public class OutputPort
{
    public string Name { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NodeLink? Link { get; set; } = null;
}

public class NodeLink
{
    public Guid SrcNodeId { get; set; }
    public string SrcPortName { get; set; } = null!;
    public Guid DestNodeId { get; set; }
    public string DestPortName { get; set; } = null!;
}


// TODO: Design Encoding for graph
// TODO: Persistence DB -> Object instance: Serialization or Factory
// TODO: Object instance -> JSON or XML: Serialization
