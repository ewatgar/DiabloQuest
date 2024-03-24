
using UnityEngine;

public static class Utils
{
    public static Vector2Int WorldToTileCoords(Vector3 worldCoords, float gridScale)
    {
        worldCoords += new Vector3(gridScale / 2f, gridScale / 2f, 0f);

        int tileX = Mathf.FloorToInt(worldCoords.x / gridScale);
        int tileY = Mathf.FloorToInt(worldCoords.y / gridScale);

        return new Vector2Int(tileX, tileY);
    }

}
