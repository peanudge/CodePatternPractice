using System.Text.Json;
using GraphDataStructure;
using Microsoft.AspNetCore.Mvc;
namespace WebApi;

[Route("api/graph")]
[ApiController]
public class GraphController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {

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

        // var encodedGraph = JsonSerializer.Serialize(graph, new JsonSerializerOptions
        // {
        //     WriteIndented = true,
        //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        // });

        return Ok(graph);
    }
}