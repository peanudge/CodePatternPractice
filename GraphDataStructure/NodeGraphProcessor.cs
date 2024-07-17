using System.Collections.Concurrent;
using Microsoft.IdentityModel.Tokens;

namespace GraphDataStructure;

public record NodeOperationResult(bool IsSuccess, string? ErrorMessage = null);
public record NextNodeStartEvent(Guid NodeId);
public record NodeCompleteEvent(Guid NodeId);

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

    public NodeGraphProcessor(NodeGraph graph)
    {
        _graph = graph;
        InitializeOutputResults();
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
        var startNode = _graph.Nodes.FirstOrDefault(n => n.InputPorts.Count == 0);
        if (startNode == null)
        {
            Console.WriteLine("Start Node is not found.");
            _isRunning = false;
            return;
        }

        _nextNodeStartEventQueue.Enqueue(new NextNodeStartEvent(startNode.Id));

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
            Thread.Sleep(500);
        }

        _isRunning = false;
    }


    // TODO: Implement Node Operation Logic
    private async Task RunNodeAsync(Node node)
    {
        Console.WriteLine($"{node.Name} [Start]");
        // TODO: Node Operation Logic.
        await Task.Delay(300);
        Console.WriteLine($"{node.Name} [End]");
    }

    private bool IsReadyToStart(Node node)
    {
        var isReady = true;

        // Check if all input ports are filled by previous node
        foreach (var inputPort in node!.InputPorts)
        {
            var connectedLinks = _graph.FindConnectedLinksByDestPort(node.Id, inputPort.Name);
            var link = connectedLinks.FirstOrDefault();
            if (link is null)
            {
                // INFO: Not Allow Input Port is not connected.
                throw new Exception($"Link connected with port (NodeId: {node.Id}, PortName: {inputPort.Name}) is not found.");
            }

            var prevNodeId = link.SrcNodeId;
            var prevNodeOutputPortName = link.SrcPortName;

            _outputPortResults.TryGetValue(prevNodeId, out var outputPortResults);

            var matchedOutputPortResult = outputPortResults!.GetValueOrDefault(prevNodeOutputPortName);
            if (matchedOutputPortResult is null)
            {
                isReady = false;
                break;
            }
        }

        return isReady;
    }


    private List<Guid> FindNextNodes(Node currentNode)
    {
        var nextNodeIds = new List<Guid>();
        foreach (var outputPort in currentNode.OutputPorts)
        {
            var connectedLinks = _graph.FindConnectedLinksBySrcPort(
                srcNodeId: currentNode.Id,
                srcPortName: outputPort.Name
            );

            var hasCycleLink = connectedLinks
                .Where(link =>
                    link.SrcNodeId == link.DestNodeId &&
                    link.SrcPortName == link.DestPortName)
                .Any();

            if (hasCycleLink)
            {
                throw new Exception($"Cycle Link is detected. (NodeId: {currentNode.Id}, PortName: {outputPort.Name})");
            }

            foreach (var link in connectedLinks)
            {
                var nextNodeId = link.DestNodeId;
                nextNodeIds.Add(nextNodeId);
            }
        }
        return nextNodeIds;
    }
}
