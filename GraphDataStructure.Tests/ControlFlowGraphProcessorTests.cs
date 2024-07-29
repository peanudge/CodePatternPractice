using GraphDataStructure.CFG;

namespace GraphDataStructure.Tests;

public class ControlFlowGraphProcessorTests
{
    [Fact]
    public void ShouldHaveEntryNodeAndExitNodeInCFG()
    {
        var entryNode = new Node
        {
            Id = Guid.NewGuid(),
            Name = "A",
            Type = NodeType.Entry,
        };

        var exitNode = new Node
        {
            Id = Guid.NewGuid(),
            Name = "B",
            Type = NodeType.Exit,
        };

        var graph = new NodeGraph()
        {
            Nodes = new List<Node> { entryNode, exitNode },
        };

        Assert.True(ControlFlowGraphValidator.ValidateUniqueEntry(graph));
        Assert.True(ControlFlowGraphValidator.ValidateUniqueExit(graph));
    }

    [Fact]
    public void ShouldHaveOnlyOneDataFlowInputInCFG()
    {
        var mockNode = new Node
        {
            Id = Guid.NewGuid(),
            Name = "A",
            Type = NodeType.Action,
        };
        mockNode.AddInputPort(new InputPort() { Name = "Input1", Type = InputPortType.DataFlow });
        mockNode.AddInputPort(new InputPort() { Name = "Input2", Type = InputPortType.DataFlow });

        var graph = new NodeGraph()
        {
            Nodes = new List<Node> { mockNode },
        };

        Assert.False(ControlFlowGraphValidator.ValidateInputPortCount(graph));
    }

    [Fact]
    public void ShouldNotAllowOneMoreOutputInCFG()
    {
        var entryNode = new Node
        {
            Id = Guid.NewGuid(),
            Name = "A",
            Type = NodeType.Entry,
        };
        var exitNode = new Node
        {
            Id = Guid.NewGuid(),
            Name = "B",
            Type = NodeType.Exit,
        };
        var mockNode = new Node
        {
            Id = Guid.NewGuid(),
            Name = "A",
            Type = NodeType.Action,
        };
        mockNode.AddOutputPort(new OutputPort() { Name = "Output1" });
        mockNode.AddOutputPort(new OutputPort() { Name = "Output2" });

        var graph = new NodeGraph()
        {
            Nodes = new List<Node> { entryNode, mockNode, exitNode },
        };

        var isValid = ControlFlowGraphValidator.ValidateSequentialFlow(graph);
        Assert.False(isValid);
    }
}
