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

    //Turns
    private bool _bPlayerTurn = false;

    //Movement
    private PathfindingManager _path = null;
    private bool _bPlayerMoving = false;
    private Tile _selectedTile = null;

    public int HealthPoints { get => _healthPoints; set => _healthPoints = value; }
    public int ActionPoints { get => _actionPoints; set => _actionPoints = value; }
    public int MovementPoints { get => _movementPoints; set => _movementPoints = value; }
    public int Damage { get => _damage; set => _damage = value; }
    public int ResistancePerc { get => _resistancePerc; set => _resistancePerc = value; }
    public int CritsPerc { get => _critsPerc; set => _critsPerc = value; }

    private void Start()
    {
        _path = PathfindingManager.Instance;
        AddAsObserverToAllTiles();
    }

    private void AddAsObserverToAllTiles()
    {
        Tile[,] tileGrid = GridGenerator.Instance.TileGrid;
        foreach (Tile tile in tileGrid)
        {
            tile.OnTileHovered += HandleTileHovered;
            tile.OnTileClicked += HandleTileClicked;
        }
    }

    private void HandleTileHovered(Tile tile)
    {
        if (!_bPlayerMoving && !tile.Solid && tile != _selectedTile)
        {
            _selectedTile = tile;
            OnPlayerSelectTileMove(_selectedTile);
        }
    }

    private void HandleTileClicked(Tile tile)
    {
        if (!_bPlayerMoving && !tile.Solid && tile == _selectedTile)
        {
            OnPlayerMoving();
        }
    }

    private void OnPlayerSelectTileMove(Tile selectedTile)
    {
        _path.ClearValues();
        _path.SetPathfinding(GetPlayerTile(), _selectedTile);
        _path.ColorTilesFromFinalPath();
    }

    private void OnPlayerMoving()
    {
        StartCoroutine(MovingThroughPath());
    }

    IEnumerator MovingThroughPath()
    {
        _bPlayerMoving = true;
        print("empieza corrutina");
        float duration = .3f;
        float currentTime = 0;

        List<Tile> path = _path.FinalPath;

        foreach (Tile tile in path)
        {

            Vector3 startPos = transform.position;
            Vector3 endPos = tile.transform.position;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float t = currentTime / duration;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
            transform.position = tile.transform.position;
            currentTime = 0;
            SpendMovementPoint();
            tile.UncolorPathTile();
        }
        print("termina corrutina");
        _bPlayerMoving = false;
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
