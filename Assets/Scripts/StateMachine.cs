using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    MatchStart,
    PlayerTurn,
    EnemiesTurn,
    Win,
    Lose,
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
    PlayerStopsMoving
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
            Vector3 enemyPos = enemy.transform.position;
            Tile enemyTile = GridManager.Instance.GetTileFromWorldCoords(enemyPos);
            enemyTile.SetAsSolid();
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
                if (event_ == Event.AllEnemiesDie) _currentState = State.Win;
                else if (event_ == Event.FinishPlayerTurn)
                {
                    _currentState = State.EnemiesTurn;
                    StartEnemiesTurn();
                }
                else if (event_ == Event.PlayerStartsMoving)
                {
                    _currentState = State.PlayerMoving;
                    StartPlayerMoving();//TODO playermoving
                }
                break;
            case State.PlayerMoving:
                if (event_ == Event.PlayerStopsMoving)
                {
                    _currentState = State.PlayerTurn;
                }
                break;
            case State.EnemiesTurn:
                if (event_ == Event.PlayerDies) _currentState = State.Lose;
                else if (event_ == Event.FinishEnemiesTurn)
                {
                    _currentState = State.PlayerTurn;
                }
                break;
            case State.Win:
                //TODO
                _currentState = State.MatchEnd;
                break;
            case State.Lose:
                //TODO
                _currentState = State.MatchEnd;
                break;
            case State.MatchEnd:
                //TODO volver al menu de niveles
                break;
        }
    }

    private void StartEnemiesTurn()
    {
        StartCoroutine(MockEnemyTurn());
    }

    private IEnumerator MockEnemyTurn()
    {
        foreach (Enemy enemy in enemiesList)
        {

        }
        print("empieza corrutina enemy turn");
        yield return StartCoroutine(DebugMovingThroughPath());
        print("termina corrutina enemy turn");
        ProcessEvent(Event.FinishEnemiesTurn);
    }

    public IEnumerator DebugMovingThroughPath()
    {
        print("empieza corrutina enemy moving");
        float currentTime = 0;

        Tile[,] tileGrid = GridManager.Instance.TileGrid;
        Tile tile = tileGrid[1, 1];

        Vector3 startPos = enemiesList[0].transform.position;

        Vector3 tilePos = tile.transform.position;
        Vector3 endPos = new Vector3(tilePos.x, tilePos.y, tile.Coords.y);

        print($"{startPos} {tilePos} {endPos}");

        while (currentTime < 0.3f)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / 0.3f;
            /*
            transform.position = new Vector3(
                Mathf.Lerp(startPos.x, endPos.x, t),
                Mathf.Lerp(startPos.y, endPos.x, t),
                tile.Coords.y //get layer
            );*/
            enemiesList[0].transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        enemiesList[0].transform.position = new Vector3(
            endPos.x,
            endPos.y,
            tile.Coords.y
        );
        //transform.position = tile.transform.position;
        currentTime = 0;
        tile.UncolorPathTile();

        print("termina corrutina enemy moving");
    }

    private void HandleTileHovered(Tile tile)
    {
        if (CurrectState == State.PlayerTurn
        && !tile.Solid
        && player.EnoughMovementPoints(tile))
        {
            _selectedTile = tile;
            player.OnPlayerSelectTileMove(_selectedTile);
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

    private void StartPlayerMoving()
    {
        StartCoroutine(PlayerMovingCoroutine());
    }

    private IEnumerator PlayerMovingCoroutine()
    {
        yield return StartCoroutine(player.MovingThroughPath());
        _selectedTile = null;
        ProcessEvent(Event.PlayerStopsMoving);
    }

    private void StartPlayerTurn()
    {

    }

}