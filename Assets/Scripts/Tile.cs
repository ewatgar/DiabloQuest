using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _highlightColor;
    private SpriteRenderer _sprite;
    private Color _baseColor;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _baseColor = _sprite.color;
        print("awake tile");
    }

    private void OnMouseEnter()
    {
        print(name);
        _sprite.color = _highlightColor;
    }

    private void OnMouseExit()
    {
        _sprite.color = _baseColor;
    }
}
