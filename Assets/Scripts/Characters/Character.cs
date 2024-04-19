using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Init Stats")]
    [SerializeField] protected int _initHealthPoints = 300;       //health points
    [SerializeField] protected int _initActionPoints = 6;         //action points
    [SerializeField] protected int _initMovementPoints = 4;       //movement points
    [SerializeField] protected int _initDamage = 1;               //damage
    [SerializeField] protected int _initResistancePerc = 1;       //resistance %
    [SerializeField] protected int _initCritsPerc = 1;            //crits %

    [Header("Character Current Stats")]
    [SerializeField] protected int _healthPoints;
    [SerializeField] protected int _actionPoints;
    [SerializeField] protected int _movementPoints;
    [SerializeField] protected int _damage;
    [SerializeField] protected int _resistancePerc;
    [SerializeField] protected int _critsPerc;

    [Header("Character Spells")]
    [SerializeField] private List<Spell> _listSpells;

    [Header("Other")]
    [SerializeField] protected float _animationSpeed = 0.3f;

    public int HealthPoints { get => _healthPoints; }
    public int ActionPoints { get => _actionPoints; }
    public int MovementPoints { get => _movementPoints; }
    public int Damage { get => _damage; }
    public int ResistancePerc { get => _resistancePerc; }
    public int CritsPerc { get => _critsPerc; }
    public List<Spell> ListSpells { get => _listSpells; }

    private void Awake()
    {
        InitStats();
    }

    protected void InitStats()
    {
        _healthPoints = _initHealthPoints;
        _actionPoints = _initActionPoints;
        _movementPoints = _initMovementPoints;
        _damage = _initDamage;
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
        if (_movementPoints > 0) _healthPoints -= hp;
        else _movementPoints = 0;
    }

    public void SpendActionPoints(int ap = 1)
    {

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

    public Tile MeleeTile(Vector2Int dir)
    {
        var grid = GridManager.Instance;

        Tile tile = grid.GetTileFromTileCoords(GetCharacterTile().Coords + dir);
        return tile;
    }
}
