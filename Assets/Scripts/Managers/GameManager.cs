using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        PlayerTurnMove,
        PlayerTurnSelectSpell,
        EnemiesTurn,
        Victory,
        Lose
    }

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    private GameState _state;
    private GameState State { get => _state; }

    //private List<Enemy>

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

    private void Update()
    {
        switch (_state)
        {
            case GameState.PlayerTurnMove:
                break;
            case GameState.PlayerTurnSelectSpell:
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


