using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private static GridGenerator _instance;
    public static GridGenerator Instance { get => _instance; }

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

    private void Start()
    {
        UpdateSolidTiles();
    }

    private void Update()
    {
        UpdateSolidTiles();
    }

    private void UpdateSolidTiles()
    {
        _tileGrid[2, 0].SetAsSolid();
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
        return _tileGrid[coords.x, coords.y];
    }

    public Tile GetTileFromWorldCoords(Vector3 worldCoords)
    {
        return GetTileFromTileCoords(WorldToTileCoords(worldCoords));
    }
}
