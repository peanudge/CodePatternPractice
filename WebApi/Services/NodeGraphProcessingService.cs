using GraphDataStructure;

namespace WebApi.Services;

public class NodeGraphProcessingService : IDisposable
{
    // Only One Graph Processing at a time
    private NodeGraphProcessor? _currentNodeGraphProcessor = null;
    public NodeGraphProcessor? CurrentGraphProcessor => _currentNodeGraphProcessor;

    public CancellationTokenSource CancellationTokenSource { get; set; } = new();

    public NodeGraphProcessingService()
    {
    }

    public bool StartGraphProcessing(NodeGraph nodeGraph)
    {
        if (_currentNodeGraphProcessor is not null && !_currentNodeGraphProcessor.IsEnd)
        {
            // Already processing a other graph
            return false;
        }

        _currentNodeGraphProcessor = new NodeGraphProcessor(nodeGraph);

        Task.Run(() =>
        {
            _currentNodeGraphProcessor.Start(CancellationTokenSource.Token);
        }, CancellationTokenSource.Token);

        return true;
    }

    public void Dispose()
    {
        CancellationTokenSource.Cancel();
    }

    // TODO: Stop, Pause, Resume, Restart, Clear
}
