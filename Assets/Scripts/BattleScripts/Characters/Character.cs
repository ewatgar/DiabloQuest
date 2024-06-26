using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    [Header("Character State")]
    [SerializeField] protected bool _isDead = false;

    [Header("Character Init Stats")]
    [SerializeField] protected int _initHealthPoints = 6;       //health points
    [SerializeField] protected int _initActionPoints = 6;         //action points
    [SerializeField] protected int _initMovementPoints = 4;       //movement points
    [SerializeField] protected int _initDamagePoints = 1;               //damage
    [SerializeField] protected int _initResistancePerc = 1;       //resistance %
    [SerializeField] protected int _initCritsPerc = 1;            //crits %

    [Header("Character Current Stats")]
    [SerializeField] protected int _health;
    [SerializeField] protected int _actionPoints;
    [SerializeField] protected int _movementPoints;
    [SerializeField] protected int _damagePoints;
    [SerializeField] protected int _resistancePerc;
    [SerializeField] protected int _critsPerc;

    [Header("Character Spells")]
    [SerializeField] protected List<Spell> _listSpells;

    protected Tile _oldGoalTile;
    protected Animator _animator;
    protected Vector2 _lastDirection;

    public event Action<Character> OnCharClicked;

    public bool IsDead { get => _isDead; set => _isDead = value; }
    public int InitHealthPoints { get => _initHealthPoints; }
    public int InitActionPoints { get => _initActionPoints; }
    public int InitMovementPoints { get => _initMovementPoints; }
    public int InitDamagePoints { get => _initDamagePoints; }
    public int InitResistancePerc { get => _initResistancePerc; }
    public int InitCritsPerc { get => _initCritsPerc; }
    public int Health { get => _health; }
    public int ActionPoints { get => _actionPoints; }
    public int MovementPoints { get => _movementPoints; }
    public int DamagePoints { get => _damagePoints; }
    public int ResistancePerc { get => _resistancePerc; }
    public int CritsPerc { get => _critsPerc; }
    public List<Spell> ListSpells { get => _listSpells; }

    private void Awake()
    {
        InitStats();
        _animator = GetComponent<Animator>();
        _lastDirection = Vector2.down;
    }

    private void Start()
    {
        _oldGoalTile = GetCharacterTile();
    }

    protected void InitStats()
    {
        _health = _initHealthPoints * 50;
        _actionPoints = _initActionPoints;
        _movementPoints = _initMovementPoints;
        _damagePoints = _initDamagePoints;
        _resistancePerc = _initResistancePerc;
        _critsPerc = _initCritsPerc;
        _isDead = false;
    }

    public void RestartStats()
    {
        _actionPoints = _initActionPoints;
        _movementPoints = _initMovementPoints;
    }


    public void TakeDamage(int hp = 1)
    {
        if (_health >= hp) _health -= hp;
        else _health = 0;
    }

    public void GetHealth(int hp = 1)
    {
        if (_health + hp <= _initHealthPoints * 50) _health += hp;
        else _health = _initHealthPoints * 50;
    }

    public void SpendActionPoints(int ap = 1)
    {
        if (_actionPoints >= ap) _actionPoints -= ap;
    }

    public void SpendMovementPoint()
    {
        SoundManager.Instance.PlayWalkSFX();
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

    public IEnumerator MovingThroughPathCoroutine(List<Tile> path, float animationSpeed = 0.3f, bool isKnockback = false)
    {
        Vector2 direction = _lastDirection;
        float currentTime = 0;
        foreach (Tile tile in path)
        {
            if (!isKnockback && _movementPoints <= 0) break;

            Vector3 startPos = transform.position;

            Vector3 tilePos = tile.transform.position;
            Vector3 endPos = new(tilePos.x, tilePos.y, tile.Coords.y);

            // ANIMATION --------------------------------------------
            if (!isKnockback)
            {
                direction = tilePos - startPos;
                direction = direction.normalized;

                AnimationManager.PlayAnimation(_animator, AnimationType.Walk, direction);
                _lastDirection = direction;
            }
            // --------------------------------------------

            while (currentTime < animationSpeed)
            {
                currentTime += Time.deltaTime;
                float t = currentTime / animationSpeed;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }

            transform.position = new Vector3(
                endPos.x,
                endPos.y,
                tile.Coords.y
            );
            currentTime = 0;
            if (!isKnockback) SpendMovementPoint();
            tile.Path = false;
        }
        AnimationManager.PlayAnimation(_animator, AnimationType.Idle, direction);
    }

    public Tile GetCharacterTile()
    {
        return GridManager.Instance.GetTileFromWorldCoords(transform.position);
    }

    protected void TakeFinalSpellDamage(Character characterAttacked, Spell spell)
    {
        bool isCrits = false;
        int baseCharDamage = _damagePoints * 10;
        int baseSpellDamage = spell.value;
        int critsPerc = _critsPerc * 10;
        int resPerc = _resistancePerc * 10;

        // critsDamage = [spellDamage * 1,5] or [spellDamage + half]
        int critsDamage = 0;
        if (Random.Range(1, 101) <= critsPerc)
        {
            isCrits = true;
            print("isCrits");
            critsDamage = Mathf.FloorToInt(baseSpellDamage * 1.5f);
        }

        int damageWithCrits = baseCharDamage + baseSpellDamage + critsDamage;
        int resDamage = damageWithCrits * (resPerc / 100);

        int finalDamage = damageWithCrits - resDamage;
        SoundManager.Instance.PlayHurtSFX(isCrits);
        characterAttacked.TakeDamage(finalDamage);
    }

    protected IEnumerator KnockbackCharCoroutine(Character characterPushed, int distance)
    {
        GridManager grid = GridManager.Instance;
        Tile charPushingTile = GetCharacterTile();
        Tile charPushedTile = characterPushed.GetCharacterTile();
        Vector2Int knockbackDir = GridManager.Instance.GetMeleeDirFromTwoTiles(charPushingTile, charPushedTile);
        List<Tile> path = new List<Tile>();

        Vector2Int sum = charPushedTile.Coords;
        for (int i = 1; i <= distance; i++)
        {
            sum += knockbackDir;
            Tile knockbackTile = grid.GetTileFromTileCoords(sum);
            if (knockbackTile == null || knockbackTile.Solid) break;
            path.Add(knockbackTile);
        }
        yield return StartCoroutine(characterPushed.MovingThroughPathCoroutine(path, 0.15f, true));

    }

    protected void OnMouseDown()
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

    public void UpdateStatsFromSave()
    {
        GameData gameData = SaveManager.Load();
        if (gameData != null)
        {
            name = gameData.playerName;
            _initHealthPoints = gameData.healthPoints;
            _initActionPoints = gameData.actionPoints;
            _initMovementPoints = gameData.movementPoints;
            _initDamagePoints = gameData.damagePoints;
            _initResistancePerc = gameData.resistancePerc;
            _initCritsPerc = gameData.critsPerc;
            _health = _initHealthPoints * 50;
        }
        InitStats();
    }

    public void EnableCollider(bool value)
    {
        GetComponent<Collider2D>().enabled = value;
    }

    public IEnumerator PlayCastSpellAnimationCoroutine(Tile selectedTile, Spell spell, bool isPlayer = true)
    {
        Vector2 direction;
        if (spell.spellAreaType == SpellAreaType.Donut || spell.spellAreaType == SpellAreaType.Self) direction = _lastDirection;
        else direction = selectedTile.Coords - GetCharacterTile().Coords;
        direction = direction.normalized;

        Debug.Log("direction spell: " + direction);
        AnimationManager.PlayAnimation(_animator, spell.animationType, direction);

        float waitTime = 1;
        if (isPlayer) waitTime = 0.75f;
        yield return new WaitForSeconds(waitTime);

        AnimationManager.PlayAnimation(_animator, AnimationType.Idle, direction);
        _lastDirection = direction;
    }

    public IEnumerator PlayHurtAnimationCoroutine()
    {
        AnimationManager.PlayAnimation(_animator, AnimationType.Hurt, _lastDirection);
        yield return new WaitForSeconds(0.5f);
        AnimationManager.PlayAnimation(_animator, AnimationType.Idle, _lastDirection);
    }

    public IEnumerator PlayDeathAnimationCoroutine()
    {
        AnimationManager.PlayAnimation(_animator, AnimationType.Death, _lastDirection);
        yield return new WaitForSeconds(0.75f);
        gameObject.SetActive(false);
    }
}
