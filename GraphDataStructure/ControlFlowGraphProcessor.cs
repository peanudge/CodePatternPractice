namespace GraphDataStructure;

public class ControlFlowGraphProcessor
{
    private readonly NodeGraph _graph;

    // Convert NodeGraph to ControlFlowGraph
    // private readonly ControlFlowGraph;

    public ControlFlowGraphProcessor(NodeGraph graph)
    {
        _graph = graph;
    }

    public (bool, string) ValidateCFGRules()
    {
        if (!ValidateNodePorts(_graph)) return (false, "Control Flow Graph should have one input and one output port in each node");
        if (!ValidateEntryNode(_graph)) return (false, "Control Flow Graph should have one entry node.");
        if (!ValidateExitNode(_graph)) return (false, "Control Flow Graph should have one exit node.");

        // Find all paths
        // Find cycle paths
        return (true, string.Empty);
    }

    private bool ValidateNodePorts(NodeGraph graph)
    {
        return graph.Nodes.All(n => n.InputPorts.Count <= 1 && n.OutputPorts.Count <= 1);
    }

    private bool ValidateEntryNode(NodeGraph graph)
    {
        return graph.Nodes.Count(n => n.Type == NodeType.Entry) == 1;
    }

    private bool ValidateExitNode(NodeGraph graph)
    {
        return graph.Nodes.Count(n => n.Type == NodeType.Exit) == 1;
    }
}
