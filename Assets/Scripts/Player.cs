using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Player Stats
    private int _healthPoints; //health points
    private int _actionPoints; //action points
    [SerializeField] private int _movementPoints = 3; //movement points
    private int _damage; //damage
    private int _resistancePerc; //resistance %
    private int _critsPerc; //crits %

    //private bool _bCurrentTurn;
    private Vector2Int _coords;
    public Vector2Int Coords { get => _coords; }


    private void Awake()
    {
        _coords = Vector2Int.RoundToInt(transform.position);
        //_bCurrentTurn = false;
        AddObserverToAllTiles();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void AddObserverToAllTiles()
    {
        Dictionary<Vector2Int, Tile> tileDict = GridManager.Instance.TileDict;
        foreach (var tile in tileDict)
        {
            tile.Value.OnTileHovered += HandleTileHovered;
            tile.Value.OnTileClicked += HandleTileClicked;
        }
    }

    private void HandleTileHovered(Tile tile)
    {
        if (CanMove(tile, out int diff))
        {
            tile.Sprite.color = tile.CanMoveColor;
        }
        else
        {
            tile.Sprite.color = tile.CanNotMoveColor;
        }
    }

    private void HandleTileClicked(Tile tile)
    {
        tile.Sprite.color = tile.CanNotMoveColor;

        if (CanMove(tile, out int diff))
        {
            //TODO player lerp animation
            transform.position = tile.transform.position;
            _coords = tile.Coords;
            _movementPoints -= diff;
        }
        else
        {
            Debug.Log("You cant move, sorry :3");
        }
    }

    private bool CanMove(Tile tile, out int diff)
    {
        diff = 0;

        /*
        if (!_bCurrentTurn)
        {
            return false;
        }*/

        if (Coords == tile.Coords)
        {
            return false;
        }

        if (Coords.x == tile.Coords.x && Coords.y != tile.Coords.y)
        {
            int diffY = Mathf.Abs(Coords.y - tile.Coords.y);
            diff = diffY;
            return diffY <= _movementPoints;
        }
        else if (Coords.y == tile.Coords.y && Coords.x != tile.Coords.x)
        {
            int diffX = Mathf.Abs(Coords.x - tile.Coords.x);
            diff = diffX;
            return diffX <= _movementPoints;
        }
        return false;
    }

}
