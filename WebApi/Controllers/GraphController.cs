using System.Net;
using System.Text.Json;
using GraphDataStructure;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
namespace WebApi;

[Route("api/graph")]
[ApiController]
public class GraphController : ControllerBase
{
    private readonly string _graphFilePath = Path.Combine(Environment.CurrentDirectory, "graph.json");
    private NodeGraphProcessingService _nodeGraphProcessingService;

    public GraphController(NodeGraphProcessingService nodeGraphProcessingService)
    {
        _nodeGraphProcessingService = nodeGraphProcessingService;
    }

    [HttpGet("example")]
    public IActionResult GetExampleGraph()
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

        return Ok(graph);
    }

    [HttpGet]
    public IActionResult GetGraph()
    {
        var encodedGraph = System.IO.File.ReadAllText(_graphFilePath);

        var decodedGraph = JsonSerializer.Deserialize<NodeGraph>(encodedGraph, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        return Ok(decodedGraph);
    }

    [HttpPost]
    public IActionResult SaveOrUpdateGraph(
        [FromBody] NodeGraph nodeGraph)
    {
        var encodedGraph = JsonSerializer.Serialize(nodeGraph, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        System.IO.File.WriteAllText(_graphFilePath, encodedGraph);

        return Ok(nodeGraph);
    }

    [HttpPut("process/start")]
    public IActionResult StartGraphProcessing(
        [FromQuery] int roundInterval = 300,
        [FromQuery] int nodeOperationDelay = 500)
    {
        var encodedGraph = System.IO.File.ReadAllText(_graphFilePath);

        var decodedGraph = JsonSerializer.Deserialize<NodeGraph>(encodedGraph, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        if (decodedGraph == null)
        {
            return Problem(
                 detail: "Saved Graph is not found.",
                 statusCode: StatusCodes.Status400BadRequest);
        }

        var isSuccess = _nodeGraphProcessingService.StartGraphProcessing(decodedGraph, new NodeGraphProcessorOptions()
        {
            RoundIntervalMs = roundInterval,
            NodeOperationDelayMs = nodeOperationDelay
        });
        if (!isSuccess)
        {
            return Problem(
                 detail: "Already processing a other graph.",
                 statusCode: StatusCodes.Status400BadRequest);
        }

        return Ok(decodedGraph);
    }

    [HttpGet("process")]
    public IActionResult GetGraphProcessorInfo()
    {
        var currentGraphProcessor = _nodeGraphProcessingService.CurrentGraphProcessor;

        return Ok(new
        {
            GraphProcessor = currentGraphProcessor
        });
    }
}
