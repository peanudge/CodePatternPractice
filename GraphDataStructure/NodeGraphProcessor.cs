using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace GraphDataStructure;

public record NodeOperationResult(bool IsSuccess);
public record NextNodeStartRequset(Guid NodeId);

public sealed class NodeGraphProcessor
{
    private readonly NodeGraph _graph;

    // TODO: Check if Data Sctructure is appropriate
    private readonly ConcurrentDictionary<Guid, Dictionary<string, NodeOperationResult?>> _outputPortResults = new();
    private readonly IDictionary<Guid, Task> _runningNodeOperations = new Dictionary<Guid, Task>();
    // private readonly Queue<Guid> _completedNodeOperations = new();
    private readonly ConcurrentQueue<NextNodeStartRequset> _nextNodeQueue = new();

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
    public async Task StartAsync(CancellationToken cancellationToken = default)
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

        while (!cancellationToken.IsCancellationRequested && !(_nextNodeQueue.IsEmpty && _runningNodeOperations.Count == 0))
        {
            Console.WriteLine($"Current:{_runningNodeOperations.Count}, Ready: {_nextNodeQueue.Count}");

            // Batch handling accumulated NextNodeQueue
            var readyNextNodeIds = new HashSet<Guid>();
            while (!_nextNodeQueue.IsEmpty)
            {
                var success = _nextNodeQueue.TryDequeue(out var nextNodeRequest);
                Debug.Assert(success);
                readyNextNodeIds.Add(nextNodeRequest!.NodeId);
            }

            // INFO: Consume all readyNextNodeIds
            foreach (var nextNodeId in readyNextNodeIds)
            {
                var nextNode = _graph.Nodes.FirstOrDefault(node => node.Id == nextNodeId);
                Debug.Assert(nextNode != null);

                // Check if all input ports are filled by previous node
                bool isReadyToStartNextNode = true;
                foreach (var inputPort in nextNode.InputPorts)
                {
                    var links = _graph.FindConnectedLinksByDestPort(nextNodeId, inputPort.Name);
                    Debug.Assert(links.Count == 1, "Output Port is connected to only one Input Port");
                    var link = links.First();
                    var prevNodeId = link.SrcNodeId;
                    var prevNodeOutputPortName = link.SrcPortName;

                    var gotOutputPortResult = _outputPortResults.TryGetValue(prevNodeId, out var outputPortResults);
                    Debug.Assert(gotOutputPortResult);

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

                    // Console.WriteLine($"Start: {currentNode.Name}");
                    // TODO: Node Operation Logic.
                    await Task.Delay(100);
                    // Console.WriteLine($"End: {currentNode.Name}");

                    // Fill the output results for next of next node
                    foreach (var outputPort in currentNode.OutputPorts)
                    {
                        var gotOutputPortResult = _outputPortResults.TryGetValue(currentNode.Id, out var outputPortResults);
                        Debug.Assert(gotOutputPortResult);

                        // Fulfill the output results
                        outputPortResults![outputPort.Name] = new NodeOperationResult(IsSuccess: true);

                        // Find the next node
                        var links = _graph.FindConnectedLinksBySrcPort(currentNode.Id, outputPort.Name);

                        foreach (var link in links)
                        {
                            var nextNodeId = link.DestNodeId;
                            // INFO: Request to start the next node of this node
                            _nextNodeQueue.Enqueue(new NextNodeStartRequset(nextNodeId));
                        }
                    }

                    // INFO: remove self from runningNodeOperations for fast reponsiveness
                    _ = _runningNodeOperations.Remove(nextNodeId, out _);
                }, cancellationToken);

                _runningNodeOperations.TryAdd(nextNodeId, nextNodeOperationTask);
            }

            // INFO: Interval to move next node, Prevent excessive CPU tick
            await Task.Delay(100, cancellationToken);

            // TODO: Implement break points.
        }

        Debug.Assert(_nextNodeQueue.Count == 0, "Next Node Queue is not empty");
        Debug.Assert(_runningNodeOperations.Count == 0, "Running Node Queue is not empty");
        _isRunning = false;
    }
}
