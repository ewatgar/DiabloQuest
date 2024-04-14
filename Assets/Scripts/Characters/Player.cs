using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Player Stats
    private int _healthPoints; //health points
    private int _actionPoints; //action points
    [SerializeField] private int _movementPoints = 3; //movement points
    private int _damage; //damage
    private int _resistancePerc; //resistance %
    private int _critsPerc; //crits %

    //Turns

    //Movement
    private bool _bPlayerMoving = false;
    [SerializeField] private float _animationSpeed = 0.3f;

    public int HealthPoints { get => _healthPoints; set => _healthPoints = value; }
    public int ActionPoints { get => _actionPoints; set => _actionPoints = value; }
    public int MovementPoints { get => _movementPoints; set => _movementPoints = value; }
    public int Damage { get => _damage; set => _damage = value; }
    public int ResistancePerc { get => _resistancePerc; set => _resistancePerc = value; }
    public int CritsPerc { get => _critsPerc; set => _critsPerc = value; }

    private void Awake()
    {

    }

    public bool EnoughMovementPoints(Tile tile)
    {
        RegeneratePath(GetPlayerTile(), tile);
        int tilePathCount = PathfindingManager.Instance.FinalPath.Count;
        return tilePathCount <= _movementPoints;
    }

    public void OnPlayerSelectTileMove(Tile selectedTile)
    {
        RegeneratePath(GetPlayerTile(), selectedTile);
        PathfindingManager.Instance.ColorFinalPath();
    }

    private void RegeneratePath(Tile playerTile, Tile selectedTile)
    {
        PathfindingManager.Instance.ClearValues();
        PathfindingManager.Instance.SetPathfinding(playerTile, selectedTile);
    }

    public IEnumerator MovingThroughPath()
    {
        print("empieza corrutina player moving");
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
                /*
                transform.position = new Vector3(
                    Mathf.Lerp(startPos.x, endPos.x, t),
                    Mathf.Lerp(startPos.y, endPos.x, t),
                    tile.Coords.y //get layer
                );*/
                transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }

            transform.position = new Vector3(
                endPos.x,
                endPos.y,
                tile.Coords.y
            );
            //transform.position = tile.transform.position;
            currentTime = 0;
            SpendMovementPoint();
            tile.UncolorPathTile();
        }
        print("termina corrutina player moving");
    }

    public Tile GetPlayerTile()
    {
        return GridManager.Instance.GetTileFromWorldCoords(transform.position);
    }

    public void GetHurt(int hp = 1)
    {
        HealthPoints -= hp;
    }

    public void SpendMovementPoint()
    {
        MovementPoints--;
    }



    /*
        public bool CanMove(Vector2Int tileCoords, out int diff)
        {
            Tile playerTile = GetPlayerTile();

            diff = 0;

            if (playerTile.Coords == tileCoords)
            {
                return false;
            }

            if (playerTile.Coords.x == tileCoords.x && playerTile.Coords.y != tileCoords.y)
            {
                int diffY = Mathf.Abs(playerTile.Coords.y - tileCoords.y);
                diff = diffY;
                return diffY <= _movementPoints;
            }
            else if (playerTile.Coords.y == tileCoords.y && playerTile.Coords.x != tileCoords.x)
            {
                int diffX = Mathf.Abs(playerTile.Coords.x - tileCoords.x);
                diff = diffX;
                return diffX <= _movementPoints;
            }
            return false;
        }*/


}
