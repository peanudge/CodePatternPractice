using System.Text.Json;
using GraphDataStructure;

var nodeA = new Node
{
    Id = Guid.NewGuid(),
    Name = "NodeA",
};
nodeA.AddOutputPort(new OutputPort { Name = "Output" });


var nodeB = new Node
{
    Id = Guid.NewGuid(),
    Name = "NodeB",
};
nodeB.AddInputPort(new InputPort { Name = "Input" });
nodeB.AddOutputPort(new OutputPort { Name = "Output" });

var nodeC = new Node
{
    Id = Guid.NewGuid(),
    Name = "NodeC",
};
nodeC.AddInputPort(new InputPort { Name = "Input" });
nodeC.AddOutputPort(new OutputPort { Name = "Output" });

var linkAB = new NodeLink
{
    SrcNodeId = nodeA.Id,
    SrcPortName = nodeA.OutputPorts[0].Name,
    DestNodeId = nodeB.Id,
    DestPortName = nodeB.InputPorts[0].Name
};

var linkBC = new NodeLink
{
    SrcNodeId = nodeB.Id,
    SrcPortName = nodeB.OutputPorts[0].Name,
    DestNodeId = nodeC.Id,
    DestPortName = nodeC.InputPorts[0].Name
};

var graph = new NodeGraph(
    new List<Node> { nodeA, nodeB, nodeC },
    new List<NodeLink> { linkAB, linkBC }
);

var nodeGraphProcessor = new NodeGraphProcessor(graph);

try
{
    nodeGraphProcessor.Start();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


var jsonSerializerOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    IncludeFields = false,
};

var encodedGraph = JsonSerializer.Serialize(graph, jsonSerializerOptions);

var graphFilePath = Path.Combine(Environment.CurrentDirectory, "graph.json");

File.WriteAllText(graphFilePath, encodedGraph);


var text = File.ReadAllText(graphFilePath);

var decodedGraph = JsonSerializer.Deserialize<NodeGraph>(text, jsonSerializerOptions);
if (decodedGraph is null)
{
    Console.WriteLine("Failed to Deserialize Graph");
    return;
}
Console.WriteLine($"Save Graph {decodedGraph.Nodes.Count} Nodes, {decodedGraph.Links.Count} Links");

// TODO: Deserialized Graph & Build Linking NodePorts and NodeLinks

