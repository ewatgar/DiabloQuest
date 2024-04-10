using System;
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

    //Movement
    private bool _bPlayerTurn = false;
    private Tile _selectedTile = null;

    public int HealthPoints { get => _healthPoints; set => _healthPoints = value; }
    public int ActionPoints { get => _actionPoints; set => _actionPoints = value; }
    public int MovementPoints { get => _movementPoints; set => _movementPoints = value; }
    public int Damage { get => _damage; set => _damage = value; }
    public int ResistancePerc { get => _resistancePerc; set => _resistancePerc = value; }
    public int CritsPerc { get => _critsPerc; set => _critsPerc = value; }

    private void Start()
    {
        AddAsObserverToAllTiles();
    }

    private void AddAsObserverToAllTiles()
    {
        Tile[,] tileGrid = GridGenerator.Instance.TileGrid;
        foreach (Tile tile in tileGrid)
        {
            //tile.OnTileHovered += HandleTileHovered;
            //tile.OnTileClicked += HandleTileClicked;
        }
    }

    private void HandleTileClicked(Vector3 coordinates)
    {
        //TODO corrutina
    }

    public Tile GetPlayerTile()
    {
        return GridGenerator.Instance.GetTileFromWorldCoords(transform.position);
    }

    public void GetHurt(int hp = 1)
    {
        HealthPoints -= hp;
    }

    public void SpendMovementPoint()
    {
        MovementPoints--;
    }

    /*
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
        }*/


}
