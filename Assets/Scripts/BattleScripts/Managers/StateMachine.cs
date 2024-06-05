using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum State
{
    MatchStart,
    PlayerTurn,
    EnemiesTurn,
    MatchEnd,
    PlayerMoving,
    PlayerSelectingSpell,
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
    PlayerStartsSelectingSpell,
    PlayerStopsSelectingSpell,
    PlayerStartsCastingSpell,
    PlayerStopsCastingSpell,
    TileHovered,
    TileClicked,
    FinishTurnButtonClicked,
    SpellButtonClicked
}

public class StateMachine : MonoBehaviour
{
    private static StateMachine _instance;
    public static StateMachine Instance { get => _instance; }

    private int _numRounds;
    public int NumRounds { get => _numRounds; }

    private State _currentState;
    public State CurrectState { get => _currentState; }

    private State _oldState; //DEBUG

    private Player _player;
    private List<Enemy> _enemiesList;

    private Tile _hoveredTile;
    private Tile _selectedTile;
    private List<Tile> _listSelectableTiles;
    private List<Tile> _listAreaOfEffectTiles;
    private Spell _selectedSpell;
    private Spell _previousSelectedSpell;

    private HashSet<Item> _listOfUsedItems = new HashSet<Item>();
    public HashSet<Item> ListOfUsedItems { get => _listOfUsedItems; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            InitStateMachine();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitStateMachine()
    {
        _numRounds = 0;
        _currentState = State.MatchStart;
        _oldState = State.MatchStart;
        _hoveredTile = null;
        _selectedTile = null;
        _selectedSpell = null;
        _previousSelectedSpell = null;
    }

    private void Start()
    {
        InitPlayer();
        InitEnemy();
        GridManager.Instance.AddAsObserverToAllTiles(HandleTileHovered, HandleTileClicked);
        BattleUIManager.Instance.AddAsObserverUI(HandleFinishTurnButtonClicked, HandleSpellButtonClicked, HandleAcceptCharPointsButtonClicked);
        ProcessEvent(Event.StartsMatch);
    }

    private void InitPlayer()
    {
        //TODO load current level
        _player = Utils.GetPlayer();
        _player.InitCharZPosition();
        _player.UpdateStatsFromSave();
    }

    private void InitEnemy()
    {
        _enemiesList = Utils.GetEnemies();
        foreach (Enemy enemy in _enemiesList)
        {
            enemy.InitCharZPosition();
        }
    }

    private void Update()
    {
        //debug
        if (_currentState != _oldState)
        {
            print($"----- STATE CHANGED -> {_currentState}");
        }
        _oldState = _currentState;
        //------------------------------
    }

    public void ProcessEvent(Event event_)
    {
        switch (_currentState)
        {
            case State.MatchStart:
                _currentState = State.PlayerTurn;
                StartPlayerTurn();
                break;
            case State.PlayerTurn:
                switch (event_)
                {
                    case Event.AllEnemiesDie:
                        _currentState = State.MatchEnd;
                        StartMatchEnd(true);
                        break;
                    case Event.FinishPlayerTurn:
                        _currentState = State.EnemiesTurn;
                        StartEnemiesTurn();
                        break;
                    case Event.PlayerStartsMoving:
                        _currentState = State.PlayerMoving;
                        StartPlayerMoving();
                        break;
                    case Event.PlayerStartsSelectingSpell:
                        _currentState = State.PlayerSelectingSpell;
                        StartPlayerSelectingSpell();
                        break;
                    case Event.TileHovered:
                        CheckHoveredTilePlayerTurn();
                        break;
                    case Event.TileClicked:
                        CheckClickedTilePlayerTurn();
                        break;
                    case Event.FinishTurnButtonClicked:
                        CheckFinishTurnButtonClicked();
                        break;
                    case Event.SpellButtonClicked:
                        CheckSpellButtonClickedPlayerTurn();
                        break;
                }
                break;
            case State.PlayerMoving:
                switch (event_)
                {
                    case Event.PlayerStopsMoving:
                        _currentState = State.PlayerTurn;
                        ResumePlayerTurn();
                        break;
                }
                break;
            case State.PlayerSelectingSpell:
                switch (event_)
                {
                    case Event.PlayerStartsCastingSpell:
                        _currentState = State.PlayerCastingSpell;
                        StartPlayerCastingSpell();
                        break;
                    case Event.PlayerStartsSelectingSpell:
                        StartPlayerSelectingSpell(); // selects another spell
                        break;
                    case Event.PlayerStopsSelectingSpell:
                        _currentState = State.PlayerTurn;
                        ResumePlayerTurn();
                        break;
                    case Event.TileHovered:
                        CheckHoveredTilePlayerSelectingSpell();
                        break;
                    case Event.TileClicked:
                        CheckClickedTilePlayerSelectingSpell();
                        break;
                    case Event.SpellButtonClicked:
                        CheckSpellButtonClickedPlayerSelectingSpell();
                        break;
                }
                break;
            case State.PlayerCastingSpell:
                switch (event_)
                {
                    case Event.PlayerStopsCastingSpell:
                        _currentState = State.PlayerTurn;
                        ResumePlayerTurn();
                        break;
                }
                break;
            case State.EnemiesTurn:
                switch (event_)
                {
                    case Event.PlayerDies:
                        _currentState = State.MatchEnd;
                        StartMatchEnd(false);
                        break;
                    case Event.FinishEnemiesTurn:
                        _numRounds++;
                        _currentState = State.PlayerTurn;
                        StartPlayerTurn();
                        break;
                }
                break;
            case State.MatchEnd:
                switch (event_)
                {
                    case Event.FinishGame:
                        FinishGame();
                        break;
                }
                break;
        }
    }

    // STATE MACHINE METHODS -------------------

    private void StartMatchEnd(bool value)
    {

        if (value)
        {
            SoundManager.Instance.PlayWinSFX();
            //print("Has ganado, se distribuye puntos de características y se guarda la partida");
            BattleUIManager.Instance.ShowEndMatchUI(true);
            //Se espera a que se clicke el boton Aceptar
        }
        else
        {
            //print("Has perdido");
            StartCoroutine(LoseMatchCoroutine());
        }
    }

    private void FinishGame()
    {
        SceneManager.LoadScene("LevelSelectionScene");
    }

    private void CheckHoveredTilePlayerTurn()
    {
        if (!_hoveredTile.Solid && _player.EnoughMovementPoints(_hoveredTile, false))
        {
            _player.SelectTileForPathfinding(_hoveredTile, true);
        }
    }

    private void CheckHoveredTilePlayerSelectingSpell()
    {
        if (_player.GetSelectableSpellTiles(_selectedSpell).Contains(_hoveredTile))
        {
            _listAreaOfEffectTiles = _player.GetAreaOfEffectTiles(_selectedSpell, _hoveredTile);
        }
    }

    private void CheckClickedTilePlayerTurn()
    {
        if (!_selectedTile.Solid && _player.EnoughMovementPoints(_hoveredTile, false) && _selectedTile == _hoveredTile)
        {
            ProcessEvent(Event.PlayerStartsMoving);
        }
    }

    private void CheckClickedTilePlayerSelectingSpell()
    {
        if (_selectedTile == _hoveredTile && _selectedTile.SpellSelectable)
        {
            ProcessEvent(Event.PlayerStartsCastingSpell);
        }
    }

    private void CheckFinishTurnButtonClicked()
    {
        ProcessEvent(Event.FinishPlayerTurn);
    }

    private void CheckSpellButtonClickedPlayerTurn()
    {
        ProcessEvent(Event.PlayerStartsSelectingSpell);
    }

    private void CheckSpellButtonClickedPlayerSelectingSpell()
    {
        if (_previousSelectedSpell == _selectedSpell) ProcessEvent(Event.PlayerStopsSelectingSpell);
        else ProcessEvent(Event.PlayerStartsSelectingSpell);
    }

    private void StartPlayerTurn()
    {
        if (CheckAllEnemiesDead()) ProcessEvent(Event.AllEnemiesDie);
        else
        {
            //PathfindingManager.Instance.ClearValues();
            _player.RestartStats();
            _player.GetCharacterTile().Solid = false;
            InitEnemiesSolidTiles();
        }
    }

    private void ResumePlayerTurn()
    {
        if (CheckAllEnemiesDead()) ProcessEvent(Event.AllEnemiesDie);
        else
        {
            ClearTiles();
            _hoveredTile = null;
            _selectedTile = null;
            _selectedSpell = null;
            _listSelectableTiles = null;
            _listAreaOfEffectTiles = null;
            //EnableAllColliders(true);
        }
    }

    private void StartPlayerMoving()
    {
        StartCoroutine(PlayerMovingCoroutine());
    }

    private void StartPlayerSelectingSpell()
    {
        //EnableAllColliders(false);
        _listSelectableTiles = _player.GetSelectableSpellTiles(_selectedSpell);
        //_player.GetSpellTiles(_selectedSpell);
    }

    private void StartPlayerCastingSpell()
    {
        if (_selectedSpell.spellAreaType == SpellAreaType.Self) StartCoroutine(PlayerSelfCoroutine());
        else StartCoroutine(PlayerAttackCoroutine());
    }

    private void StartEnemiesTurn()
    {
        //player.GetCharacterTile().Solid = true;
        StartCoroutine(AllEnemiesTurnsCoroutine());
    }

    // COROUTINES ----------------------------------------

    private IEnumerator PlayerMovingCoroutine()
    {
        List<Tile> path = PathfindingManager.Instance.FinalPath;
        yield return StartCoroutine(_player.MovingThroughPathCoroutine(path));
        _hoveredTile = null;
        _selectedTile = null;
        ProcessEvent(Event.PlayerStopsMoving);
    }

    private IEnumerator PlayerSelfCoroutine()
    {
        ClearTiles();
        if (_player.ActionPoints >= _selectedSpell.actionPointCost)
        {
            //yield return StartCoroutine(_player.PlayCastSpellAnimationCoroutine(_player.LastDirection, _selectedSpell));
            yield return StartCoroutine(_player.ApplySpellEffectCoroutine(_selectedSpell));
        }
        _player.SpendActionPoints(_selectedSpell.actionPointCost);
        if (_selectedSpell is Item item)
        {
            _listOfUsedItems.Add(item);
            BattleUIManager.Instance.ReplaceSpellWithItemButtons(true);
            SoundManager.Instance.PlayPotionSFX();
        }
        ProcessEvent(Event.PlayerStopsCastingSpell);
    }

    private IEnumerator PlayerAttackCoroutine()
    {
        ClearTiles();
        if (_player.ActionPoints >= _selectedSpell.actionPointCost)
        {
            SoundManager.Instance.PlaySpellSFX(_selectedSpell);
            yield return StartCoroutine(_player.PlayCastSpellAnimationCoroutine(_selectedTile, _selectedSpell));

            foreach (Enemy enemy in _enemiesList)
            {
                if (!enemy.IsDead && _listAreaOfEffectTiles.Contains(enemy.GetCharacterTile()))
                {
                    SoundManager.Instance.PlayHurtSFX();
                    yield return StartCoroutine(_player.AttackEnemyCoroutine(enemy, _selectedSpell));
                    if (enemy.Health <= 0)
                    {
                        SoundManager.Instance.PlayDyingSFX();
                        enemy.IsDead = true;
                        enemy.GetCharacterTile().Solid = false;
                        yield return StartCoroutine(enemy.PlayDeathAnimationCoroutine());
                    }
                }
            }
            if (_selectedSpell.utilityType == UtilityType.Knockback) UpdateEnemiesSolidTiles();
        }
        _player.SpendActionPoints(_selectedSpell.actionPointCost);
        ProcessEvent(Event.PlayerStopsCastingSpell);
    }

    private IEnumerator LoseMatchCoroutine()
    {
        BattleUIManager.Instance.ShowEndMatchUI(false);
        SoundManager.Instance.PlayLoseSFX();
        yield return new WaitForSeconds(1);
        ProcessEvent(Event.FinishGame);
    }

    private IEnumerator AllEnemiesTurnsCoroutine()
    {
        foreach (Enemy enemy in _enemiesList)
        {
            if (!enemy.IsDead)
            {
                enemy.RestartStats();
                enemy.GetCharacterTile().Solid = false;
                yield return StartCoroutine(enemy.EnemyTurnCoroutine(_player));
                enemy.GetCharacterTile().Solid = true;
                if (_player.IsDead == true)
                {
                    SoundManager.Instance.PlayDyingSFX();
                    yield return StartCoroutine(_player.PlayDeathAnimationCoroutine());
                    ProcessEvent(Event.PlayerDies);
                    yield break;
                }
            }
        }
        if (!_player.IsDead) ProcessEvent(Event.FinishEnemiesTurn);
    }


    // EVENTS --------------------------------

    private void HandleTileHovered(Tile tile)
    {
        GridManager.Instance.ClearSpellTiles();
        _hoveredTile = tile;
        ProcessEvent(Event.TileHovered);
    }

    private void HandleTileClicked(Tile tile)
    {
        _selectedTile = tile;
        ProcessEvent(Event.TileClicked);
    }

    private void HandleFinishTurnButtonClicked()
    {
        ProcessEvent(Event.FinishTurnButtonClicked);
    }

    public void HandleSpellButtonClicked(Spell spell)
    {
        ClearTiles();
        _previousSelectedSpell = _selectedSpell;
        _selectedSpell = spell;
        ProcessEvent(Event.SpellButtonClicked);
    }

    public void HandleAcceptCharPointsButtonClicked(List<int> listCharPointsGD)
    {
        SaveProgress(listCharPointsGD);
        ProcessEvent(Event.FinishGame);
    }

    // OTHER ------------------------------------------------

    private void InitEnemiesSolidTiles()
    {
        foreach (Enemy enemy in _enemiesList)
        {
            if (!enemy.IsDead) enemy.GetCharacterTile().Solid = true;
        }
    }

    private void ClearTiles()
    {
        PathfindingManager.Instance.ClearValues();
        GridManager.Instance.ClearSpellTiles();
    }

    private void EnableAllColliders(bool value)
    {
        _player.EnableCollider(value);
        foreach (Enemy enemy in _enemiesList)
        {
            if (!enemy.IsDead) enemy.EnableCollider(value);
        }
    }

    private void UpdateEnemiesSolidTiles()
    {
        GridManager.Instance.ClearSolidTiles();
        InitEnemiesSolidTiles();
    }

    private bool CheckAllEnemiesDead()
    {
        foreach (Enemy enemy in _enemiesList)
        {
            if (!enemy.IsDead) return false;
        }
        return true;
    }

    private void SaveProgress(List<int> listCharPointsGD)
    {
        SaveManager.SaveProgress(
            listCharPointsGD[0],
            listCharPointsGD[1],
            listCharPointsGD[2],
            listCharPointsGD[3],
            listCharPointsGD[4],
            listCharPointsGD[5]
        );
    }
}