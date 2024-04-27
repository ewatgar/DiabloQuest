using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public List<Tile> GetSpellTiles(Spell spell)
    {
        List<Tile> listTiles = new List<Tile>();
        switch (spell.spellAreaType)
        {
            case SpellAreaType.Melee:
                listTiles = GetMeleeTiles();
                break;
            case SpellAreaType.Range:
                //TODO no se usa SpellAreaType.Range en player
                break;
            case SpellAreaType.Donut:
                listTiles = GetDonutTiles();
                break;
            case SpellAreaType.Line:
                //TODO no se usa SpellAreaType.Line en player
                break;
            case SpellAreaType.Self:
                //TODO no se usa SpellAreaType.Self en player
                break;
        }
        return listTiles;
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


    private Tile GetSpellTile(Vector2Int dir)
    {
        var grid = GridManager.Instance;
        Tile spellTile = grid.GetTileFromDirection(GetCharacterTile(), dir);
        if (spellTile != null) spellTile.Spell = true;
        return spellTile;
    }
    /*
        public bool CheckIfCanCastSpell(Spell spell, Tile tile)
        {
            return GetSpellTiles(spell).Contains(tile);
        }*/

    public IEnumerator AttackEnemyCoroutine(Enemy enemy, Spell spell)
    {
        //TODO attack enemy
        yield return null;

    }


}
