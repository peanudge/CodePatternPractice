namespace GraphDataStructure;

public class ActionGraph
{
    public Dictionary<Guid, ActionGraphNode> Nodes { get; set; } = new();
    public Dictionary<Guid, List<Guid>> AdjacancyList { get; set; } = new();
    public List<ActionGraphEdge> Edges => AdjacancyList
        .SelectMany(
            kvp => kvp.Value
                .Select(v => new ActionGraphEdge(Nodes[kvp.Key], Nodes[v]))
        ).ToList();

    public void AddNode(ActionGraphNode node)
    {
        Nodes.Add(node.Id, node);
    }

    public void Link(Guid from, Guid to)
    {
        var newEdge = new ActionGraphEdge(Nodes[from], Nodes[to]);

        var prevAdjacencyList = AdjacancyList.GetValueOrDefault(from, new List<Guid>());
        AdjacancyList[from] = prevAdjacencyList;

        if (prevAdjacencyList.Any(id => id == to))
        {
            // Already linked
            return;
        }
        else
        {
            prevAdjacencyList.Add(to);
        }
    }
}

public class ActionGraphNode
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class ActionGraphEdge
{
    public ActionGraphNode From { get; set; }
    public ActionGraphNode To { get; set; }

    public ActionGraphEdge(ActionGraphNode from, ActionGraphNode to)
    {
        From = from;
        To = to;
    }
}
