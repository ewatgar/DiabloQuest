// https://discussions.unity.com/t/run-coroutine-only-once/122924/2


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

    //private PathfindingManager _path;
    //private Player _player;
    //private Tile _selectedTile;

    //private bool _bPlayerMoving;

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
        //_path = PathfindingManager.Instance;
        //_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        switch (_gameState)
        {
            case GameState.PlayerIdle:
                OnPlayerIdle();
                break;
            case GameState.PlayerSelectTileMove:
                OnPlayerSelectTileMove();
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

    }

    private void OnPlayerSelectTileMove()
    {
    }

    private void OnPlayerMoving()
    {
    }
}
