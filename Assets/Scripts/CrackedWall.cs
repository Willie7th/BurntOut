using UnityEngine;

public class CrackedWall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Collider2D wallCollider;
   
    void Start()
    {
         wallCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        FlameController flameController = collision.gameObject.GetComponent<FlameController>();
        Debug.Log("Enter? " + flameController.flameType);
        wallCollider.enabled = !(flameController.flameType==FlameType.miniFlame);
        Debug.Log("Enter? " + wallCollider.enabled);

    }
        void OnCollisionExit2D(Collision2D collision)
    {
        wallCollider.enabled = true;

    }
}
