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

    //A* Pathfinding
    private int _gCost;
    private int _hCost;
    private int _fCost;
    bool _bStart;
    bool _bGoal;
    bool _bOpen;
    bool _bChecked;

    public Color BaseColor { get => _baseColor; }
    public Color CanMoveColor { get => _canMoveColor; }
    public Color CanNotMoveColor { get => _canNotMoveColor; }
    public SpriteRenderer Sprite { get => _sprite; }
    public Vector2Int Coords { get => _coords; }

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _baseColor = _sprite.color;
        _coords = Utils.WorldToTileCoords(transform.position, GridGenerator.Instance.GridScale);
    }

    private void OnMouseEnter()
    {

    }

    private void OnMouseDown()
    {
        print($"name: {name}");
        print($"coords: {Coords}");
    }

    private void OnMouseExit()
    {
    }


}
