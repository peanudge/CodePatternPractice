using System.Collections.Concurrent;
using Microsoft.IdentityModel.Tokens;

namespace GraphDataStructure;

public record NodeOperationResult(bool IsSuccess);
public record NextNodeStartRequset(Guid NodeId);

public sealed class NodeGraphProcessor
{
    private readonly NodeGraph _graph;

    // TODO: Check if Data Sctructure is appropriate
    private readonly ConcurrentDictionary<Guid, Dictionary<string, NodeOperationResult?>> _outputPortResults = new();
    private readonly IDictionary<Guid, Task> _runningNodeOperations = new Dictionary<Guid, Task>();
    private readonly ConcurrentQueue<Guid> _completedNodeQueue = new();

    private readonly Queue<NextNodeStartRequset> _nextNodeQueue = new();

    private bool _isRunning = false;
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

    // TODO: Refactoring for readability
    // TODO: Implement Break points between each node
    public void StartAsync(CancellationToken cancellationToken = default)
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

        _nextNodeQueue.Enqueue(new NextNodeStartRequset(startNode.Id));

        while (!cancellationToken.IsCancellationRequested
            && !(_nextNodeQueue.IsNullOrEmpty() && _completedNodeQueue.IsEmpty && _runningNodeOperations.Count == 0))
        {
            // Ready next node id from completed nodes
            while (!_completedNodeQueue.IsEmpty)
            {
                _completedNodeQueue.TryDequeue(out var completedNodeId);

                // Remove completed node id in _runningNodeOperations
                _ = _runningNodeOperations.Remove(completedNodeId, out _);

                // Create NextNodeStartRequest from CompletedNode.
                var completedNode = _graph.Nodes.FirstOrDefault(node => node.Id == completedNodeId);
                if (completedNode is null)
                {
                    throw new Exception("Completed Node is not found.");
                }

                foreach (var outputPort in completedNode.OutputPorts)
                {
                    var links = _graph.FindConnectedLinksBySrcPort(completedNode.Id, outputPort.Name);
                    foreach (var link in links)
                    {
                        var nextNodeId = link.DestNodeId;
                        _nextNodeQueue.Enqueue(new NextNodeStartRequset(nextNodeId));
                    }
                }
            }

            var nextNodeIds = new HashSet<Guid>();
            while (!_nextNodeQueue.IsNullOrEmpty())
            {
                var nextNodeRequest = _nextNodeQueue.Dequeue();
                nextNodeIds.Add(nextNodeRequest.NodeId);
            }

            foreach (var nextNodeId in nextNodeIds)
            {
                var nextNode = _graph.Nodes.FirstOrDefault(node => node.Id == nextNodeId);

                // Check if all input ports are filled by previous node
                bool isReadyToStartNextNode = true;

                foreach (var inputPort in nextNode!.InputPorts)
                {
                    var links = _graph.FindConnectedLinksByDestPort(nextNodeId, inputPort.Name);

                    var link = links.First();
                    var prevNodeId = link.SrcNodeId;
                    var prevNodeOutputPortName = link.SrcPortName;

                    _outputPortResults.TryGetValue(prevNodeId, out var outputPortResults);

                    var matchedOutputPortResult = outputPortResults!.GetValueOrDefault(prevNodeOutputPortName);
                    if (matchedOutputPortResult is null)
                    {
                        isReadyToStartNextNode = false;
                        break;
                    }
                }

                if (!isReadyToStartNextNode)
                {
                    continue;
                }

                // Start the next node
                var nextNodeOperationTask = Task.Run(async () =>
                {
                    var currentNode = nextNode;

                    Console.WriteLine($"Start: {currentNode.Name}");
                    // TODO: Node Operation Logic.
                    await Task.Delay(100);
                    Console.WriteLine($"End: {currentNode.Name}");

                    // Fill the output results for next of next node
                    foreach (var outputPort in currentNode.OutputPorts)
                    {
                        _outputPortResults.TryGetValue(currentNode.Id, out var outputPortResults);
                        outputPortResults![outputPort.Name] = new NodeOperationResult(IsSuccess: true);
                    }

                    _completedNodeQueue.Enqueue(currentNode.Id);

                }, cancellationToken);

                _runningNodeOperations.TryAdd(nextNodeId, nextNodeOperationTask);
            }

            // INFO: Interval to move next node, Prevent excessive CPU tick
            Thread.Sleep(100);
        }

        _isRunning = false;
    }
}
