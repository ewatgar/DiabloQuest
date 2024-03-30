using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Player : MonoBehaviour
{

    //Player Stats
    private int _healthPoints; //health points
    private int _actionPoints; //action points
    [SerializeField] private int _movementPoints = 3; //movement points
    private int _damage; //damage
    private int _resistancePerc; //resistance %
    private int _critsPerc; //crits %

    private Tile _currentTile;
    public Tile CurrentTile { get => _currentTile; }

    private GridGenerator _grid;


    private void Start()
    {
        _grid = GridGenerator.Instance;
        _currentTile = _grid.GetTileFromWorldCoords(transform.position);
    }

    public int GetTileDistance(Tile goalTile)
    {
        int xDistance = Mathf.Abs(goalTile.Coords.x - _currentTile.Coords.x);
        int yDistance = Mathf.Abs(goalTile.Coords.y - _currentTile.Coords.y);
        int totalDistance = xDistance + yDistance;

        return totalDistance;
    }

    public bool EnoughMovementPoints(Tile goalTile)
    {
        return GetTileDistance(goalTile) <= _movementPoints;
    }

    public void Move(List<Tile> path, Tile goalTile)
    {
        StartCoroutine(MoveOnPath(path, goalTile));
    }

    private IEnumerator MoveOnPath(List<Tile> path, Tile goalTile)
    {
        print("corroutine");
        _movementPoints -= GetTileDistance(goalTile);
        float duration = .3f;

        foreach (Tile tile in path)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = tile.transform.position;
            float currentTime = 0;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float t = currentTime / duration;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
        }
        transform.position = goalTile.transform.position;
        _currentTile = goalTile;
        yield break;
    }


    /*
        private IEnumerator MoveOnPathOld(List<Tile> path, Tile goalTile)
        {
            _bPlayerMoving = true;
            float duration = .3f;

            foreach (Tile tile in path)
            {
                Vector3 startPos = transform.position;
                Vector3 endPos = tile.transform.position;
                float currentTime = 0;

                while (currentTime < duration)
                {
                    currentTime += Time.deltaTime;
                    float t = currentTime / duration;
                    transform.position = Vector3.Lerp(startPos, endPos, t);
                    yield return null;
                }
                transform.position = tile.transform.position;
            }
            _currentTile = goalTile;
            _bPlayerMoving = false;
        }
    */
    public void GetHurt(int hp = 1)
    {
        _healthPoints -= hp;
    }
}
