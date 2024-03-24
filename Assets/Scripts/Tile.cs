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
    private Tile pathfindingParent;
    private int _gCost;
    private int _hCost;
    private int _fCost;
    private bool _bStart;
    private bool _bGoal;
    private bool _bSolid;
    private bool _bOpen;
    private bool _bChecked;

    public Color BaseColor { get => _baseColor; }
    public Color CanMoveColor { get => _canMoveColor; }
    public Color CanNotMoveColor { get => _canNotMoveColor; }
    public SpriteRenderer Sprite { get => _sprite; }
    public Vector2Int Coords { get => _coords; }

    public Tile PathfindingParent { get => pathfindingParent; set => pathfindingParent = value; }
    public int GCost { get => _gCost; set => _gCost = value; }
    public int HCost { get => _hCost; set => _hCost = value; }
    public int FCost { get => _fCost; set => _fCost = value; }
    public bool BStart { get => _bStart; }
    public bool BGoal { get => _bGoal; }
    public bool BSolid { get => _bSolid; set => _bSolid = value; }
    public bool BOpen { get => _bOpen; }
    public bool BChecked { get => _bChecked; }

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _baseColor = _sprite.color;
        _coords = GridGenerator.Instance.WorldToTileCoords(transform.position);
        CleanStates();
    }

    private void OnMouseEnter()
    {

    }

    private void OnMouseDown()
    {
        print($"name: {name}");
        print($"coords: {Coords}");
        PathfindingManager.Instance.InitPlayerPathfinding(this);
    }

    private void OnMouseExit()
    {
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
    }

    public void SetAsOpen()
    {
        _bOpen = true;
    }

    public void SetAsChecked()
    {
        if (!_bStart && !_bGoal) _bChecked = true;
        _sprite.color = Color.yellow;
    }

    public void SetAsPath()
    {
        _sprite.color = Color.green;
    }

    public void CleanStates()
    {
        _gCost = 9999;
        _hCost = 9999;
        _fCost = 9999;
        _bStart = false;
        _bGoal = false;
        _bSolid = false;
        _bOpen = false;
        _bChecked = false;
    }
}
