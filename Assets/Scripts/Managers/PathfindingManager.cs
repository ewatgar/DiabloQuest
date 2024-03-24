using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    private static PathfindingManager _instance;
    public static PathfindingManager Instance { get => _instance; }



    private Tile _startTile, _goalTile, _solidTile, _currentTile;
    public Tile StartTile { get => _startTile; }
    public Tile GoalTile { get => _goalTile; }
    public Tile SolidTile { get => _solidTile; }
    public Tile CurrentTile { get => _currentTile; }

    private List<Tile> _openList = new List<Tile>();
    private List<Tile> _checkedList = new List<Tile>();
    private bool _bGoalReached = false;

    private bool _bInitPlayer = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_bInitPlayer && Input.GetKey(KeyCode.Space))
        {
            Search();
        }

    }

    public void InitPlayerPathfinding(Tile selectedTile)
    {
        Tile[,] tileGrid = GridGenerator.Instance.TileGrid;
        GameObject _player = GameObject.FindGameObjectsWithTag("Player").First();
        Vector3 playerPos = _player.transform.position;
        Tile tileFromPlayer = GridGenerator.Instance.GetTileFromWorldCoords(playerPos);
        SetStartTile(tileFromPlayer);
        SetGoalTile(selectedTile);
        SetSolidTile(tileGrid[4, 1]);
        SetSolidTile(tileGrid[4, 2]);
        SetSolidTile(tileGrid[4, 3]);
        SetSolidTile(tileGrid[4, 4]);
        SetSolidTile(tileGrid[4, 5]);
        SetSolidTile(tileGrid[5, 1]);
        SetSolidTile(tileGrid[6, 1]);
        SetSolidTile(tileGrid[7, 1]);
        SetCostOnTiles();
        _bInitPlayer = true;
    }

    private void SetStartTile(Tile tile)
    {
        _startTile = tile;
        _currentTile = _startTile;
        tile.SetAsStart();
    }

    private void SetGoalTile(Tile tile)
    {
        _goalTile = tile;
        tile.SetAsGoal();
    }

    private void SetSolidTile(Tile tile)
    {
        _solidTile = tile;
        tile.SetAsSolid();
    }

    private void SetCostOnTiles()
    {
        Tile[,] tileGrid = GridGenerator.Instance.TileGrid;
        foreach (Tile tile in tileGrid)
        {
            SetTileCost(tile);
        }
    }

    private void SetTileCost(Tile tile)
    {
        int xDistance = Mathf.Abs(tile.Coords.x - _startTile.Coords.x);
        int yDistance = Mathf.Abs(tile.Coords.y - _startTile.Coords.y);
        tile.GCost = xDistance + yDistance;

        xDistance = Mathf.Abs(tile.Coords.x - _goalTile.Coords.x);
        yDistance = Mathf.Abs(tile.Coords.y - _goalTile.Coords.y);
        tile.HCost = xDistance + yDistance;

        tile.FCost = tile.GCost + tile.HCost;
    }

    public void Search()
    {
        if (!_bGoalReached)
        {
            _currentTile.SetAsChecked();
            _checkedList.Add(_currentTile);
            _openList.Remove(_currentTile);

            int col = _currentTile.Coords.x;
            int row = _currentTile.Coords.y;
            Tile[,] tileGrid = GridGenerator.Instance.TileGrid;

            //Up Tile
            if (row - 1 >= 0) OpenTile(tileGrid[col, row - 1]);

            //Left Tile
            if (col - 1 >= 0) OpenTile(tileGrid[col - 1, row]);

            //Down Tile
            if (row + 1 < GridGenerator.Instance.NRows) OpenTile(tileGrid[col, row + 1]);

            //Right Tile
            if (row + 1 < GridGenerator.Instance.NCols) OpenTile(tileGrid[col + 1, row]);

            //Find best Tile
            int bestTileIndex = 0;
            int bestTileFCost = 9999;

            for (int i = 0; i < _openList.Count; i++)
            {
                int fCost;
                int gCost;
                //Check if tile F Cost is better than the last one
                if ((fCost = _openList[i].FCost) < bestTileFCost)
                {
                    bestTileIndex = i;
                    bestTileFCost = fCost;
                }
                //Check if G Cost is better than the last one
                else if (_openList[i].FCost == bestTileFCost && (gCost = _openList[i].GCost) > _openList[bestTileIndex].GCost)
                {
                    bestTileIndex = i;
                }
            }

            _currentTile = _openList[bestTileIndex];

            if (_currentTile == _goalTile)
            {
                _bGoalReached = true;
                TrackThePath();
            }

        }
    }

    private void OpenTile(Tile tile)
    {
        if (!tile.BOpen && !tile.BChecked && !tile.BSolid)
        {
            tile.SetAsOpen();
            tile.PathfindingParent = _currentTile;
            _openList.Add(tile);
        }
    }

    private void TrackThePath()
    {
        Tile current = _goalTile;
        while (current != _startTile)
        {
            current = current.PathfindingParent;

            if (current != _startTile)
            {
                current.SetAsPath();
            }
        }
    }

}
