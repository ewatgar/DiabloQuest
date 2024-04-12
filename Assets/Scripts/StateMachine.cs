using System.Text;
using UnityEngine;

public enum State
{
    MatchStart,
    PlayerTurn,
    //PlayerMovingAnim,
    //PlayerSpellAnim,
    EnemiesTurn,
    Win,
    Lose,
    MatchEnd,
}

public enum Event
{
    FinishPlayerTurn,
    FinishEnemiesTurn,
    PlayerDies,
    AllEnemiesDie
}

public class StateMachine : MonoBehaviour
{
    private static StateMachine _instance;
    public static StateMachine Instance { get => _instance; }

    private State _currentState;
    public State CurrectState { get => _currentState; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _currentState = State.MatchStart;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ProcessEvent(Event event_)
    {
        switch (_currentState)
        {
            case State.MatchStart:
                _currentState = State.PlayerTurn;
                break;
            case State.PlayerTurn:
                if (event_ == Event.AllEnemiesDie) _currentState = State.Win;
                else if (event_ == Event.FinishPlayerTurn) _currentState = State.EnemiesTurn;
                break;
            case State.EnemiesTurn:
                if (event_ == Event.PlayerDies) _currentState = State.Lose;
                else if (event_ == Event.FinishEnemiesTurn) _currentState = State.EnemiesTurn;
                break;
            case State.Win:
                //TODO
                _currentState = State.MatchEnd;
                break;
            case State.Lose:
                //TOOD
                _currentState = State.MatchEnd;
                break;
            case State.MatchEnd:
                //TODO volver al menu de niveles
                break;
        }
    }

}