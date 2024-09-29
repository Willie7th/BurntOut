using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractableTile : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap
    public TileBase passableTile; // The tile to replace the obstacle with
    public int hitPoints = 1; // Hitpoints of the tile

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Damage the tile
            hitPoints--;

            // Check if hitpoints are depleted
            if (hitPoints <= 0)
            {
                // Change the tile to a passable tile
                Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
                tilemap.SetTile(cellPosition, passableTile);

                // Optionally, destroy the GameObject if it's no longer needed
                Destroy(gameObject);
            }
        }
    }
}
