using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace GraphDataStructure;

public record NodeOperationResult(bool IsSuccess, string? ErrorMessage = null);
public record NextNodeStartEvent(Guid NodeId);
public record NodeCompleteEvent(Guid NodeId);


public class NodeGraphProcessorOptions
{
    public int RoundIntervalMs { get; set; }
    public int NodeOperationDelayMs { get; set; }
}

public sealed class NodeGraphProcessor
{
    private readonly NodeGraph _graph;
    private readonly Dictionary<Guid, Dictionary<string, NodeOperationResult?>> _outputPortResults = new();
    private readonly Dictionary<Guid, Task> _currentRunningNodes = new();
    private readonly ConcurrentQueue<NodeCompleteEvent> _nodeCompleteEventQueue = new();
    private readonly Queue<NextNodeStartEvent> _nextNodeStartEventQueue = new();
    private bool _isRunning = false;


    public List<Guid> CurrentRunningNodeIds => _currentRunningNodes.Keys.ToList();
    public bool IsRunning => _isRunning;
    public bool IsEnd => !_isRunning;


    private int RoundInterval { get; set; }
    private int NodeOperationDelay { get; set; }

    public NodeGraphProcessor(NodeGraph graph, NodeGraphProcessorOptions? options = null)
    {
        _graph = graph;
        InitializeOutputResults();

        RoundInterval = options?.RoundIntervalMs ?? 500;
        NodeOperationDelay = options?.NodeOperationDelayMs ?? 300;
    }

    /// <summary>
    /// Initialize OutputResults as HashTable
    /// </summary>
    public void InitializeOutputResults()
    {
        foreach (var node in _graph.Nodes)
        {
            var outputPortResultDict = new Dictionary<string, NodeOperationResult?>();
            foreach (var outputPort in node.OutputPorts)
            {
                outputPortResultDict.Add(outputPort.Name, null);
            }
            _outputPortResults.TryAdd(node.Id, outputPortResultDict);
        }
    }

    // TODO: Implement Break points between each steps
    public void Start(CancellationToken cancellationToken = default)
    {
        InitializeOutputResults();

        _isRunning = true;

        // Find & Start the first node.
        var startNodes = _graph.Nodes
           .Where(n => n.InputPorts.Where(p => p.IsParameter == false).Count() == 0)
           .ToList();

        if (startNodes.Count == 0)
        {
            Console.WriteLine("Start Node is not found.");
            _isRunning = false;
            return;
        }

        startNodes.ForEach(startNode =>
        {
            _nextNodeStartEventQueue.Enqueue(new NextNodeStartEvent(startNode.Id));
        });

        while (!cancellationToken.IsCancellationRequested
            && !(_nextNodeStartEventQueue.IsNullOrEmpty() && _currentRunningNodes.Count == 0))
        {
            // Ready next node id from completed nodes
            while (!_nodeCompleteEventQueue.IsEmpty)
            {
                _nodeCompleteEventQueue.TryDequeue(out var nodeCompleteEvent);

                var completedNodeId = nodeCompleteEvent!.NodeId;

                _currentRunningNodes.Remove(completedNodeId);

                var completedNode = _graph.Nodes.Find(node => node.Id == completedNodeId);
                if (completedNode is null)
                {
                    throw new Exception($"Completed Node(Id: {completedNodeId}) is not found.");
                }

                FindNextNodes(completedNode).ForEach(nextNodeId =>
                {
                    _nextNodeStartEventQueue.Enqueue(new NextNodeStartEvent(nextNodeId));
                });
            }

            // TODO: Break Point before start next round

            var nextNodeIds = new HashSet<Guid>();
            while (!_nextNodeStartEventQueue.IsNullOrEmpty())
            {
                var nextNodeStartEvent = _nextNodeStartEventQueue.Dequeue();
                nextNodeIds.Add(nextNodeStartEvent.NodeId);
            }

            // TODO: Show Waiting Node to User for better understanding situation

            foreach (var nextNodeId in nextNodeIds)
            {
                var nextNode = _graph.Nodes.FirstOrDefault(node => node.Id == nextNodeId);
                if (nextNode is null)
                {
                    throw new Exception($"Next Node(Id: {nextNodeId}) is not found.");
                }

                if (!IsReadyToStart(nextNode))
                {
                    continue;
                }

                // INFO: If the node is ready to start, Consume Previsous NodeOperationResult.
                ConsumeInputsFor(nextNode);

                // Start the next node
                var nodeOperationTask = Task.Run(async () =>
                {
                    var currentNode = nextNode;
                    var isSuccess = false;
                    var errorMessage = string.Empty;
                    try
                    {
                        await RunNodeAsync(currentNode);
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        errorMessage = ex.Message;
                    }
                    finally
                    {
                        // Fill the output results for finding next nodes
                        foreach (var outputPort in currentNode.OutputPorts)
                        {
                            _outputPortResults.TryGetValue(currentNode.Id, out var outputPortResult);

                            // TODO: IF Node is Primitive, Arithmetic, Logical Node,
                            // You should implement Different output port filling logic.
                            outputPortResult![outputPort.Name] = new NodeOperationResult(IsSuccess: isSuccess, errorMessage);
                        }

                        // Send Signal to move next node
                        _nodeCompleteEventQueue.Enqueue(new NodeCompleteEvent(currentNode.Id));
                    }
                }, cancellationToken);

                _currentRunningNodes.TryAdd(nextNodeId, nodeOperationTask);
            }

            // INFO: Interval to move next node, Prevent excessive CPU tick
            Thread.Sleep(RoundInterval);
        }

        _isRunning = false;
    }


