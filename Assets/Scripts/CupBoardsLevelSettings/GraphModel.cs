using System.Collections.Generic;

namespace CupBoardsLevelSettings
{
    public class GraphModel
    {
        public List<NodeModel> Nodes { get; }

        public GraphModel(List<NodeModel> nodes)
        {
            Nodes = nodes;
        }
    }
}