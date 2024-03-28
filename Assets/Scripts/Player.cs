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

    private GridGenerator _grid;

    private void Start()
    {
        _grid = GridGenerator.Instance;
    }

    public Tile GetPlayerTile()
    {
        return _grid.GetTileFromWorldCoords(transform.position);
    }

    public void Move(Vector3 position, int _mp = 0)
    {
        transform.position = position;
        _movementPoints -= _mp;
    }

    public void MovePath(List<Tile> path)
    {//TODO movepath player
    }

    public bool CanMove(Vector2Int tileCoords, out int diff)
    {
        Tile playerTile = GetPlayerTile();

        diff = 0;

        if (playerTile.Coords == tileCoords)
        {
            return false;
        }

        if (playerTile.Coords.x == tileCoords.x && playerTile.Coords.y != tileCoords.y)
        {
            int diffY = Mathf.Abs(playerTile.Coords.y - tileCoords.y);
            diff = diffY;
            return diffY <= _movementPoints;
        }
        else if (playerTile.Coords.y == tileCoords.y && playerTile.Coords.x != tileCoords.x)
        {
            int diffX = Mathf.Abs(playerTile.Coords.x - tileCoords.x);
            diff = diffX;
            return diffX <= _movementPoints;
        }
        return false;
    }

    public void GetHurt(int hp = 1)
    {
        _healthPoints -= hp;
    }
}
