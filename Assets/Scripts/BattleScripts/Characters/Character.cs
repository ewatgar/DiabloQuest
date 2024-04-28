using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    [Header("Character Init Stats")]
    [SerializeField] protected int _initHealthPoints = 300;       //health points
    [SerializeField] protected int _initActionPoints = 6;         //action points
    [SerializeField] protected int _initMovementPoints = 4;       //movement points
    [SerializeField] protected int _initDamagePoints = 1;               //damage
    [SerializeField] protected int _initResistancePerc = 1;       //resistance %
    [SerializeField] protected int _initCritsPerc = 1;            //crits %

    [Header("Character Current Stats")]
    [SerializeField] protected int _healthPoints;
    [SerializeField] protected int _actionPoints;
    [SerializeField] protected int _movementPoints;
    [SerializeField] protected int _damagePoints;
    [SerializeField] protected int _resistancePerc;
    [SerializeField] protected int _critsPerc;

    [Header("Character Spells")]
    [SerializeField] protected List<Spell> _listSpells;

    [Header("Other")]
    [SerializeField] protected float _animationSpeed = 0.3f;

    protected Tile _oldGoalTile;

    public event Action<Character> OnCharClicked;

    public int HealthPoints { get => _healthPoints; }
    public int ActionPoints { get => _actionPoints; }
    public int MovementPoints { get => _movementPoints; }
    public int DamagePoints { get => _damagePoints; }
    public int ResistancePerc { get => _resistancePerc; }
    public int CritsPerc { get => _critsPerc; }
    public List<Spell> ListSpells { get => _listSpells; }

    private void Awake()
    {
        InitStats();
    }

    private void Start()
    {
        _oldGoalTile = GetCharacterTile();
    }

    protected void InitStats()
    {
        _healthPoints = _initHealthPoints;
        _actionPoints = _initActionPoints;
        _movementPoints = _initMovementPoints;
        _damagePoints = _initDamagePoints;
        _resistancePerc = _initResistancePerc;
        _critsPerc = _initCritsPerc;
    }

    public void RestartStats()
    {
        _actionPoints = _initActionPoints;
        _movementPoints = _initMovementPoints;
    }


    public void TakeDamage(int hp = 1)
    {
        if (_healthPoints > 0) _healthPoints -= hp;
        else _healthPoints = 0;
    }

    public void GetHealth(int hp = 1)
    {
        if (_healthPoints < _initActionPoints) _healthPoints += hp;
        else _healthPoints = _initActionPoints;
    }

    public void SpendActionPoints(int ap = 1)
    {
        if (_actionPoints > 0) _actionPoints -= ap;
        else _actionPoints = 0;
    }

    public void SpendMovementPoint()
    {
        if (_movementPoints > 0) _movementPoints--;
        else _movementPoints = 0;
    }

    public bool EnoughMovementPoints(Tile tile, bool color)
    {
        PathfindingManager.Instance.RegeneratePath(GetCharacterTile(), tile, color);
        int tilePathCount = PathfindingManager.Instance.FinalPath.Count;
        return tilePathCount <= _movementPoints;
    }

    public void SelectTileForPathfinding(Tile selectedTile, bool color)
    {
        PathfindingManager.Instance.RegeneratePath(GetCharacterTile(), selectedTile, color);
    }

    public IEnumerator MovingThroughPathCoroutine()
    {
        //print("empieza corrutina character moving");
        float currentTime = 0;

        List<Tile> path = PathfindingManager.Instance.FinalPath;

        foreach (Tile tile in path)
        {
            Vector3 startPos = transform.position;

            Vector3 tilePos = tile.transform.position;
            Vector3 endPos = new Vector3(tilePos.x, tilePos.y, tile.Coords.y);

            while (currentTime < _animationSpeed)
            {
                currentTime += Time.deltaTime;
                float t = currentTime / _animationSpeed;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }

            transform.position = new Vector3(
                endPos.x,
                endPos.y,
                tile.Coords.y
            );
            currentTime = 0;
            SpendMovementPoint();
            tile.Path = false;
        }
        //print("termina corrutina character moving");
    }

    public Tile GetCharacterTile()
    {
        return GridManager.Instance.GetTileFromWorldCoords(transform.position);
    }

    protected void TakeFinalSpellDamage(Character characterAttacked, Spell spell)
    {
        int baseCharDamage = _damagePoints * 10;
        int baseSpellDamage = spell.baseDamageOrHealing;
        int critsPerc = _critsPerc * 10;
        int resPerc = _resistancePerc * 10;

        // critsDamage = [spellDamage * 1,5] or [spellDamage + half]
        int critsDamage = 0;
        if (Random.Range(1, 101) >= critsPerc) critsDamage = Mathf.FloorToInt(baseSpellDamage * 1.5f);

        int damageWithCrits = baseCharDamage + baseSpellDamage + critsDamage;
        int resDamage = damageWithCrits * (resPerc / 100);

        int finalDamage = damageWithCrits - resDamage;
        characterAttacked.TakeDamage(finalDamage);
    }

    private void OnMouseDown()
    {
        OnCharClicked?.Invoke(this);
    }

    public void InitCharZPosition()
    {
        Vector3 newPos = new Vector3(
            transform.position.x,
            transform.position.y,
            GetCharacterTile().Coords.y);
        transform.position = newPos;
    }

    public void EnableCollider(bool value)
    {
        GetComponent<Collider2D>().enabled = value;
    }
}
