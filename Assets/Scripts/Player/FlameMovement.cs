using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FlameMovement : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D characterBody;
    private Vector2 velocity;
    private Vector3 inputMovement;

    void Start()
    {
        // velocity = new Vector2(speed, speed);
        characterBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        inputMovement = new Vector2 (
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
    }

    private void FixedUpdate()
    {
        Vector2 delta = speed * Time.deltaTime * inputMovement;
        Vector2 newPosition = characterBody.position + delta;
        characterBody.MovePosition(newPosition);
    }   
}
