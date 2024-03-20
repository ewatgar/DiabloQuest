using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager Instance { get => _instance; }

    private List<Tile> _tileList = new List<Tile>();
    public List<Tile> TileList { get => _tileList; }

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
                var newTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                newTile.name = $"({x},{y})";
                newTile.transform.parent = transform;
                _tileList.Add(newTile);
            }
        }
    }
}
