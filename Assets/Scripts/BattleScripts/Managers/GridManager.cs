using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager Instance { get => _instance; }

    private Tile[,] _tileGrid;
    public Tile[,] TileGrid { get => _tileGrid; }

    [SerializeField] private int _nCols = 13, _nRows = 7;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private float _gridScale = 1.5f;
    public float GridScale { get => _gridScale; set => _gridScale = value; }
    public int NCols { get => _nCols; }
    public int NRows { get => _nRows; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            GenerateGrid();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void GenerateGrid()
    {
        _tileGrid = new Tile[_nCols, _nRows];

        float globalX = 0;
        for (int x = 0; x < _nCols; x++)
        {
            float globalY = 0;
            for (int y = 0; y < _nRows; y++)
            {
                Tile newTile = Instantiate(_tilePrefab, new Vector3(globalX, globalY, 9), Quaternion.identity);
                newTile.name = newTile.Coords.ToString();
                newTile.transform.parent = transform;
                _tileGrid[x, y] = newTile;
                globalY += _gridScale;
            }
            globalX += _gridScale;
        }
    }

    public Vector2Int WorldToTileCoords(Vector3 worldCoords)
    {
        worldCoords += new Vector3(_gridScale / 2f, _gridScale / 2f, 0f);

        int tileX = Mathf.FloorToInt(worldCoords.x / _gridScale);
        int tileY = Mathf.FloorToInt(worldCoords.y / _gridScale);

        return new Vector2Int(tileX, tileY);
    }

    public Tile GetTileFromTileCoords(Vector2Int coords)
    {
        //return _tileGrid[coords.x, coords.y];
        try
        {
            return _tileGrid[coords.x, coords.y];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    public Tile GetTileFromWorldCoords(Vector3 worldCoords)
    {
        return GetTileFromTileCoords(WorldToTileCoords(worldCoords));
    }

    public void AddAsObserverToAllTiles(Action<Tile> HandleTileHovered, Action<Tile> HandleTileClicked)
    {
        foreach (Tile tile in _tileGrid)
        {
            tile.OnTileHovered += HandleTileHovered;
            tile.OnTileClicked += HandleTileClicked;
        }
    }

    public void AddAsObserverToAllTiles(Action<Tile> HandleTileClicked)
    {
        foreach (Tile tile in _tileGrid)
        {
            tile.OnTileClicked += HandleTileClicked;
        }
    }

    public void ClearSolidTiles()
    {
        foreach (Tile tile in _tileGrid)
        {
            tile.Solid = false;
        }
    }

    public void ClearSpellTiles()
    {
        foreach (Tile tile in _tileGrid)
        {
            tile.SpellSelectable = false;
            tile.SpellAreaEffect = false;
            tile.SpellAreaEffectNoAP = false;
        }
    }

    public static int DistanceBetweenTiles(Tile tileA, Tile tileB)
    {
        int xDistance = Mathf.Abs(tileA.Coords.x - tileB.Coords.x);
        int yDistance = Mathf.Abs(tileA.Coords.y - tileB.Coords.y);
        return xDistance + yDistance;
    }

    public Tile GetTileFromDirection(Tile tile, Vector2Int dir)
    {
        return GetTileFromTileCoords(tile.Coords + dir); ;
    }

    public Vector2Int GetMeleeDirFromTwoTiles(Tile tileStart, Tile tileDir)
    {
        Vector2Int dir = Vector2Int.up;
        if (tileDir == GetTileFromDirection(tileStart, Vector2Int.up)) dir = Vector2Int.up;
        if (tileDir == GetTileFromDirection(tileStart, Vector2Int.down)) dir = Vector2Int.down;
        if (tileDir == GetTileFromDirection(tileStart, Vector2Int.left)) dir = Vector2Int.left;
        if (tileDir == GetTileFromDirection(tileStart, Vector2Int.right)) dir = Vector2Int.right;
        return dir;
    }

    /*
    public Vector2Int GetMeleeDirFromTwoTiles(Tile tileStart, Tile tileDir)
    {
        Vector2 direction = tileDir.Coords - tileStart.Coords;
        Vector2Int directionNormalized = Vector2Int.RoundToInt(direction.normalized); ;
        return directionNormalized;
    }*/
}
