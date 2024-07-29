using System.Collections.Concurrent;

namespace GraphDataStructure.CFG;

public record NodeOperationResult(bool IsSuccess, string? ErrorMessage = null);
public record NextNodeStartEvent(Guid NodeId, bool byLoop = false);
public record NodeCompleteEvent(Guid NodeId);

public class ControlFlowGraphProcessor
{
    private readonly NodeGraph _graph;

    // Convert NodeGraph to ControlFlowGraph
    // private readonly ControlFlowGraph;

    /// <summary>
    /// Key: NodeId, Value: Dictionary of OutputPortName and NodeExecutionResult
    /// </summary>
    private readonly Dictionary<Guid, Dictionary<string, NodeOperationResult?>> _results = new();

    public ControlFlowGraphProcessor(NodeGraph graph)
    {
        _graph = graph;
        Initialize();
    }

    private void Initialize()
    {
        foreach (var node in _graph.Nodes)
        {
            var outputPortResultDict = new Dictionary<string, NodeOperationResult?>();
            foreach (var outputPort in node.OutputPorts)
            {
                outputPortResultDict.Add(outputPort.Name, null);
            }
            _results.TryAdd(node.Id, outputPortResultDict);
        }
    }

    public void Start()
    {
        Initialize();

        var validationResult = ValidateCFGRules();
        if (!validationResult.IsValid)
        {
            return;
        }

        var entryNode = _graph.Nodes.Find(n => n.Type == NodeType.Entry);
        if (entryNode == null) return;

        // Start from Entry Node
        // While (<Has Running Noe> && <Next Node Event>)
        // 1. Get Next Node
        // 2. Run Node (async Task, when finish, will send NodeCompleteEvent)
        // 3. Wait NodeCompleteEvent
        // 4. If Next Node is Exit Node, then break;
    }

    public (bool IsValid, string ErrorMessage) ValidateCFGRules()
    {
        if (!ControlFlowGraphValidator.ValidateInputPortCount(_graph)) return (false, "Control Flow Graph should have one input port in each node");
        if (!ControlFlowGraphValidator.ValidateSequentialFlow(_graph)) return (false, "Control Flow Graph should be sequential.");
        if (!ControlFlowGraphValidator.ValidateUniqueEntry(_graph)) return (false, "Control Flow Graph should have one entry node.");
        if (!ControlFlowGraphValidator.ValidateUniqueExit(_graph)) return (false, "Control Flow Graph should have one exit node.");

        return (true, string.Empty);
    }

}
