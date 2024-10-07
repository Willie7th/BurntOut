using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MetalWallCollision : MonoBehaviour

{
    public int metalTileHitPoints = 3; 
    public int damage = 1;
    public float damageInterval = 0.5f;


    public TileBase metlingTile; 
    public TileBase meltedTile; 
    private Dictionary<Vector3Int, int> tileHitPoints = new Dictionary<Vector3Int, int>();
    private Tilemap tilemap;
    private Tilemap background;
    private float damageTimer;

    void Start()
    {
        tilemap = GameObject.FindWithTag("metal_1").GetComponent<Tilemap>();
        background = GameObject.FindWithTag("paths").GetComponent<Tilemap>();

    }

    void Update()
    {
       
        damageTimer += Time.deltaTime;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
      
        if (collision.gameObject.CompareTag("metal_1"))
        {
          
            if (damageTimer >= damageInterval)
            {
               
                damageTimer = 0f;

              
                Vector3 hitPosition = Vector3.zero;
                foreach (ContactPoint2D hit in collision.contacts)
                {
                    hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                    hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                    break;
                }

                Vector3Int cellPosition = tilemap.WorldToCell(hitPosition);

             
                if (!tileHitPoints.ContainsKey(cellPosition))
                {
              
                    tileHitPoints[cellPosition] = metalTileHitPoints;
                }

             
                tileHitPoints[cellPosition] -= damage;



                if (tileHitPoints[cellPosition] <= 0)
                {
                    // tilemap.SetTile(cellPosition, null);
                    tilemap.SetTile(cellPosition,meltedTile);
                    tileHitPoints.Remove(cellPosition); 
                } else if (tileHitPoints[cellPosition] <= metalTileHitPoints/2)
                {
                    tilemap.SetTile(cellPosition,metlingTile);
                }
            }
        }
    }
}
