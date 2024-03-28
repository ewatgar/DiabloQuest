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

    public Color BaseColor { get => _baseColor; }
    public Color CanMoveColor { get => _canMoveColor; }
    public Color CanNotMoveColor { get => _canNotMoveColor; }
    public SpriteRenderer Sprite { get => _sprite; }
    public Vector2Int Coords { get => _coords; }

    public event Action<Tile> OnTileHovered;
    public event Action<Tile> OnTileExited;
    public event Action<Tile> OnTileClicked;

    //A* Pathfinding Fields
    private Tile _pathfindingParent;
    private int _gCost;
    private int _hCost;
    private int _fCost;
    private bool _bStart;
    private bool _bGoal;
    private bool _bSolid;
    private bool _bOpen;
    private bool _bChecked;

    public Tile PathfindingParent { get => _pathfindingParent; set => _pathfindingParent = value; }
    public int GCost { get => _gCost; set => _gCost = value; }
    public int HCost { get => _hCost; set => _hCost = value; }
    public int FCost { get => _fCost; set => _fCost = value; }
    public bool BStart { get => _bStart; }
    public bool BGoal { get => _bGoal; }
    public bool BSolid { get => _bSolid; set => _bSolid = value; }
    public bool BOpen { get => _bOpen; }
    public bool BChecked { get => _bChecked; }

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _baseColor = _sprite.color;
        _coords = GridGenerator.Instance.WorldToTileCoords(transform.position);
    }

    private void OnMouseEnter()
    {
        GameManager.Instance.HandleTileHovered(this);
        /*
        if (!BSolid)
        {

            InitPlayerPathfinding();
        }*/
    }

    private void OnMouseDown()
    {
        GameManager.Instance.HandleTileClicked(this);
        //print($"name: {name}");
        //print($"coords: {Coords}");
        /*if (!BSolid)
        {
            InitPlayerMoving();
        }*/
    }

    private void OnMouseExit()
    {
        //GameManager.Instance.HandleTileExited(this);
        //PathfindingManager.Instance.ClearValues();
    }

    //A* Pathfinding Methods
    /*
    private void InitPlayerPathfinding()
    {
        Vector3 playerPos = _player.transform.position;
        Tile tileFromPlayer = GridGenerator.Instance.GetTileFromWorldCoords(playerPos);
        PathfindingManager.Instance.InitPathfinding(tileFromPlayer, this);
    }

    private void InitPlayerMoving()
    {
        StartCoroutine(MovePlayer());
        PathfindingManager.Instance.ClearValues();
    }

    IEnumerator MovePlayer()
    {
        _player.transform.position = transform.position;
        yield return new WaitForSeconds(5);
        yield return null;
    }*/

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
        _sprite.color = Color.cyan;
    }

    public void SetAsGoal()
    {
        _bGoal = true;
        _sprite.color = _canMoveColor;
    }

    public void SetAsSolid()
    {
        _bSolid = true;
        _sprite.color = Color.black;
    }

    public void SetAsOpen()
    {
        _bOpen = true;
    }

    public void SetAsChecked()
    {
        if (!_bStart && !_bGoal) _bChecked = true;
    }

    public void SetAsPath()
    {
        _sprite.color = _canMoveColor;
    }


}
