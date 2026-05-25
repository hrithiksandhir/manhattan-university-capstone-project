using UnityEngine;
using UnityEngine.Tilemaps;

public class FlatCollidersForTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public float colliderDepth = 0.5f; // Thickness in Z

    void Start()
    {
        if (tilemap == null)
            tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos))
                continue;

            Vector3 worldPos = tilemap.GetCellCenterWorld(pos);

            GameObject tileCollider = new GameObject("TileCollider_" + pos);
            tileCollider.transform.parent = this.transform;

            // Add collider
            BoxCollider box = tileCollider.AddComponent<BoxCollider>();
            box.size = new Vector3(2f, 2f, colliderDepth);

            // Keep everything at Z = 0 so all characters interact correctly
            tileCollider.transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
        }
    }
}
