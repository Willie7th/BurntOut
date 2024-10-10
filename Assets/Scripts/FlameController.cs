using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlameController : MonoBehaviour
{
    public int speed = 5;
    private Rigidbody2D characterBody;
    private Vector2 velocity;
    private Vector2 inputMovement;
    private Vector2 previousInputMovement = new Vector2(0f, 0f);

    public GameObject emberPrefab;

    public TextMeshProUGUI energyLabel;

    public double energy = 1000;  //Some stage make private set but public get

    public string flameType = "default";

    void Start()
    {
        velocity = new Vector2(speed, speed);
        characterBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        inputMovement = new Vector2 (
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (inputMovement != Vector2.zero)
        {
            previousInputMovement = inputMovement;
        }

        //energyLabel.text = (int) energy + "";  //Counter for now to track energy

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Drop Ember
            dropEmber();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Flame Jump
            flameJump();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            //Mini flame
            flameSplit();
        }

    }

    private void FixedUpdate()
    {
        Vector2 delta = inputMovement * velocity * Time.deltaTime;
        Vector2 newPosition = characterBody.position + delta;
        characterBody.MovePosition(newPosition);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Trigger enter");

        if(col.gameObject.name == "Ember")
        {
            pickUPEmber(col.gameObject);
        }
    }   

    public void meltFlame()
    {
        
    }

    private void flameJump()
    {
        if(energy < 30)
        {
            Debug.Log("Too small to drop ember");
            return;
        }
        Debug.Log("Flame jumped");
        energy = energy * 0.99;
    }

    private void flameSplit()
    {
        if(energy < 80)
        {
            Debug.Log("Too small to split ember");
            return;
        }
        Debug.Log("Flame split");
        energy = energy - 50;
    }

    private void dropEmber()
    {
        if(energy < 30)
        {
            Debug.Log("Too small to drop ember");
            return;
        }
        Debug.Log("Ember dropped");
        energy = energy - 10;

        Vector3 offset = -previousInputMovement * 0.5f;
        Debug.Log(previousInputMovement);
        GameObject emberInstance = Instantiate(emberPrefab, this.gameObject.transform.position + offset, this.gameObject.transform.rotation);  //drop ember at player position
        emberInstance.name = "Ember";
    }

    private void pickUPEmber(GameObject ember)
    {
        Debug.Log("Ember picked up");
        Destroy(ember); //Destroy the ember
        energy = energy + 10;
    }

    private void combineSplitFlame(GameObject miniFlame)
    {
        Debug.Log("Mini flame picked up");
        Destroy(miniFlame); //Destroy the ember
        energy = energy + 30;
    }

    private void changeColour()
    {

    }

    public void setFlameEnergy(double inEnergy)
    {
        energy = inEnergy;
    }

    public double getFlameEnergy()
    {
        return energy;
    }
}
