using System;
using System.Collections.Generic;
using CupBoardsLevelSettings;
using UnityEngine;

/// <summary>
/// Класс отвечающий за визуальное представление графа
/// </summary>
public class GraphView : MonoBehaviour
{
    public event Action<Node> OnNodeClicked;
    
    [SerializeField]
    private Node _nodePrefab;
    
    [SerializeField]
    private LineRenderer _edge;

    private List<Node> _nodes;

    public void Initialize(LevelSettings settings)
    {
        CreateGraph(settings);
    }

    private void CreateGraph(LevelSettings settings)
    {
        _nodes = new List<Node>();
        foreach (var gameBoardNode in settings.GameBoard.Nodes)
        {
            // создаем визуальное представление ноды
            var node = Instantiate(_nodePrefab, transform);
            // При клике на ноду - мы вызываем ивент OnNodeClicked
            node.SetClickCallback(value => OnNodeClicked?.Invoke(value));
            node.SetPosition(gameBoardNode.Position);

            _nodes.Add(node);
        }

        AddNeighboursAndEdges(settings);
    }

    private void AddNeighboursAndEdges(LevelSettings settings)
    {
        for (var index = 0; index < settings.GameBoard.Nodes.Count; index++)
        {
            var nodeModel = settings.GameBoard.Nodes[index];

            var currentNodeView = _nodes[index];
            foreach (var neighbourIndex in nodeModel.Neighbours)
            {
                var neighbourNodeView = _nodes[neighbourIndex];
                currentNodeView.AddNeighbour(neighbourNodeView);

                var edge = Instantiate(_edge);
                edge.positionCount = 2;
                edge.SetPosition(0, currentNodeView.Position);
                edge.SetPosition(1, neighbourNodeView.Position);
            }
        }
    }

    public Node GetNode(int index)
    {
        return _nodes[index];
    }
}