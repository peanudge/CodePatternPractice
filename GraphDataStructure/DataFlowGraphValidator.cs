namespace GraphDataStructure;

public static class DataFlowGraphValidator
{
    public static bool ValidateAcyclicGraph(NodeGraph graph)
    {
        // TODO: Find all path
        // TODO: Find cycle
        return true;
    }

    public static bool ValidateIncomingLinks(NodeGraph graph)
    {
        // TODO: Find Input Port with multiple incoming links

        return true;
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
