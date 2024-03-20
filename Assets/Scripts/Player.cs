using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private int healthPoints; //health points
    private int actionPoints; //action points
    [SerializeField] private int movementPoints = 3; //movement points
    private int damage; //damage
    private int resistancePerc; //resistance %
    private int critsPerc; //crits %
    private Vector2Int _coords;
    public Vector2Int Coords { get => _coords; }


    private void Awake()
    {
        _coords = Vector2Int.RoundToInt(transform.position);
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
            _coords = Vector2Int.RoundToInt(transform.position);
            movementPoints -= diff;

        }
        else
        {
            Debug.Log("You cant move, sorry :3");
        }
    }

    private bool CanMove(Tile tile, out int diff)
    {
        diff = 0;

        if (Coords == tile.Coords)
        {
            return false;
        }

        if (Coords.x == tile.Coords.x && Coords.y != tile.Coords.y)
        {
            int diffY = Mathf.Abs(Coords.y - tile.Coords.y);
            diff = diffY;
            return diffY <= movementPoints;
        }
        else if (Coords.y == tile.Coords.y && Coords.x != tile.Coords.x)
        {
            int diffX = Mathf.Abs(Coords.x - tile.Coords.x);
            diff = diffX;
            return diffX <= movementPoints;
        }
        return false;
    }

}
