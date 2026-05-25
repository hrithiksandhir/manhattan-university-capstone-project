using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapToBoxColliders : MonoBehaviour
{
    public float depth = 0.5f; // thickness of the collider along Z-axis
    public string climbableTag = "Climbable"; // tag to assign to each tile collider

    void Start()
    {
        GenerateBoxColliders();
    }

    void GenerateBoxColliders()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        Vector3 tileSize = tilemap.cellSize;
        Vector3 worldScale = tilemap.transform.lossyScale; // accounts for any scaling

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (!tilemap.HasTile(tilePos)) continue;

                // Create a child GameObject with BoxCollider
                GameObject boxObj = new GameObject($"BoxCollider_{x}_{y}");
                boxObj.transform.parent = transform;

                // Set the position to the center of the tile
                Vector3 worldPos = tilemap.GetCellCenterWorld(tilePos);
                boxObj.transform.position = worldPos;

                // Apply the tilemap scale to the object
                boxObj.transform.localScale = worldScale;

                // Add and configure BoxCollider
                BoxCollider box = boxObj.AddComponent<BoxCollider>();

                // Set the size to compensate for scaling (so it's not scaled twice)
                box.size = new Vector3(
                    tileSize.x / worldScale.x,
                    tileSize.y / worldScale.y,
                    depth / worldScale.z // optional: scale depth too
                );

                // Assign tag if needed
                boxObj.tag = climbableTag;
            }
        }
    }
}
