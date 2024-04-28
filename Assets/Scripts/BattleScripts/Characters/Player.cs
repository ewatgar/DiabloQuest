using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
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
        ColorSpellAreaOfEffectTiles(areaEffectsTiles);
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
        Vector2Int dir = Vector2Int.up;
        if (tile == GetSpellTile(Vector2Int.up)) dir = Vector2Int.up;
        if (tile == GetSpellTile(Vector2Int.down)) dir = Vector2Int.down;
        if (tile == GetSpellTile(Vector2Int.left)) dir = Vector2Int.left;
        if (tile == GetSpellTile(Vector2Int.right)) dir = Vector2Int.right;

        Vector2Int sum = GetCharacterTile().Coords;
        for (int i = 1; i <= range; i++)
        {
            sum += dir;
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

    private void ColorSpellAreaOfEffectTiles(List<Tile> selectableTiles)
    {
        foreach (Tile tile in selectableTiles)
        {
            if (tile != null) tile.SpellAreaEffect = true;
        }
    }

    // ----------------------------------------------------------------------

    public IEnumerator AttackEnemyCoroutine(Enemy enemy, Spell spell)
    {
        //TODO attack enemy
        yield return null;

    }


}
