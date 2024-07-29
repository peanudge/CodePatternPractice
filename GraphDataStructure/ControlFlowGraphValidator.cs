namespace GraphDataStructure;

public static class ControlFlowGraphValidator
{
    public static bool ValidateInputPortCount(NodeGraph graph)
    {
        return graph.Nodes.All(node => node.InputPorts.Where(input => input.Type == InputPortType.DataFlow).Count() <= 1);
    }

    public static bool ValidateSequentialFlow(NodeGraph graph)
    {
        var parallelableNode = graph.Nodes.Find(n => n.Type != NodeType.Conditional && n.OutputPorts.Count() >= 2);
        return parallelableNode is null;
    }

    public static bool ValidateUniqueEntry(NodeGraph graph)
    {
        return graph.Nodes.Count(n => n.Type == NodeType.Entry) == 1;
    }

    public static bool ValidateUniqueExit(NodeGraph graph)
    {
        return graph.Nodes.Count(n => n.Type == NodeType.Exit) == 1;
    }
}
