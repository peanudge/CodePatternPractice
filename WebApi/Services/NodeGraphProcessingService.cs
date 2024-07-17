using GraphDataStructure;

namespace WebApi.Services;

public class NodeGraphProcessingService : IDisposable
{
    // Only One Graph Processing at a time
    public NodeGraphProcessor? CurrentGraphProcessor { get; set; }

    public CancellationTokenSource CancellationTokenSource { get; set; } = new();

    public NodeGraphProcessingService()
    {
    }

    public bool StartGraphProcessing(NodeGraph nodeGraph, NodeGraphProcessorOptions? options = null)
    {
        if (CurrentGraphProcessor is not null && !CurrentGraphProcessor.IsEnd)
        {
            // Already processing a other graph
            return false;
        }

        CurrentGraphProcessor = new NodeGraphProcessor(nodeGraph, options);

        Task.Run(() =>
        {
            CurrentGraphProcessor.Start(CancellationTokenSource.Token);
        }, CancellationTokenSource.Token);

        return true;
    }

    public void Dispose()
    {
        CancellationTokenSource.Cancel();
    }

    // TODO: Stop, Pause, Resume, Restart, Clear
}
