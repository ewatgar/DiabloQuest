using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    private Dictionary<Vector2Int, Tile> _tileDict = new Dictionary<Vector2Int, Tile>();

    public static GridManager Instance { get => _instance; }
    public Dictionary<Vector2Int, Tile> TileDict { get => _tileDict; }

    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;

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

    void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile newTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                newTile.name = $"({x},{y})";
                newTile.transform.parent = transform;
                Vector2Int tileCoords = new Vector2Int(x, y);
                _tileDict.Add(tileCoords, newTile);
            }
        }
    }
}
