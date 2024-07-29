namespace GraphDataStructure.Tests;

public class ControlFlowGraphProcessorTests
{

    [Fact]
    public void ShouldHaveOneInputAndOneOutputInCFG()
    {
        var startNode = new Node
        {
            Id = Guid.NewGuid(),
            Name = "Start",
            Type = NodeType.Action,
        };
        startNode.AddInputPort(new InputPort() { Name = "Input" });
        startNode.AddOutputPort(new OutputPort() { Name = "Output" });

        var endNode = new Node
        {
            Id = Guid.NewGuid(),
            Name = "End",
            Type = NodeType.Action,
        };
        startNode.AddInputPort(new InputPort() { Name = "Input" });
        startNode.AddOutputPort(new OutputPort() { Name = "Output" });

        var graph = new NodeGraph()
        {
            Nodes = new List<Node> { startNode, endNode },
        };

        var processor = new ControlFlowGraphProcessor(graph);
        processor.ValidateCFGRules();
    }
}
