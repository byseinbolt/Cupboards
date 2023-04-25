using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, который представляет нашу ноду в графе и ее визуальную составляющую.
/// </summary>
public class Node : MonoBehaviour
{
    public Vector2 Position => transform.position;
    public bool IsWalkable { get; set; }

    /// <summary>
    /// Ссылка на изображение бэка нашей ноды.
    /// У этого изображения мы дальше в коде меняем цвет.
    /// </summary>
    [SerializeField]
    private Image _background;
    
    /// <summary>
    /// Соседние ноды, до которых мы можем добраться через текущую ноду.
    /// 
    /// Поскольку этот список помечен как SerializeField, то у нас в редакторе
    /// есть возможность его настраивать.
    /// </summary>
    [SerializeField]
    private List<Node> _neighbours;

    [SerializeField]
    private RectTransform _rectTransform;

    private Action<Node> _onClicked;

    private void Awake()
    {
        IsWalkable = true;
    }

    public void SetPosition(Vector2 position)
    {
        _rectTransform.anchoredPosition = position;
    }

    /// <summary>
    /// Пример использования инкапсуляции:
    /// 
    /// Мы возвращает не просто List<Node>, а IReadOnlyList (который List реализует в .NET по умолчанию)
    /// это интерфейс, который по умолчанию не позволяет наш List редактировать
    /// т.е. добавлять / удалять элементы.
    ///
    /// Сейчас в нашем коде - мы нигде не редактируем этот список и казалось бы
    /// можно вернуть просто List, но тогда в будущем кто-то может случайно неправильно им
    /// воспользоваться и сломать всю логику. Поэтому сразу пишем более надежный вариант.
    /// </summary>
    public IReadOnlyList<Node> GetNeighbours()
    {
        return _neighbours;
    }

    public void AddNeighbour(Node node)
    {
        _neighbours.Add(node);
    }

    public void Click()
    {
        _onClicked?.Invoke(this);
    }
    
    public void PlayBounceAnimation()
    {
        StopBounceAnimation();
        
        var endValue = new Vector3(1.1f, 1.1f, 1.1f);
        _background.DOColor(Color.green, 1f)
            .SetEase(Ease.OutBounce)
            .SetLoops(-1, LoopType.Yoyo);
        transform.DOScale(endValue, 0.5f)
            .SetEase(Ease.OutBounce)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopBounceAnimation()
    {
        transform.localScale = Vector3.one;
        _background.color = Color.white;
        transform.DOKill();
        _background.DOKill();
    }

    public void SetClickCallback(Action<Node> onClicked)
    {
        _onClicked = onClicked;
    }
}
