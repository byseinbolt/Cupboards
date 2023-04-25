using System;
using UnityEngine;
using UnityEngine.UI;

public class Cupboard : MonoBehaviour
{
    public Node Node { get; private set; }

    [SerializeField]
    private Image _image;

    [SerializeField]
    private RectTransform _rectTransform;

    private Action<Cupboard> _onClick;

    public void Click()
    {
        _onClick?.Invoke(this);
    }

    public void SetColor(Color color)
    {
        _image.color = color;
    }

    public void SetPosition(Node node)
    {
        // Reset previous
        if (Node != null)
        {
            Node.IsWalkable = true;
        }
        
        Node = node;
        node.IsWalkable = false;
        _rectTransform.position = node.Position;
    }

    public void SetClickCallback(Action<Cupboard> onClick)
    {
        _onClick = onClick;
    }
}