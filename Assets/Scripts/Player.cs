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

    private Vector2Int _coords;
    public Vector2Int Coords { get => _coords; }

    private void Awake()
    {
        _coords = Utils.WorldToTileCoords(transform.position, GridGenerator.Instance.GridScale);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void GetHurt(int hp = 1)
    {
        _healthPoints -= hp;
    }

    public void Move(Vector3 position, int _mp = 1)
    {
        transform.position = position;
        _movementPoints -= _mp;
    }

    public bool CanMove(Vector2Int tileCoords, out int diff)
    {
        diff = 0;

        if (Coords == tileCoords)
        {
            return false;
        }

        if (Coords.x == tileCoords.x && Coords.y != tileCoords.y)
        {
            int diffY = Mathf.Abs(Coords.y - tileCoords.y);
            diff = diffY;
            return diffY <= _movementPoints;
        }
        else if (Coords.y == tileCoords.y && Coords.x != tileCoords.x)
        {
            int diffX = Mathf.Abs(Coords.x - tileCoords.x);
            diff = diffX;
            return diffX <= _movementPoints;
        }
        return false;
    }

}
