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

    //private PathfindingManager _path;
    //private Player _player;
    //private Tile _selectedTile;

    //private bool _bPlayerMoving;

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
        //_path = PathfindingManager.Instance;
        //_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {

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
