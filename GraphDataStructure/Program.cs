using System.Text.Json;
using GraphDataStructure;

var graph = new ActionGraph();
var node1 = new ActionGraphNode { Id = Guid.NewGuid(), Name = "Node 1" };
var node2 = new ActionGraphNode { Id = Guid.NewGuid(), Name = "Node 2" };
var node3 = new ActionGraphNode { Id = Guid.NewGuid(), Name = "Node 3" };

graph.AddNode(node1);
graph.AddNode(node2);
graph.AddNode(node3);

graph.Link(node1.Id, node2.Id);
graph.Link(node2.Id, node3.Id);


var stringifyGraph = JsonSerializer.Serialize(
    graph,
    new JsonSerializerOptions { WriteIndented = true });

Console.WriteLine(stringifyGraph);
