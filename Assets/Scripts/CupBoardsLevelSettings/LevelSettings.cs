using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CupBoardsLevelSettings
{
    /// <summary>
    /// Парсинг файла с конфигом - обсуждали реализацию на встрече:
    /// https://www.youtube.com/watch?v=xhsutfdFnx4&list=PLZLvRAPO09LHNyQXa90HPU4RM4Sx5U0q8&index=8
    /// </summary>
    public class LevelSettings
    {
        private readonly StreamReader _streamReader;
    
        public List<int> CupboardsStartPositions { get; private set; }
        public List<int> CupboardsFinishPositions { get; private set; }

        private readonly int _nodeCount;
        private int _cupboardsCount;

        public GraphModel GameBoard { get; }
    
        public LevelSettings(string path)
        {
            _streamReader = new StreamReader(path);
            _cupboardsCount = GetCupboardsCount();
            _nodeCount = GetNodeCount();
            GameBoard = ReadGraphStructure();
        }

        private int GetCupboardsCount()
        {
            var cupboardsCount = Convert.ToInt32(_streamReader.ReadLine());
            return cupboardsCount;
        }

        private int GetNodeCount()
        {
            var nodeCount = Convert.ToInt32(_streamReader.ReadLine());
            return nodeCount;
        }

        private List<Vector2> GetNodeCoordinates()
        {
            var nodeCoordinates = new List<Vector2>();
        
            for (int i = 0; i < _nodeCount; i++)
            {
                var stringCoordinate = _streamReader.ReadLine()?.Split(',');

                var x = Convert.ToInt32(stringCoordinate?[0]);
                var y = Convert.ToInt32(stringCoordinate?[1]);

                var position = new Vector2(x,y);
            
                nodeCoordinates.Add(position);
            }

            return nodeCoordinates;
        }

        private List<int> GetStartPositions()
        {
            var startPositionsInString = _streamReader.ReadLine().Split(',');
        
            var startPositions = startPositionsInString.Select(position => Convert.ToInt32(position)).ToList();
            return startPositions;
        }
    
        private List<int> GetFinishPositions()
        {
            var finishPositionsInString = _streamReader.ReadLine().Split(',');
        
            var finishPositions = finishPositionsInString.Select(position => Convert.ToInt32(position)).ToList();
            return finishPositions;
        }
    
        private GraphModel ReadGraphStructure()
        {
            var nodeCoordinates = GetNodeCoordinates();
        
            var nodes = new List<NodeModel>();
            for (var i = 0; i < _nodeCount; i++)
            {
                nodes.Add(new NodeModel(nodeCoordinates[i]));
            }
        
            CupboardsStartPositions = GetStartPositions();
            CupboardsFinishPositions = GetFinishPositions();

            var countOfConnections = Convert.ToInt32(_streamReader.ReadLine());

            for (var i = 0; i < countOfConnections; i++)
            {
                var connections = _streamReader.ReadLine().Split(',');
                var firstNodeIndex = Convert.ToInt32(connections[0])-1;
                var secondNodeIndex = Convert.ToInt32(connections[1])-1;
            
                nodes[firstNodeIndex].Neighbours.Add(secondNodeIndex);
                nodes[secondNodeIndex].Neighbours.Add(firstNodeIndex);
            }

            var graph = new GraphModel(nodes);
            return graph;
        }
    }
}