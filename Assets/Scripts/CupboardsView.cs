using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс отвечающий за визуальное представление фишек
/// </summary>
public class CupboardsView : MonoBehaviour
{
    public event Action<Cupboard> OnCupboardClicked;

    [SerializeField]
    private Color[] _colors;

    [SerializeField]
    private Cupboard _cupboardPrefab;

    public void Initialize(GraphView graphView, List<int> cupboardPositions)
    {
        CreateCupboards(graphView, cupboardPositions);
    }

    private void CreateCupboards(GraphView graphView, IReadOnlyList<int> cupboardPositions)
    {
        for (var index = 0; index < cupboardPositions.Count; index++)
        {
            var cupboardsStartIndex = cupboardPositions[index];
            var node = graphView.GetNode(cupboardsStartIndex - 1);
            var color = _colors[index];

            // создаем визуальное представление конретной фишки
            var cupboard = Instantiate(_cupboardPrefab, transform);
            // При клике на фишку - мы вызываем ивент OnCupboardClicked
            cupboard.SetClickCallback(value => OnCupboardClicked?.Invoke(value));
            cupboard.SetColor(color);
            cupboard.SetPosition(node);
        }
    }
}