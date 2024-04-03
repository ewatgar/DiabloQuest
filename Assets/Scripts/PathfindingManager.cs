using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    private static PathfindingManager _instance;
    public static PathfindingManager Instance { get => _instance; }

    private const int STEP_LIMIT_DEFAULT = 300;
    private Tile _startTile, _goalTile, _solidTile, _currentTile;
    public Tile StartTile { get => _startTile; }
    public Tile GoalTile { get => _goalTile; }
    public Tile SolidTile { get => _solidTile; }
    public Tile CurrentTile { get => _currentTile; }

    private List<Tile> _openList = new List<Tile>();
    private List<Tile> _checkedList = new List<Tile>();
    private bool _bGoalReached = false;
    private int _step = 0;

    private List<Tile> _finalPath = new List<Tile>();
    public List<Tile> FinalPath { get => _finalPath; }

    private GridGenerator _grid;

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

    private void Start()
    {
        _grid = GridGenerator.Instance;
        SetSolidTiles();
    }

    private void SetSolidTiles()
    {
        Tile[,] tileGrid = _grid.TileGrid;
        SetSolidTile(tileGrid[4, 1]);
        SetSolidTile(tileGrid[4, 2]);
        SetSolidTile(tileGrid[4, 3]);
        SetSolidTile(tileGrid[4, 4]);
        SetSolidTile(tileGrid[4, 5]);
        SetSolidTile(tileGrid[5, 1]);
        SetSolidTile(tileGrid[6, 1]);
        SetSolidTile(tileGrid[7, 1]);
    }

    public void SetPathfinding(Tile startTile, Tile goalTile, int limit = STEP_LIMIT_DEFAULT)
    {
        SetStartTile(startTile);
        SetGoalTile(goalTile);
        SetCostOnAllTiles();
        Search(limit);
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

    private void SetCostOnAllTiles()
    {
        Tile[,] tileGrid = _grid.TileGrid;
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

    private void Search(int limit)
    {
        while (!_bGoalReached && _step < limit)
        {
            _currentTile.SetAsChecked();
            _checkedList.Add(_currentTile);
            _openList.Remove(_currentTile);

            int col = _currentTile.Coords.x;
            int row = _currentTile.Coords.y;
            Tile[,] tileGrid = _grid.TileGrid;

            //Up Tile
            if (row - 1 >= 0) OpenTile(tileGrid[col, row - 1]);

            //Left Tile
            if (col - 1 >= 0) OpenTile(tileGrid[col - 1, row]);

            //Down Tile
            if (row + 1 < _grid.NRows) OpenTile(tileGrid[col, row + 1]);

            //Right Tile
            if (col + 1 < _grid.NCols) OpenTile(tileGrid[col + 1, row]);

            //Find best Tile
            int bestTileIndex = 0;
            int bestTileFCost = int.MaxValue;

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

            //print($"bestTileIndex: {bestTileIndex}");
            _currentTile = _openList[bestTileIndex];

            if (_currentTile == _goalTile)
            {
                _bGoalReached = true;
                TrackFinalPath();
            }
        }
        _step++;
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

    private void TrackFinalPath()
    {
        while (_currentTile != _startTile)
        {
            _currentTile.SetAsPath();
            _finalPath.Add(_currentTile);
            _currentTile = _currentTile.PathfindingParent;
        }
        _finalPath.Reverse();
    }

    public void ClearValues()
    {
        _startTile = _goalTile = _solidTile = _currentTile = null;
        foreach (Tile tile in _openList)
        {
            tile.ClearPathfindingValues();
        }
        foreach (Tile tile in _checkedList)
        {
            tile.ClearPathfindingValues();
        }
        _openList = new List<Tile>();
        _checkedList = new List<Tile>();
        _bGoalReached = false;
        _step = 0;
        _finalPath = new List<Tile>();
    }
}
