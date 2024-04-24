using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    MatchStart,
    PlayerTurn,
    EnemiesTurn,
    MatchEnd,
    PlayerMoving,
    PlayerCastingSpell,
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
    FinishGame,
    PlayerStartsCastingSpell,
    PlayerStopsCastingSpell
}

public class StateMachine : MonoBehaviour
{
    private static StateMachine _instance;
    public static StateMachine Instance { get => _instance; }

    private State _currentState;
    public State CurrectState { get => _currentState; }
    private State _oldState; //DEBUG

    private Player player;
    private List<Enemy> enemiesList;

    private Tile _selectedTile;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _currentState = State.MatchStart;
            _oldState = State.MatchStart;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = Utils.GetPlayer();
        enemiesList = Utils.GetEnemies();
        GridManager.Instance.AddAsObserverToAllTiles(HandleTileHovered, HandleTileClicked);
        ProcessEvent();
    }


    private void Update()
    {
        //debug
        if (_currentState != _oldState) { print("current state changed to: " + _currentState); }
        _oldState = _currentState;
        //------------------------------
    }

    public void ProcessEvent(Event event_ = Event.StartsMatch)
    {
        switch (_currentState)
        {
            case State.MatchStart:
                {
                    _currentState = State.PlayerTurn;
                    StartPlayerTurn();
                }
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
                else if (event_ == Event.PlayerStartsCastingSpell)
                {
                    _currentState = State.PlayerCastingSpell;
                    StartPlayerCastingSpell();
                }
                break;
            case State.PlayerMoving:
                if (event_ == Event.PlayerStopsMoving)
                {
                    _currentState = State.PlayerTurn;
                    ResumePlayerTurn();
                }
                break;
            case State.PlayerCastingSpell:
                if (event_ == Event.PlayerStopsCastingSpell) //?
                {
                    //TODO Event.PlayerStopsCastingSpell
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
        //PathfindingManager.Instance.ClearValues();
        player.RestartStats();
        player.GetCharacterTile().Solid = false;
        InitEnemiesSolidTiles();

    }

    private void ResumePlayerTurn()
    {

    }

    private void StartPlayerMoving()
    {
        StartCoroutine(PlayerMovingCoroutine());
    }

    private void StartPlayerCastingSpell()
    {
        //throw new NotImplementedException();
    }

    private void StartEnemiesTurn()
    {
        //player.GetCharacterTile().Solid = true;
        StartCoroutine(AllEnemiesTurnsCoroutine());
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
            enemy.RestartStats();
            enemy.GetCharacterTile().Solid = false;
            yield return StartCoroutine(enemy.EnemyTurnCoroutine(player));
            enemy.GetCharacterTile().Solid = true;
        }
        ProcessEvent(Event.FinishEnemiesTurn);
    }

    // OTHER ------------------------------------------------

    private void InitEnemiesSolidTiles()
    {
        foreach (Enemy enemy in enemiesList)
        {
            enemy.GetCharacterTile().Solid = true;
        }
    }

    // EVENTS --------------------------------

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

    // SPELLS -------------------------------
    /*
    public void HandleSpellButtonClicked(Spell spell)
    {
        print(spell.name);
    }*/








}