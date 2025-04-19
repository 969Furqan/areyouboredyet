using UnityEngine;
using UnityEngine.Tilemaps;

public class TileAdjuster : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile leftWallTile, rightWallTile, bottomWallTile;

    void Start()
    {
        AdjustTiles();
    }

    void AdjustTiles()
    {
        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);
            if (tile != null)
            {
                if (tile == leftWallTile)
                {
                    // Adjust the position if necessary
                    tilemap.SetTransformMatrix(position, Matrix4x4.Translate(new Vector3(-0.25f, 0, 0)));
                }
                else if (tile == rightWallTile)
                {
                    tilemap.SetTransformMatrix(position, Matrix4x4.Translate(new Vector3(0.25f, 0, 0)));
                }
                else if (tile == bottomWallTile)
                {
                    tilemap.SetTransformMatrix(position, Matrix4x4.Translate(new Vector3(0, -0.25f, 0)));
                }
            }
        }
    }
}