    // TODO: Implement Node Operation Logic
    private async Task RunNodeAsync(Node node)
    {
        Console.WriteLine($"{node.Name} [Start]");
        // TODO: Node Operation Logic.
        await Task.Delay(NodeOperationDelay);
        Console.WriteLine($"{node.Name} [End]");
    }

    private bool IsReadyToStart(Node node)
    {
        // Check if all input ports are filled by previous node
        return node!.InputPorts
            .All(inputPort => IsReadyDataIn(node, inputPort));
    }

    private bool IsReadyDataIn(Node node, InputPort inputPort)
    {
        if (inputPort.IsParameter)
        {
            return true;
        }

        var incomingLinks = _graph.FindIncomingLinksTo(node.Id, inputPort.Name);
        if (incomingLinks.Count == 0)
        {
            // INFO: Not Allow Input Port is not connected.
            return false;
        }

        // If incoming links include loopback link, there are more than one incoming link.
        var hasReadyAnyIncomingLink = incomingLinks.Any(
            link => HasDataInPort(link.SrcNodeId, link.SrcPortName)
        );

        return hasReadyAnyIncomingLink;
    }

    private bool HasDataInPort(Guid nodeId, string portName)
    {
        var outputPortResults = _outputPortResults.GetValueOrDefault(nodeId);
        if (outputPortResults is null)
        {
            throw new Exception($"Port Results for Node(Id: {nodeId}) is not found.");
        }

        var outputPortResult = outputPortResults!.GetValueOrDefault(portName);
        return outputPortResult is not null;
    }

    private Dictionary<string, NodeOperationResult> ConsumeInputsFor(Node node)
    {
        var inputs = new Dictionary<string, NodeOperationResult>();

        // Check if all input ports are filled by previous node
        // TODO: Pop data in srouce of incomming links
        return inputs;
    }


    private List<Guid> FindNextNodes(Node currentNode)
    {
        var nextNodeIds = new List<Guid>();
        foreach (var outputPort in currentNode.OutputPorts)
        {
            var outgoingLinks = _graph.FindOutgoingLinksFrom(
                srcNodeId: currentNode.Id,
                srcPortName: outputPort.Name
            );

            var hasLinkToSelf = outgoingLinks
                .Where(link =>
                    link.SrcNodeId == link.DestNodeId &&
                    link.SrcPortName == link.DestPortName)
                .Any();

            if (hasLinkToSelf)
            {
                throw new Exception($"Self Cycle Link is detected. (NodeId: {currentNode.Id}, PortName: {outputPort.Name})");
            }

            outgoingLinks
                .ForEach(link => nextNodeIds.Add(link.DestNodeId));
        }
        return nextNodeIds;
    }
}
