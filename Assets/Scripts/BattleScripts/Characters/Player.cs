using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public List<Tile> GetSpellTiles(Spell spell)
    {
        var grid = GridManager.Instance;

        print(spell.name);
        List<Tile> listTiles = new List<Tile>
        {
            GetSpellTile(Vector2Int.up),
            GetSpellTile(Vector2Int.down),
            GetSpellTile(Vector2Int.left),
            GetSpellTile(Vector2Int.right),
            //diagonales
            //GetSpellTile()
            //TODO 

        };
        return listTiles;
    }

    private Tile GetSpellTile(Vector2Int dir)
    {
        var grid = GridManager.Instance;
        Tile spellTile = grid.GetTileFromDirection(GetCharacterTile(), dir);
        spellTile.Spell = true;
        return spellTile;
    }
}
