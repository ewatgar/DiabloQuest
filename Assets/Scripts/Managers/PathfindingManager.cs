using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    Player player;

    private static PathfindingManager _instance;
    public static PathfindingManager Instance { get => _instance; }

    private Tile _startTile, _goalTile, _currentTile;
    public Tile StartTile { get => _startTile; set => _startTile = value; }
    public Tile GoalTile { get => _goalTile; set => _goalTile = value; }
    public Tile CurrentTile { get => _currentTile; set => _currentTile = value; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            player = GetComponent<Player>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetStartTile(Vector2Int tileCoords)
    {
        Tile tile = GridGenerator.Instance.GetTileFromGridArray(tileCoords);
        _startTile = tile;
        _currentTile = _startTile;
    }

    private void SetGoalTile(Vector2Int tileCoords)
    {
        Tile tile = GridGenerator.Instance.GetTileFromGridArray(tileCoords);
        _goalTile = tile;
    }

    private void PlayerPathfinding()
    {

    }
}
