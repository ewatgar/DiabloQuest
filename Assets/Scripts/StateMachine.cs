using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    [SerializeField] List<Enemy> EnemiesList;

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

    private void Update()
    {
        if (_currentState != _oldState) { print("current state changed to: " + _currentState); }
        _oldState = _currentState;
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
                    //StartMovingCoroutine()//TODO playermoving
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
        print("empieza corrutina enemy turn");
        yield return new WaitForSeconds(3);
        print("termina corrutina enemy turn");
        ProcessEvent(Event.FinishEnemiesTurn);
    }

}