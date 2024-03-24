using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager Instance { get => _instance; }

    private Dictionary<Vector2Int, Tile> _tileDict = new Dictionary<Vector2Int, Tile>();
    public Dictionary<Vector2Int, Tile> TileDict { get => _tileDict; }

    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private float _gridScale;

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
        float globalX = 0;
        for (int x = 0; x < _width; x++)
        {
            float globalY = 0;
            for (int y = 0; y < _height; y++)
            {
                Tile newTile = Instantiate(_tilePrefab, new Vector3(globalX, globalY, 9), Quaternion.identity);
                newTile.Coords = new Vector2Int(x, y);
                newTile.name = newTile.Coords.ToString();
                newTile.transform.parent = transform;
                _tileDict.Add(newTile.Coords, newTile);
                globalY += _gridScale;
            }
            globalX += _gridScale;
        }
    }
}
