using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class FlameController : MonoBehaviour
{
    public int speed = 5;
    private Rigidbody2D characterBody;
    private ParticleSystem FlameAlpha;
    private ParticleSystem FlameAdd;
    private ParticleSystem FlameGlow;
    private ParticleSystem FlameSparks;
    private Light2D FlameLight;
    private Vector2 velocity;
    private Vector2 inputMovement;
    private Vector2 previousInputMovement = new Vector2(0f, 0f);

    public GameObject emberPrefab;
    private GameController _gameController;

    public TextMeshProUGUI energyLabel;

    public double energy = 1000;  //Some stage make private set but public get

    // public string flameType = "default";

    public FlameType flameType = FlameType.mainFlame;

    public FlameColour flameColour=FlameColour.none;

    private static GradientAlphaKey[] ALPHA_KEYS = new GradientAlphaKey[3] {
        new GradientAlphaKey(0.0f, 0.0f),
        new GradientAlphaKey(1.0f, 0.3f),
        new GradientAlphaKey(0.0f, 1.0f)
    };

    public Color AluminumColor = new Color(0.14f, 0.76f, 0.92f, 1.0f);
    public Gradient AluminumGradient = new Gradient()
    {
        colorKeys = new GradientColorKey[2] {
            new GradientColorKey(new Color(0.14f, 0.76f, 0.92f, 1.0f), 0.3f),
            new GradientColorKey(new Color(0.08f, 0.15f, 0.54f, 1.0f), 0.6f),
        },
        alphaKeys = ALPHA_KEYS
    };


    void Start()
    {
        velocity = new Vector2(speed, speed);
        characterBody = GetComponent<Rigidbody2D>();

        if (_gameController == null)
            _gameController = FindAnyObjectByType<GameController>();
        
        FlameLight = this.transform.GetChild(1).gameObject.GetComponent<Light2D>();
        FlameAlpha = this.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        FlameAdd = this.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
        FlameGlow = this.transform.GetChild(4).gameObject.GetComponent<ParticleSystem>();
        FlameSparks = this.transform.GetChild(5).gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        inputMovement = new Vector2(
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
        Debug.Log("Trigger enter " + col.gameObject.name);

        string colName = col.gameObject.name;

        if(colName == "Ember")
        {
            pickUPEmber(col.gameObject);
        }
        else if (colName == "AlluminumOre")
        {
            changeColour(AluminumGradient, AluminumColor,FlameColour.alluminium);
        }
        else if (colName == "IronOre")
        {
            changeColour(AluminumGradient, AluminumColor,FlameColour.iron);
        }
        else if (colName == "CopperOre")
        {
            changeColour(AluminumGradient, AluminumColor,FlameColour.copper);
        }

        if(colName == "Finish")
        {
            _gameController.finishLevel();
        }
    }

    public void meltFlame()
    {

    }

    private void flameJump()
    {
        if (energy < 30)
        {
            Debug.Log("Too small to drop ember");
            return;
        }
        Debug.Log("Flame jumped");
        energy = energy * 0.99;
    }

    private void flameSplit()
    {
        if (energy < 80)
        {

            
            Debug.Log("Too small to split ember");
            return;
        }
        Debug.Log("Flame split");
        //TEMP CODE REMOVE REMOVE REMOVE REMOVE
        if (this.flameType == FlameType.mainFlame)
        this.flameType = FlameType.miniFlame;
        else
        this.flameType = FlameType.mainFlame;
        // REMOVE REMOVE REMOVE
        energy = energy - 50;
    }

    private void dropEmber()
    {
        if (energy < 30)
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

    private void changeColour(Gradient grad, Color lightColor,FlameColour flameColour)
    {

        Debug.Log("Changing colour");
        this.flameColour = flameColour;
        switch (flameColour)
        {
            case FlameColour.copper:
                break;
            case FlameColour.iron:

                break;
            case FlameColour.alluminium:
                var colAlpha = FlameAlpha.colorOverLifetime;
                colAlpha.color = grad;
                var colAdd = FlameAdd.colorOverLifetime;
                colAdd.color = grad;
                var colGlow = FlameGlow.colorOverLifetime;
                colGlow.color = grad;
                var colSparks = FlameSparks.colorOverLifetime;
                colSparks.color = grad;
                FlameLight.color = lightColor;
                break;

            default:
                break;
        }
    }

    public void setFlameEnergy(double inEnergy)
    {
        energy = inEnergy;
    }

    public double getFlameEnergy()
    {
        int tempEnergy = (int) energy;
        return tempEnergy;
    }
}
