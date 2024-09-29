using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public int damage = 1; 
    public int metalTileHitPoints = 3;
    private Dictionary<Vector3Int, int> tileHitPoints = new Dictionary<Vector3Int, int>();


    void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.GetComponent<TilemapCollider2D>() != null)
        {
            // Debug.Log(collision.gameObject.tag.ToString());
            if (collision.gameObject.CompareTag("Metal_1"))
            {
              
                Vector3 hitPosition = Vector3.zero;
                foreach (ContactPoint2D hit in collision.contacts)
                {
                    hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                    hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                    break;
                }

                // Convert to Grid position and destroy the tile
                Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
                Vector3Int cellPosition = tilemap.WorldToCell(hitPosition);

                if (!tileHitPoints.ContainsKey(cellPosition))
                {
                    // If not, initialize with the maximum hit points
                    tileHitPoints[cellPosition] = metalTileHitPoints;
                }

                // Reduce the hit points of the tile
                tileHitPoints[cellPosition] -= damage;

                // Destroy the tile if its hit points are 0 or less
                if (tileHitPoints[cellPosition] <= 0)
                {
                    tilemap.SetTile(cellPosition, null);
                    tileHitPoints.Remove(cellPosition); // Remove from dictionary
                }
            }
        }
    }
}
