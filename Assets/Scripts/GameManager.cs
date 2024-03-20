using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        PlayerTurn,
        EnemiesTurns,
        Victory,
        Lose
    }

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    private GameState _state;
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

}


