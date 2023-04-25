using System.Collections.Generic;
using CupBoardsLevelSettings;

public class GraphModel
{
    public List<NodeModel> Nodes { get; }

    public GraphModel(List<NodeModel> nodes)
    {
        Nodes = nodes;
    }
}