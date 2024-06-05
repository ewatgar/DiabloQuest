using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("Player Items")]
    [SerializeField] private List<Item> listItems;

    public List<Item> ListItems { get => listItems; }

    public List<Tile> GetSelectableSpellTiles(Spell spell)
    {
        List<Tile> selectableTiles = new List<Tile>();
        switch (spell.spellAreaType)
        {
            case SpellAreaType.Melee:
                selectableTiles = GetMeleeTiles();
                break;
            case SpellAreaType.Range:
                throw new NotImplementedException();
            case SpellAreaType.Donut:
                selectableTiles.Add(GetCharacterTile());
                break;
            case SpellAreaType.Line:
                selectableTiles = GetMeleeTiles();
                break;
            case SpellAreaType.Self:
                selectableTiles.Add(GetCharacterTile());
                break;
        }
        bool notEnoughAP = _actionPoints < spell.actionPointCost;
        ColorSpellSelectableTiles(selectableTiles);
        return selectableTiles;
    }

    public List<Tile> GetAreaOfEffectTiles(Spell spell, Tile selectableTile)
    {
        List<Tile> areaEffectsTiles = new List<Tile>();
        switch (spell.spellAreaType)
        {
            case SpellAreaType.Melee:
                areaEffectsTiles.Add(selectableTile);
                break;
            case SpellAreaType.Range:
                throw new NotImplementedException();
            case SpellAreaType.Donut:
                areaEffectsTiles = GetAreaEffectDonutTiles();
                break;
            case SpellAreaType.Line:
                areaEffectsTiles = GetAreaEffectLineTiles(selectableTile, 3);
                break;
            case SpellAreaType.Self:
                areaEffectsTiles.Add(GetCharacterTile());
                break;
        }
        bool notEnoughAP = _actionPoints < spell.actionPointCost;
        ColorSpellAreaOfEffectTiles(areaEffectsTiles, notEnoughAP);
        return areaEffectsTiles;
    }

    private List<Tile> GetAreaEffectDonutTiles()
    {
        return GetDonutTiles();
    }

    private List<Tile> GetAreaEffectLineTiles(Tile selectableTile, int range)
    {
        return GetLineTiles(selectableTile, range);
    }


    private List<Tile> GetMeleeTiles()
    {
        List<Tile> listTiles = new List<Tile>
        {
            GetSpellTile(Vector2Int.up),
            GetSpellTile(Vector2Int.down),
            GetSpellTile(Vector2Int.left),
            GetSpellTile(Vector2Int.right),
        };
        return listTiles;
    }

    private List<Tile> GetDonutTiles()
    {
        List<Tile> listTiles = GetMeleeTiles();
        List<Tile> diagonalTiles = new List<Tile>
        {
            GetSpellTile(new Vector2Int(1, 1)),     // Diagonal up-right
            GetSpellTile(new Vector2Int(-1, 1)),    // Diagonal up-left
            GetSpellTile(new Vector2Int(1, -1)),    // Diagonal down-right
            GetSpellTile(new Vector2Int(-1, -1)),   // Diagonal down-left
        };
        listTiles.AddRange(diagonalTiles);
        return listTiles;
    }

    private List<Tile> GetLineTiles(Tile tile, int range)
    {
        var grid = GridManager.Instance;

        List<Tile> listTiles = new List<Tile>();
        Vector2Int lineDir = grid.GetMeleeDirFromTwoTiles(GetCharacterTile(), tile);

        Vector2Int sum = GetCharacterTile().Coords;
        for (int i = 1; i <= range; i++)
        {
            sum += lineDir;
            Tile lineTile = grid.GetTileFromTileCoords(sum);
            listTiles.Add(lineTile);
        }
        return listTiles;
    }

    private Tile GetSpellTile(Vector2Int dir)
    {
        var grid = GridManager.Instance;
        Tile spellTile = grid.GetTileFromDirection(GetCharacterTile(), dir);
        //if (spellTile != null) spellTile.SpellSelection = true;
        return spellTile;
    }

    // Color spell tiles -------------------------
    private void ColorSpellSelectableTiles(List<Tile> selectableTiles)
    {
        foreach (Tile tile in selectableTiles)
        {
            if (tile != null) tile.SpellSelectable = true;
        }
    }

    private void ColorSpellAreaOfEffectTiles(List<Tile> selectableTiles, bool notEnoughAP)
    {
        foreach (Tile tile in selectableTiles)
        {
            if (tile != null)
            {
                if (notEnoughAP) tile.SpellAreaEffectNoAP = true;
                else tile.SpellAreaEffect = true;
            }
        }
    }

    // ----------------------------------------------------------------------

    public IEnumerator AttackEnemyCoroutine(Enemy enemyAttacked, Spell spell)
    {
        switch (spell.utilityType)
        {
            case UtilityType.Damage:
                yield return StartCoroutine(enemyAttacked.PlayHurtAnimationCoroutine());
                TakeFinalSpellDamage(enemyAttacked, spell);
                break;
            case UtilityType.Healing:
                throw new NotImplementedException();
            case UtilityType.Knockback:
                yield return StartCoroutine(KnockbackCharCoroutine(enemyAttacked, 2));
                break;
        }
        //yield return new WaitForSeconds(.2f);
        print("Enemy Attacked: " + enemyAttacked.name);
    }

    public IEnumerator ApplySpellEffectCoroutine(Spell selectedSpell)
    {
        if (selectedSpell.utilityType == UtilityType.Healing)
        {
            GetHealth(selectedSpell.value);
        }
        else if (selectedSpell.utilityType == UtilityType.MovementPoints)
        {
            _movementPoints += selectedSpell.value;
        }
        else if (selectedSpell.utilityType == UtilityType.ActionPoints)
        {
            _actionPoints += selectedSpell.value;
        }
        else if (selectedSpell.utilityType == UtilityType.Crits)
        {
            _critsPerc = selectedSpell.value / 10;
        }
        yield return null;
    }
}
