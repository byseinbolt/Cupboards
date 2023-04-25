using TMPro;
using UnityEngine;

public class MovesCountView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _movesCountLabel;

    private int _movesCount;

    private void Awake()
    {
        _movesCountLabel.text = "Moves: 0";
    }

    public void ShowCurrentMovesCount()
    {
        _movesCount += 1;
        _movesCountLabel.text = $"Moves: {_movesCount.ToString()}";
    }
}