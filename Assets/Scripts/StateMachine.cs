using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    MatchStart,
    PlayerTurn,
    EnemiesTurn,
    MatchEnd,
    PlayerMoving,
    //PlayerCastingSpell,
}

public enum Event
{
    StartsMatch,
    FinishPlayerTurn,
    FinishEnemiesTurn,
    PlayerDies,
    AllEnemiesDie,
    PlayerStartsMoving,
    PlayerStopsMoving,
    FinishGame
}

public class StateMachine : MonoBehaviour
{
    private static StateMachine _instance;
    public static StateMachine Instance { get => _instance; }

    private State _currentState;
    public State CurrectState { get => _currentState; }
    private State _oldState; //DEBUG

    [SerializeField] Player player;
    [SerializeField] List<Enemy> enemiesList;

    private Tile _selectedTile;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _currentState = State.MatchStart;
            _oldState = State.MatchStart;
            ProcessEvent();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GridManager.Instance.AddAsObserverToAllTiles(HandleTileHovered, HandleTileClicked);
    }

    private void Update()
    {
        //debug
        if (_currentState != _oldState) { print("current state changed to: " + _currentState); }
        _oldState = _currentState;
        //------------------------------

        UpdateSolidTiles();
    }

    private void UpdateSolidTiles()
    {
        GridManager.Instance.ClearSolidTiles();
        foreach (Enemy enemy in enemiesList)
        {
            enemy.GetCharacterTile().Solid = true;
        }

    }

    public void ProcessEvent(Event event_ = Event.StartsMatch)
    {
        switch (_currentState)
        {
            case State.MatchStart:
                _currentState = State.PlayerTurn;
                break;
            case State.PlayerTurn:
                if (event_ == Event.AllEnemiesDie)
                {
                    _currentState = State.MatchEnd;
                    //StartMatchEnd(true);
                }
                else if (event_ == Event.FinishPlayerTurn)
                {
                    _currentState = State.EnemiesTurn;
                    StartEnemiesTurn();
                }
                else if (event_ == Event.PlayerStartsMoving)
                {
                    _currentState = State.PlayerMoving;
                    StartPlayerMoving();
                }
                break;
            case State.PlayerMoving:
                if (event_ == Event.PlayerStopsMoving)
                {
                    _currentState = State.PlayerTurn;
                    ResumePlayerTurn();
                }
                break;
            case State.EnemiesTurn:
                if (event_ == Event.PlayerDies)
                {
                    _currentState = State.MatchEnd;
                    //StartMatchEnd(false);
                }
                else if (event_ == Event.FinishEnemiesTurn)
                {
                    _currentState = State.PlayerTurn;
                    StartPlayerTurn();
                }
                break;
            case State.MatchEnd:
                if (event_ == Event.FinishGame)
                {
                    //TODO terminar pelea
                }
                break;
        }
    }


    // START METHODS FOR STATE MACHINE -------------------

    private void StartPlayerTurn()
    {
        player.RestartStats();
    }

    private void ResumePlayerTurn()
    {

    }

    private void StartPlayerMoving()
    {
        StartCoroutine(PlayerMovingCoroutine());
    }

    private void StartEnemiesTurn()
    {
        StartCoroutine(AllEnemiesTurnsCoroutine());
    }

    // HANDLE TILE EVENTS --------------------------------

    private void HandleTileHovered(Tile tile)
    {
        if (CurrectState == State.PlayerTurn
        && !tile.Solid
        && player.EnoughMovementPoints(tile, false))
        {
            _selectedTile = tile;
            player.SelectTileForPathfinding(_selectedTile, true);
        }
    }

    private void HandleTileClicked(Tile tile)
    {
        if (CurrectState == State.PlayerTurn
        && !tile.Solid
        && tile == _selectedTile)
        {
            ProcessEvent(Event.PlayerStartsMoving);
        }
    }

    // COROUTINES ----------------------------------------

    private IEnumerator PlayerMovingCoroutine()
    {
        yield return StartCoroutine(player.MovingThroughPathCoroutine());
        _selectedTile = null;
        ProcessEvent(Event.PlayerStopsMoving);
    }

    private IEnumerator AllEnemiesTurnsCoroutine()
    {
        foreach (Enemy enemy in enemiesList)
        {
            print("--- turno de enemigo entero ---");
            enemy.RestartStats();
            yield return StartCoroutine(EnemyTurnCoroutine(enemy));
        }
        ProcessEvent(Event.FinishEnemiesTurn);
    }


    // turno enemigo tiene dos partes:
    // 1. Movimiento (acercarse o alejarse del player)
    // 2. Atacar (melee o distancia)
    private IEnumerator EnemyTurnCoroutine(Enemy enemy)
    {
        //PASO 1 - moverse si puede -------------------------------------------
        Tile mockTile = player.MeleeTile(Vector2Int.left);
        enemy.SelectTileForPathfinding(mockTile, true);
        yield return StartCoroutine(enemy.MovingThroughPathCoroutine());

        yield return new WaitForSeconds(3);

        //PASO 2 - atacar si puede --------------------------------------------
        if (enemy.GetCharacterTile() == mockTile) print("enemigo te ataca");
    }









}