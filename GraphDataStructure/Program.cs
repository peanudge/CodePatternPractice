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


var nodeD = new Node
{
    Id = Guid.NewGuid(),
    Name = "NodeD",
};
nodeD.AddInputPort(new InputPort { Name = "Input" });
nodeD.AddOutputPort(new OutputPort { Name = "Output" });

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

var linkCD = new NodeLink
{
    SrcNodeId = nodeC.Id,
    SrcPortName = nodeC.OutputPorts[0].Name,
    DestNodeId = nodeD.Id,
    DestPortName = nodeD.InputPorts[0].Name
};

var linkDB_Loopback = new NodeLink
{
    SrcNodeId = nodeD.Id,
    SrcPortName = nodeD.OutputPorts[0].Name,
    DestNodeId = nodeB.Id,
    DestPortName = nodeB.InputPorts[0].Name
};

var graph1 = new NodeGraph(
    new List<Node> { nodeA, nodeB, nodeC, nodeD },
    new List<NodeLink> { linkAB, linkBC, linkCD, linkDB_Loopback }
);

// Merge Graph
// var relayNode1 = new RelayNode("RelayNode1");
// relayNode1.StartPort.IsParameter = true;
// var relayNode2 = new RelayNode("RelayNode2");
// var relayNodelink = new NodeLink()
// {
//     SrcNodeId = relayNode1.Id,
//     SrcPortName = relayNode1.EndPort.Name,
//     DestNodeId = relayNode2.Id,
//     DestPortName = relayNode2.StartPort.Name
// };
// var graph2 = new NodeGraph(
//     new List<Node> { relayNode1, relayNode2 },
//     new List<NodeLink> { relayNodelink }
// );

// var mergedGraph = graph1.Merge(graph2);
var mergedGraph = graph1;

var options = new NodeGraphProcessorOptions()
{
    RoundIntervalMs = 1000,
    NodeOperationDelayMs = 0,
};

var nodeGraphProcessor = new NodeGraphProcessor(mergedGraph, options);

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

var encodedGraph = JsonSerializer.Serialize(mergedGraph, jsonSerializerOptions);

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

