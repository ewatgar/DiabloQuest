using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _pathTileColor;
    private Color _baseColor;
    private SpriteRenderer _sprite;
    private Vector2Int _coords;

    public Color BaseColor { get => _baseColor; }
    public Color PathTileColor { get => _pathTileColor; }
    public SpriteRenderer Sprite { get => _sprite; }
    public Vector2Int Coords { get => _coords; }

    //A* Pathfinding Fields
    private Tile _parentTile;
    private int _gCost;
    private int _hCost;
    private int _fCost;
    private bool _bStart;
    private bool _bGoal;
    private bool _bSolid;
    private bool _bOpen;
    private bool _bChecked;

    public Tile ParentTile { get => _parentTile; set => _parentTile = value; }
    public int GCost { get => _gCost; set => _gCost = value; }
    public int HCost { get => _hCost; set => _hCost = value; }
    public int FCost { get => _fCost; set => _fCost = value; }
    public bool StartTile { get => _bStart; }
    public bool Goal { get => _bGoal; }
    public bool Solid { get => _bSolid; }
    public bool Open { get => _bOpen; }
    public bool Checked { get => _bChecked; }

    public event Action<Tile> OnTileHovered;
    public event Action<Tile> OnTileClicked;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _baseColor = _sprite.color;
        _coords = GridManager.Instance.WorldToTileCoords(transform.position);
    }

    private void OnMouseEnter()
    {
        //print($"coords: {Coords}");
        OnTileHovered?.Invoke(this);
    }

    private void OnMouseDown()
    {

        OnTileClicked?.Invoke(this);
    }

    //A* Pathfinding Methods
    public void ClearPathfindingValues()
    {
        _gCost = int.MaxValue;
        _hCost = int.MaxValue;
        _fCost = int.MaxValue;
        _bStart = false;
        _bGoal = false;
        _bSolid = false;
        _bOpen = false;
        _bChecked = false;
        _sprite.color = _baseColor;
    }

    public void SetAsStart()
    {
        _bStart = true;
    }

    public void SetAsGoal()
    {
        _bGoal = true;
    }

    public void SetAsSolid()
    {
        _bSolid = true;
        _sprite.color = Color.black;
    }

    public void SetAsNotSolid()
    {
        _bSolid = false;
        _sprite.color = _baseColor;
    }

    public void SetAsOpen()
    {
        _bOpen = true;
    }

    public void SetAsChecked()
    {
        if (!_bStart && !_bGoal) _bChecked = true;
    }

    public void ColorPathTile()
    {
        _sprite.color = _pathTileColor;
    }

    public void UncolorPathTile()
    {
        _sprite.color = _baseColor;
    }


}
