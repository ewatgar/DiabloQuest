using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    private static PathfindingManager _instance;
    public static PathfindingManager Instance { get => _instance; }

    private Tile _startTile, _goalTile, _currentTile;
    public Tile StartTile { get => _startTile; }
    public Tile GoalTile { get => _goalTile; }
    public Tile CurrentTile { get => _currentTile; }

    private List<Tile> _openList = new List<Tile>();
    private List<Tile> _checkedList = new List<Tile>();
    private bool _bGoalReached = false;

    private List<Tile> _finalPath = new List<Tile>();
    public List<Tile> FinalPath { get => _finalPath; }

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

    public void RegeneratePath(Tile startTile, Tile goalTile, bool color)
    {
        ClearValues();
        SetPathfinding(startTile, goalTile);
        if (color) ColorFinalPath();
    }

    public void SetPathfinding(Tile startTile, Tile goalTile)
    {
        SetStartTile(startTile);
        SetGoalTile(goalTile);
        SetCostOnAllTiles();
        Search();
    }

    private void SetStartTile(Tile tile)
    {
        _startTile = tile;
        _currentTile = _startTile;
        tile.Start = true;
    }

    private void SetGoalTile(Tile tile)
    {
        _goalTile = tile;
        tile.Goal = true;
    }

    private void SetCostOnAllTiles()
    {
        Tile[,] tileGrid = GridManager.Instance.TileGrid;
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

    private List<Tile> Search()
    {
        GridManager _grid = GridManager.Instance;
        while (!_bGoalReached)
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
            }
        }

        TrackFinalPath();
        return _finalPath;
    }

    private void OpenTile(Tile tile)
    {
        if (!tile.Open && !tile.Checked && !tile.Solid)
        {
            tile.Open = true;
            tile.ParentTile = _currentTile;
            _openList.Add(tile);
        }
    }

    private void TrackFinalPath()
    {
        while (_currentTile != _startTile)
        {
            _finalPath.Add(_currentTile);
            _currentTile = _currentTile.ParentTile;
        }
        _finalPath.Reverse();
    }

    public void ColorFinalPath()
    {
        foreach (Tile tile in _finalPath)
        {
            tile.Path = true;
        }
    }

    public void ClearValues()
    {
        _startTile = _goalTile = _currentTile = null;
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
        _finalPath = new List<Tile>();
    }




}
