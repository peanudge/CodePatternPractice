using System.Text.Json;
using GraphDataStructure;
using GraphDataStructure.NodeGraphProcessor;

var graphFilePath = Path.Combine(Environment.CurrentDirectory, "graph.json");

var options = new NodeGraphProcessorOptions()
{
    RoundIntervalMs = 1000,
    NodeOperationDelayMs = 0,
};

var jsonSerializerOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    IncludeFields = false,
};

var text = File.ReadAllText(graphFilePath);

var decodedGraph = JsonSerializer.Deserialize<NodeGraph>(text, jsonSerializerOptions);
if (decodedGraph is null)
{
    Console.WriteLine("Failed to Deserialize Graph");
    return;
}
Console.WriteLine($"Save Graph {decodedGraph.Nodes.Count} Nodes, {decodedGraph.Links.Count} Links");

// TODO: Deserialized Graph & Build Linking NodePorts and NodeLinks

// var graph = decodedGraph;
var graph = NoLoopGraphExample.CreateGraph();

var encodedGraph = JsonSerializer.Serialize(graph, jsonSerializerOptions);
File.WriteAllText(graphFilePath, encodedGraph);

// var nodeGraphProcessor = new NodeGraphProcessor(graph, options);

// try
// {
//     nodeGraphProcessor.Start();
// }
// catch (Exception ex)
// {
//     Console.WriteLine(ex.Message);
// }
