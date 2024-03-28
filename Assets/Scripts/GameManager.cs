using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    PlayerIdle,
    PlayerSelectTileMove,
    PlayerMoving,
    PlayerSelectSpell,
    PlayerSelectTileSpell,
    PlayerSpellCasted,
    EnemiesTurn,
    Victory,
    Lose
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    private GameState _gameState;
    public GameState GameState { get => _gameState; set => _gameState = value; }

    private PathfindingManager _path;
    private Player _player;
    private Tile _selectedTile;

    private bool _bPlayerMoving;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _gameState = GameState.PlayerIdle;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _path = PathfindingManager.Instance;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        switch (_gameState)
        {
            case GameState.PlayerIdle:
                OnPlayerIdle();
                break;
            case GameState.PlayerSelectTileMove:
                OnPlayerSelectTileMove(_selectedTile);
                break;
            case GameState.PlayerMoving:
                OnPlayerMoving();
                break;
            case GameState.PlayerSelectSpell:
                break;
            case GameState.PlayerSelectTileSpell:
                break;
            case GameState.PlayerSpellCasted:
                break;
            case GameState.EnemiesTurn:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
        }
    }

    private void OnPlayerIdle()
    {
        _bPlayerMoving = false;
    }

    private void OnPlayerSelectTileMove(Tile selectedTile)
    {
        _path.ClearValues();
        _path.SetPathfinding(_player.GetPlayerTile(), _selectedTile);
    }

    private void OnPlayerMoving()
    {
        StartCoroutine(MovingPath());
        _bPlayerMoving = false;
    }

    IEnumerator MovingPath()
    {
        float duration = .5f;

        List<Tile> path = _path.FinalPath;
        foreach (Tile tile in path)
        {
            float currentTime = 0;
            while (currentTime < duration)
            {
                Vector3 lerpPosition = Vector3.Lerp(
                    _player.transform.position,
                    tile.transform.position,
                    currentTime / duration
                    );
                _player.Move(lerpPosition);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void HandleTileHovered(Tile tile)
    {
        if (!_bPlayerMoving && !tile.BSolid && tile != _selectedTile)
        {
            _gameState = GameState.PlayerSelectTileMove;
            _selectedTile = tile;
        }
    }

    public void HandleTileClicked(Tile tile)
    {
        if (!tile.BSolid && tile == _selectedTile)
        {
            _bPlayerMoving = true;
            _gameState = GameState.PlayerMoving;
        }
    }
}
