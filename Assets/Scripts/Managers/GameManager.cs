using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PlayerWaiting,
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

    private GameState _state;
    public GameState State { get => _state; set => _state = value; }

    private Tile _selectedTile;
    private Player _player;
    //private List<Enemy>

    public Tile SelectedTile { get => _selectedTile; set => _selectedTile = value; }
    public Player Player { get => _player; }

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
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        switch (_state)
        {
            case GameState.PlayerWaiting:
                break;
            case GameState.PlayerSelectTileMove:
                break;
            case GameState.PlayerMoving:
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
}
