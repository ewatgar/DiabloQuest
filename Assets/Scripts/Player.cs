using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private void Awake()
    {
        AddObserverToAllTiles();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void AddObserverToAllTiles()
    {
        var tileList = GridManager.Instance.TileList;
        foreach (Tile tile in tileList)
        {
            tile.OnTileClicked += HandleTileClicked;
        }
    }

    private void HandleTileClicked(Vector3 coordinates)
    {
        transform.position = coordinates;
    }

}
