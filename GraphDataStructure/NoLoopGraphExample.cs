namespace GraphDataStructure;

public static class NoLoopGraphExample
{
    public static NodeGraph CreateGraph()
    {
        var nodeA = new Node
        {
            Id = Guid.NewGuid(),
            Name = "NodeA",
        };
        nodeA.AddOutputPort(new OutputPort { Name = "Output" });

        var nodeB = new Node
        {
            Id = Guid.NewGuid(),
            Name = "NodeB",
        };
        nodeB.AddInputPort(new InputPort { Name = "Input" });
        nodeB.AddOutputPort(new OutputPort { Name = "Output" });

        var nodeC = new Node
        {
            Id = Guid.NewGuid(),
            Name = "NodeC",
        };
        nodeC.AddInputPort(new InputPort { Name = "Input" });
        nodeC.AddOutputPort(new OutputPort { Name = "Output" });


        var nodeD = new Node
        {
            Id = Guid.NewGuid(),
            Name = "NodeD",
        };
        nodeD.AddInputPort(new InputPort { Name = "Input" });
        nodeD.AddOutputPort(new OutputPort { Name = "Output1" });
        nodeD.AddOutputPort(new OutputPort { Name = "Output2" });
        nodeD.AddOutputPort(new OutputPort { Name = "Output3" });

        var nodeE = new Node
        {
            Id = Guid.NewGuid(),
            Name = "NodeE",
        };
        nodeE.AddInputPort(new InputPort { Name = "Input" });
        nodeE.AddOutputPort(new OutputPort { Name = "Output" });

        var nodeF = new Node
        {
            Id = Guid.NewGuid(),
            Name = "NodeF",
        };
        nodeF.AddInputPort(new InputPort { Name = "Input" });
        nodeF.AddOutputPort(new OutputPort { Name = "Output" });

        var nodeG = new Node
        {
            Id = Guid.NewGuid(),
            Name = "nodeG",
        };
        nodeG.AddInputPort(new InputPort { Name = "Input" });
        nodeG.AddOutputPort(new OutputPort { Name = "Output" });

        var linkAB = new NodeLink
        {
            SrcNodeId = nodeA.Id,
            SrcPortName = nodeA.OutputPorts[0].Name,
            DestNodeId = nodeB.Id,
            DestPortName = nodeB.InputPorts[0].Name
        };

        var linkBC = new NodeLink
        {
            SrcNodeId = nodeB.Id,
            SrcPortName = nodeB.OutputPorts[0].Name,
            DestNodeId = nodeC.Id,
            DestPortName = nodeC.InputPorts[0].Name
        };

        var linkCD = new NodeLink
        {
            SrcNodeId = nodeC.Id,
            SrcPortName = nodeC.OutputPorts[0].Name,
            DestNodeId = nodeD.Id,
            DestPortName = nodeD.InputPorts[0].Name
        };

        var linkDE = new NodeLink
        {
            SrcNodeId = nodeD.Id,
            SrcPortName = nodeD.OutputPorts[0].Name,
            DestNodeId = nodeE.Id,
            DestPortName = nodeE.InputPorts[0].Name
        };

        var linkDF = new NodeLink
        {
            SrcNodeId = nodeD.Id,
            SrcPortName = nodeD.OutputPorts[1].Name,
            DestNodeId = nodeF.Id,
            DestPortName = nodeF.InputPorts[0].Name
        };

        var linkDG = new NodeLink
        {
            SrcNodeId = nodeD.Id,
            SrcPortName = nodeD.OutputPorts[2].Name,
            DestNodeId = nodeG.Id,
            DestPortName = nodeG.InputPorts[0].Name
        };


        return new NodeGraph(
            new List<Node> { nodeA, nodeB, nodeC, nodeD, nodeE, nodeF, nodeG },
            new List<NodeLink> { linkAB, linkBC, linkCD, linkDE, linkDF, linkDG }
        );
    }
}
