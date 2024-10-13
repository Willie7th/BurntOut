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

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     FlameController flameController = collision.gameObject.GetComponent<FlameController>();
    //     wallCollider.enabled = !(flameController.flameType==FlameType.miniFlame);
    //     Debug.Log("Enter? " + flameController.flameType + " " + wallCollider.enabled);

    // }
    //     void OnCollisionExit2D(Collision2D collision)
    // {
    //     FlameController flameController = collision.gameObject.GetComponent<FlameController>();

    //     if ( flameController.flameType==FlameType.miniFlame)
    //         wallCollider.enabled = true;

    //     Debug.Log("Exit Enabled?" + wallCollider.enabled);

    // }
    void OnTriggerEnter2D(Collider2D other)
{
    FlameController flameController = other.GetComponent<FlameController>();

    if (flameController != null)
    {
        // Disable the collider only if the flame type is miniFlame
        if (flameController.flameType == FlameType.miniFlame)
        {
            wallCollider.enabled = false; // Allow mini flames to pass through
        }
        else
        {
            wallCollider.enabled = true; // Block other flame types
        }
        
        Debug.Log("Trigger Enter? " + flameController.flameType + " " + wallCollider.enabled);
    }
}

void OnTriggerExit2D(Collider2D other)
{
    // Re-enable the wall collider when the object exits
    wallCollider.enabled = true;
    Debug.Log("Trigger Exit Enabled? " + wallCollider.enabled);
}

}
