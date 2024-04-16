using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _pathTileColor;
    [SerializeField] private Color _spellTileColor;
    private Color _baseColor;
    private SpriteRenderer _sprite;
    private Vector2Int _coords;

    public Color BaseColor { get => _baseColor; }
    public Color PathTileColor { get => _pathTileColor; }
    public Color SpellTileColor { get => _spellTileColor; set => _spellTileColor = value; }

    public SpriteRenderer Sprite { get => _sprite; }
    public Vector2Int Coords { get => _coords; }

    private bool _bPath;
    private bool _bSpell;

    public bool Path { get => _bPath; set => _bPath = value; }
    public bool Spell { get => _bSpell; set => _bSpell = value; }

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
    public bool Start { get => _bStart; set => _bStart = value; }
    public bool Goal { get => _bGoal; set => _bGoal = value; }
    public bool Solid { get => _bSolid; set => _bSolid = value; }
    public bool Open { get => _bOpen; set => _bOpen = value; }
    public bool Checked { get => _bChecked; set => _bChecked = value; }

    public event Action<Tile> OnTileHovered;
    public event Action<Tile> OnTileClicked;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _baseColor = _sprite.color;
        _coords = GridManager.Instance.WorldToTileCoords(transform.position);
    }

    private void Update()
    {
        CheckTileColor();
    }

    private void CheckTileColor()
    {
        if (_bSolid) _sprite.color = Color.black;
        else if (_bPath) _sprite.color = _pathTileColor;
        else if (_bSpell) _sprite.color = _spellTileColor;
        else _sprite.color = _baseColor;
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
        _bPath = false;
    }

    public void SetAsChecked()
    {
        if (!_bStart && !_bGoal) _bChecked = true;
    }
}
