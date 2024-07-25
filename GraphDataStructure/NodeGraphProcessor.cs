using System.Collections.Concurrent;
using Microsoft.IdentityModel.Tokens;

namespace GraphDataStructure;

public record NodeOperationResult(bool IsSuccess, string? ErrorMessage = null);
public record NextNodeStartEvent(Guid NodeId, bool byLoop = false);
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
        Initialize();

        RoundInterval = options?.RoundIntervalMs ?? 500;
        NodeOperationDelay = options?.NodeOperationDelayMs ?? 300;
    }

    public void Initialize()
    {
        InitializeOutputResults();

        _nodeCompleteEventQueue.Clear();
        _nextNodeStartEventQueue.Clear();
        _currentRunningNodes.Select(task => task);
    }

    /// <summary>
    /// Initialize OutputResults as HashTable
    /// </summary>
    private void InitializeOutputResults()
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
        Initialize();

        _isRunning = true;

        // Find & Start the first node.
        var startNodes = _graph.Nodes
           .Where(n => n.InputPorts.Where(p => p.IsParameter == false).Count() == 0)
           .ToList();

        if (startNodes.Count == 0)
        {
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
                    // TODO: Check if next node is triggered by loopback link
                    // new NextNodeStartEvent(nextNodeId, byLoop: true)
                    _nextNodeStartEventQueue.Enqueue(new NextNodeStartEvent(nextNodeId));
                });
            }

            var nextNodeIds = new HashSet<Guid>();
            while (!_nextNodeStartEventQueue.IsNullOrEmpty())
            {
                var nextNodeStartEvent = _nextNodeStartEventQueue.Dequeue();
                nextNodeIds.Add(nextNodeStartEvent.NodeId);
            }

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

                // TODO: If started by loopback link,
                // Cleanup the output results of nodes in cycle triggered by this node

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
                        foreach (var outputPort in currentNode.OutputPorts)
                        {
                            _outputPortResults.TryGetValue(currentNode.Id, out var outputPortResult);

                            // You should implement different output port filling logic.

                            outputPortResult![outputPort.Name] = new NodeOperationResult(IsSuccess: isSuccess, errorMessage);
                        }

                        _nodeCompleteEventQueue.Enqueue(new NodeCompleteEvent(currentNode.Id));
                    }
                }, cancellationToken);


                // TODO: Create Cancelllation Token for stopping nodeOperationTask.
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
        Console.WriteLine($"{node.Name} visited.");
        // TODO: Node Operation Logic.
        await Task.Delay(NodeOperationDelay);
    }

    private bool IsReadyToStart(Node node)
    {
        // Check if all input ports are filled by previous node
        return node!.InputPorts
            .All(inputPort => IsReadyPort(node, inputPort));
    }

    private bool IsReadyPort(Node node, InputPort inputPort)
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

        // TODO: Check whether all inputs is ready, except inputs from loopback link
        // Except loopback link, All link sholud have data.


        var isReadyAllIncomingLinks = incomingLinks.All(
            link => HasDataInPort(link.SrcNodeId, link.SrcPortName)
        );

        return isReadyAllIncomingLinks;
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
