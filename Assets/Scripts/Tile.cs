using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _canMoveColor;
    [SerializeField] private Color _canNotMoveColor;
    private Color _baseColor;
    private SpriteRenderer _sprite;
    private Vector2Int _coords;

    public Color CanMoveColor { get => _canMoveColor; }
    public Color CanNotMoveColor { get => _canNotMoveColor; }
    public SpriteRenderer Sprite { get => _sprite; }
    public Vector2Int Coords { get => _coords; }

    public event Action<Tile> OnTileClicked;
    public event Action<Tile> OnTileHovered;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _coords = Vector2Int.RoundToInt(transform.position);
        _baseColor = _sprite.color;
    }

    private void OnMouseEnter()
    {
        OnTileHovered?.Invoke(this);
    }

    private void OnMouseDown()
    {
        print(name);
        OnTileClicked?.Invoke(this);
    }

    private void OnMouseExit()
    {
        _sprite.color = _baseColor;
    }


}
