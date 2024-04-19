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

    public new void SelectTileForPathfinding(Tile playerTile, bool color)
    {
        Tile goalTile = playerTile;
        int distance = GridManager.DistanceBetweenTiles(GetCharacterTile(), playerTile);
        if (_enemyType == EnemyType.LongRange
        && distance < _minDistanceLongRange)
        {
            goalTile = GetTileAwayFromPlayer(playerTile);
        }

        PathfindingManager.Instance.RegeneratePath(GetCharacterTile(), goalTile, color);
    }

    private Tile GetTileAwayFromPlayer(Tile playerTile)
    {
        Tile[,] tileGrid = GridManager.Instance.TileGrid;

        List<Tile> farAwayTiles = new List<Tile>();
        foreach (Tile tile in tileGrid)
        {
            if (GridManager.DistanceBetweenTiles(tile, playerTile) >= _minDistanceLongRange)
            {
                farAwayTiles.Add(tile);
            }
        }
        int index = Random.Range(0, farAwayTiles.Count - 1);
        return farAwayTiles[index];
    }

    public IEnumerator EnemyMovementCoroutine(Tile playerTile)
    {
        SelectTileForPathfinding(playerTile, true);
        yield return StartCoroutine(MovingThroughPathCoroutine());
    }

    public IEnumerator EnemyAttackCoroutine(Tile playerTile)
    {
        int step = 100;
        while (_actionPoints > 0 || step < 100)
        {
            int index = Random.Range(0, ListSpells.Count - 1);
            Spell randomSpell = _listSpells[index];

            //check if can use spell
            if (_actionPoints >= randomSpell.actionPointCost)
            {
                if (randomSpell.spellAreaType == SpellAreaType.Range)
                {

                }

                SpendActionPoints(randomSpell.actionPointCost);
            }



            //check if its damage spell

            //check if can use spell on player

            //

            step++;
        }




        yield return null;

    }

    public new IEnumerator MovingThroughPathCoroutine()
    {
        //print("empieza corrutina character moving");
        float currentTime = 0;

        List<Tile> path = PathfindingManager.Instance.FinalPath;
        if (_enemyType == EnemyType.Melee) path.RemoveAt(path.Count - 1);

        foreach (Tile tile in path)
        {
            if (_movementPoints <= 0)
            {
                break;
            }

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



}
