using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyType
{
    Melee,
    LongRange,
}

public class Enemy : Character
{
    [Header("Enemy Type")]
    [SerializeField] EnemyType _enemyType;
    [SerializeField] int _minDistanceLongRange = 5;

    public new bool SelectTileForPathfinding(Tile playerTile, bool color)
    {
        Tile goalTile = null;
        if (_enemyType == EnemyType.LongRange)
        {
            playerTile.Solid = true;
            goalTile = GetTileAwayFromPlayer(playerTile);
        }
        else
        {
            playerTile.Solid = false;
            goalTile = playerTile;
        }

        if (goalTile == null) return false;
        PathfindingManager.Instance.RegeneratePath(GetCharacterTile(), goalTile, color);
        return true;
    }

    private Tile GetTileAwayFromPlayer(Tile playerTile)
    {
        GridManager grid = GridManager.Instance;
        Tile[,] tileGrid = grid.TileGrid;

        int distanceCurrentTile = GridManager.DistanceBetweenTiles(GetCharacterTile(), playerTile);
        if (distanceCurrentTile >= _minDistanceLongRange) return null;

        int distanceOldTile = GridManager.DistanceBetweenTiles(_oldGoalTile, playerTile);
        if (distanceOldTile >= _minDistanceLongRange) return _oldGoalTile;

        Tile goalTile = null;
        int step = 0;
        while (goalTile == null && step < 100)
        {
            int randCol = Random.Range(0, grid.NCols);
            int randRow = Random.Range(0, grid.NRows);
            Tile randTile = tileGrid[randCol, randRow];
            int distanceRandTile = GridManager.DistanceBetweenTiles(randTile, playerTile);
            if (distanceRandTile >= _minDistanceLongRange) goalTile = randTile;
            step++;
        }
        _oldGoalTile = goalTile;
        return goalTile;
    }

    // turno enemigo tiene dos partes:
    // 1. Movimiento (acercarse o alejarse del player)
    // 2. Atacar (melee o distancia)
    public IEnumerator EnemyTurnCoroutine(Player player)
    {
        //PASO 1 - moverse si puede
        yield return StartCoroutine(EnemyMovementCoroutine(player));

        //PASO 2 - atacar si puede
        yield return StartCoroutine(EnemyAttackCoroutine(player));

    }

    private IEnumerator EnemyMovementCoroutine(Player player)
    {
        List<Tile> path = PathfindingManager.Instance.FinalPath;
        bool wantToMove = true;
        wantToMove = SelectTileForPathfinding(player.GetCharacterTile(), true);
        if (wantToMove) yield return StartCoroutine(MovingThroughPathCoroutine());
        yield return null;
    }

    private IEnumerator EnemyAttackCoroutine(Player player)
    {
        int step = 0;
        while (_actionPoints > 0 && step < 100)
        {
            int index = Random.Range(0, ListSpells.Count);
            Spell randomSpell = _listSpells[index];
            UseSpell(player, randomSpell);
            step++;
        }
        yield return null;

    }

    private void UseSpell(Player playerAttacked, Spell spell)
    {
        if (CheckCanUseSpell(playerAttacked, spell))
        {
            switch (spell.utilityType)
            {
                case UtilityType.Damage:
                    TakeFinalSpellDamage(playerAttacked, spell);
                    break;
                case UtilityType.Healing:
                    if (spell.spellAreaType == SpellAreaType.Self) HealSelfWithSpell(spell);
                    else throw new System.NotImplementedException(); //TODO UtilityType.Healing curar otros aliados
                    break;
                case UtilityType.Knockback:
                    throw new System.NotImplementedException(); //TODO UtilityType.Knockback enemy
            }
            SpendActionPoints(spell.actionPointCost);
        }
    }

    protected void HealSelfWithSpell(Spell spell)
    {
        int baseCharHealing = _damagePoints * 10;
        int baseSpellHealing = spell.baseDamageOrHealing;
        int critsPerc = _critsPerc * 10;

        int critsHealing = 0;
        if (Random.Range(1, 101) >= critsPerc) critsHealing = Mathf.FloorToInt(baseSpellHealing * 1.5f);

        int damageWithCrits = baseCharHealing + baseSpellHealing + critsHealing;

        int finalHealing = damageWithCrits;
        GetHealth(finalHealing);
    }

    private bool CheckCanUseSpell(Player playerAttacked, Spell spell)
    {
        if (_actionPoints < spell.actionPointCost) return false;

        bool canUseSpell = true;
        int distance = GridManager.DistanceBetweenTiles(GetCharacterTile(), playerAttacked.GetCharacterTile());
        switch (spell.spellAreaType)
        {
            case SpellAreaType.Melee:
                canUseSpell = distance <= 1;
                break;
            case SpellAreaType.Donut:
                canUseSpell = distance <= 2;
                break;
            case SpellAreaType.Range:
                canUseSpell = distance <= 5 && distance > 2;
                break;
            case SpellAreaType.Line:
                //TODO SpellAreaType.Line
                throw new System.NotImplementedException();
            case SpellAreaType.Self:
                canUseSpell = true;
                break;
        }
        return canUseSpell;
    }


    public IEnumerator MovingThroughPathCoroutine(float animationSpeed = 0.3f)
    {
        float currentTime = 0;
        List<Tile> path = PathfindingManager.Instance.FinalPath;

        if (_enemyType == EnemyType.Melee) path.RemoveAt(path.Count - 1);

        foreach (Tile tile in path)
        {
            if (_movementPoints <= 0) break;

            Vector3 startPos = transform.position;

            Vector3 tilePos = tile.transform.position;
            Vector3 endPos = new Vector3(tilePos.x, tilePos.y, tile.Coords.y);

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
            SpendMovementPoint();
            tile.Path = false;
        }
    }



}
