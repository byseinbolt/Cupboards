using System.Collections.Generic;
using UnityEngine;

namespace CupBoardsLevelSettings
{
    public class NodeModel
    {
        public Vector2 Position { get; }
        public List<int> Neighbours { get; }

        public NodeModel(Vector2 position)
        {
            Position = position;
            Neighbours = new List<int>();
        }
    }
}