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

    [JsonConstructor]
    public NodeGraph()
    {
    }

    public NodeGraph(IList<Node> nodes, IList<NodeLink> links)
    {
        Nodes.AddRange(nodes);
        Links.AddRange(links);
    }

    public List<NodeLink> FindConnectedLinksBySrcPort(Guid srcNodeId, string srcPortName)
    {
        return Links.Where(link => link.SrcNodeId == srcNodeId && link.SrcPortName == srcPortName)
            .ToList();
    }

    public List<NodeLink> FindConnectedLinksByDestPort(Guid destNodeId, string destPortName)
    {
        return Links.Where(link => link.DestNodeId == destNodeId && link.DestPortName == destPortName)
            .ToList();
    }

    public NodeGraph Merge(NodeGraph nodeGraph)
    {
        return new NodeGraph(
            Nodes.Concat(nodeGraph.Nodes).ToList(),
            Links.Concat(nodeGraph.Links).ToList()
        );
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

    public bool IsParameter { get; set; } = false;
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
