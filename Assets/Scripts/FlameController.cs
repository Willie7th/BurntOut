using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;


public class FlameController : MonoBehaviour
{
    public float speed = 5f;  //maybe add a sprint mechanic
    public float jumpDistance = 2.5f;

    private Rigidbody2D characterBody;
    public ParticleSystem FlameAlpha;
    public ParticleSystem FlameAdd;
    public ParticleSystem FlameGlow;
    public ParticleSystem FlameSparks;
    public Light2D FlameLight;
    private Vector2 velocity;
    private Vector2 inputMovement;
    private Vector2 previousInputMovement = new Vector2(0f, 0f);

    public GameObject emberPrefab;
    private GameController _gameController;
    private SoundManager _soundManager;

    public GameObject miniflamePrefab;
    private Camera flameCamera;

    private bool isMoving = false;
    public bool mainFlame;

    private Vector2? jumpTarget = null;
    private Vector2 jumpVector;

    [SerializeField] private AudioClip flameMoveAudio;

    public TextMeshProUGUI energyLabel;

    public double energy = 1000;  //Some stage make private set but public get
    private double startEnergy;
    private float startOuterRadius;
    private float startInnerRadius;

    public FlameType flameType = FlameType.mainFlame;
    public FlameColour flameColour = FlameColour.iron;
    public Color AluminumColor = new(0.14f, 0.76f, 0.92f, 1.0f);
    public Color CopperColor = new(0.15f, 0.6f, 0.08f, 1.0f);
    public Color IronColor = new(1.0f, 0.68f, 0.125f, 1.0f);

    public double Energy
    {
        get { return energy; }
        set
        {
            energy = value;
            float energyRatio = (float) energy / (float) startEnergy;
            FlameLight.pointLightOuterRadius = startOuterRadius * energyRatio;
            FlameLight.pointLightInnerRadius = startInnerRadius * energyRatio;
        }
    }

    void Start()
    {
        velocity = new Vector2(speed, speed);
        characterBody = GetComponent<Rigidbody2D>();
        if (_gameController == null)
            _gameController = FindAnyObjectByType<GameController>();
        if (_soundManager == null)
            _soundManager = FindAnyObjectByType<SoundManager>();
        FlameLight = this.transform.GetChild(1).gameObject.GetComponent<Light2D>();
        FlameAlpha = this.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        FlameAdd = this.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
        FlameGlow = this.transform.GetChild(4).gameObject.GetComponent<ParticleSystem>();
        FlameSparks = this.transform.GetChild(5).gameObject.GetComponent<ParticleSystem>();
        flameCamera = GetComponentInChildren<Camera>();
        startEnergy = energy;
        startOuterRadius = FlameLight.pointLightOuterRadius;
        startInnerRadius = FlameLight.pointLightInnerRadius;
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
            if(!mainFlame)
            {
                return;
            }
            //Drop Ember
            dropEmber();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!mainFlame)
            {
                return;
            }
            //Flame Jump
            FlameJump();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if(!mainFlame)
            {
                return;
            }
            // Mini flame
            flameSplit();
            CapsuleCollider2D cap = GetComponent<CapsuleCollider2D>();
            cap.size = new Vector2(1f, 1f);
            SpawnMiniflame();
        }
        bool isMovementKeyPressed =
            Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow);

        if (!_soundManager)
        {
            return;
        }
        else if (isMovementKeyPressed && !isMoving)
        {
            // Start playing movement sound
            _soundManager.PlayMovementSound(flameMoveAudio, 0.1f);
            isMoving = true;
            //Debug.Log("Is moving true");
        }
        else if (!isMovementKeyPressed && isMoving)
        {
            // Stop movement sound if no key is pressed
            _soundManager.StopMovementSound();
            isMoving = false;
            //Debug.Log("Is moving false");
        }
    }

    private void FixedUpdate()
    {
        Vector2 delta;
        // If jumpTarget is set we are jumping
        if (jumpTarget != null)
        {
            delta = jumpVector;
        }
        else
        {
            delta = inputMovement * velocity * Time.deltaTime;
        }
        Vector2 newPosition = characterBody.position + delta;
        characterBody.MovePosition(newPosition);
        // Check whether we've jumped to the target
        if (jumpTarget != null && (jumpTarget - newPosition)?.magnitude < 0.1)
        {
            jumpTarget = null;
            Debug.Log("End jump");
        }

    }

    public FlameColour getFlameType()
    {
        return flameColour;
    }

    public bool IsJumping()
    {
        return jumpTarget != null;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Trigger enter " + col.gameObject.name);

        string colName = col.gameObject.name;

        if (colName == "Ember")
        {
            pickUPEmber(col.gameObject);
        }
        else if (colName == "AlluminumOre")
        {
            ChangeColor(FlameColour.alluminium);
        }
        else if (colName == "IronOre")
        {
            ChangeColor(FlameColour.iron);
        }
        else if (colName == "CopperOre")
        {
            ChangeColor(FlameColour.copper);
        }
        else if (colName == "Finish")
        {
            _gameController.finishLevel();
        }
        else if (colName == "Water" && !IsJumping())
        {
            Debug.Log("You die from water");
            _gameController.waterDeath();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        string colName = collision.gameObject.name;
        if (colName == "Flame")
        {
            Debug.Log("Combining back into flame");
            combineSplitFlame(collision.gameObject);
        }
        else if (colName == "Water" && !IsJumping())
        {
            Debug.Log("Player Dies");
            _gameController.waterDeath();
        }
        else if (colName == "MiniFlame(Clone)")
        {
            Debug.Log("Get mini flame colour");
            Debug.Log("Current big flame colour - " + flameColour);
            Debug.Log("Current mini flame colour - " + collision.gameObject.GetComponent<FlameController>().flameColour);
            flameColour = collision.gameObject.GetComponent<FlameController>().flameColour;
            ChangeColor(flameColour);
            Debug.Log("New big flame colour - " + flameColour);
        }

    }

    private void FlameJump()
    {
        if (Energy < 30)
        {
            Debug.Log("Too small to jump");
            return;
        }
        // jumpVector is the direction we want to jump in
        jumpVector = previousInputMovement * jumpDistance;
        // jumpTarget is the position we want to jump to
        jumpTarget = characterBody.position + jumpVector;
        // Now we scale jumpVector so we can apply the jump in x steps
        jumpVector.Scale(new Vector2(0.2f, 0.2f));
        Energy *= 0.99;  // Use energy to jump
        Debug.Log("Start jump");
    }

    private void flameSplit()
    {
        if (Energy < 80)
        {
            Debug.Log("Too small to split ember");
            return;
        }
        Debug.Log("Flame split");
        Energy -= 50;
    }

    private void dropEmber()
    {
        if (Energy < 30)
        {
            Debug.Log("Too small to drop ember");
            return;
        }
        Debug.Log("Ember dropped");
        Energy -= 10;

        Vector3 offset = -previousInputMovement * 0.5f;
        //Debug.Log(previousInputMovement);
        GameObject emberInstance = Instantiate(emberPrefab, this.gameObject.transform.position + offset, this.gameObject.transform.rotation);  //drop ember at player position
        emberInstance.name = "Ember";
    }

    private void pickUPEmber(GameObject ember)
    {
        Debug.Log("Ember picked up");
        Destroy(ember); //Destroy the ember
        Energy += 10;
    }

    private void combineSplitFlame(GameObject originalFlame)
    {
        // Increase the energy of the original flame
        FlameController originalFlameController = originalFlame.GetComponent<FlameController>();
        //originalFlameController.energy += 30;  // You can adjust how much energy is transferred

        // Switch control back to the original flame
        flameCamera.enabled = false;  // Disable current flame's camera (mini flame)
        Camera originalFlameCamera = originalFlame.GetComponentInChildren<Camera>();
        originalFlameCamera.enabled = true;  // Enable original flame's camera

        // Re-enable the original flame's controller and disable the mini flame's controller
        originalFlameController.enabled = true;
        this.enabled = false;  // Disable the mini flame controller
        Debug.Log("Mini flame picked up");
        Energy += 30;

        // Destroy the mini flame (this game object)
        Destroy(this.gameObject);
    }

    public void ChangeColor(FlameColour fc)
    {
        Debug.Log("Changing colour " + FlameColour.GetName(fc.GetType(), fc));
        Color lightColor = fc switch
        {
            FlameColour.alluminium => AluminumColor,
            FlameColour.copper => CopperColor,
            FlameColour.iron => IronColor,
            // Shouldn't happen
            _ => Color.white,
        };
        Gradient grad = new();
        // Go from 0 opacity to 1 and back to 0
        var alphas = new GradientAlphaKey[3] {
            new (0.0f, 0.0f),
            new (1.0f, 0.3f),
            new (0.0f, 1.0f)
        };
        var colors = new GradientColorKey[2];
        // One primary flame color
        colors[0] = new GradientColorKey(lightColor, 0.3f);
        // One secondary, darker color
        colors[1] = new GradientColorKey(lightColor * 0.8f, 0.6f);
        grad.SetKeys(colors, alphas);
        flameColour = fc;
        var colAlpha = FlameAlpha.colorOverLifetime;
        colAlpha.color = grad;
        var colAdd = FlameAdd.colorOverLifetime;
        colAdd.color = grad;
        var colGlow = FlameGlow.colorOverLifetime;
        colGlow.color = grad;
        var colSparks = FlameSparks.colorOverLifetime;
        colSparks.color = grad;
        FlameLight.color = lightColor;
    }

    private void SpawnMiniflame()
    {
        if (Energy < 80)
        {
            Debug.Log("Too small to split flame");
            return;
        }

        Debug.Log("Spawning mini flame");

        // Reduce the energy of the current flame
        Energy -= 50;

        // Calculate where to spawn the new flame
        Vector3 tempPosition = new Vector3(-previousInputMovement.x*1.5f, -previousInputMovement.y * 1.5f + 1f, 0);
        Vector3 spawnPosition = transform.position + tempPosition;  // Example offset for the new miniflame

        // Spawn the new flame
        GameObject newMiniflame = Instantiate(miniflamePrefab, spawnPosition, transform.rotation);

        // Disable the camera on the current flame
        flameCamera.enabled = false;

        // Get the new flame's camera component and enable it
        Camera newFlameCamera = newMiniflame.GetComponentInChildren<Camera>();
        newFlameCamera.enabled = true;

        Rigidbody2D miniFlameBody = newMiniflame.GetComponent<Rigidbody2D>();
        miniFlameBody.linearVelocity = Vector2.zero;
        miniFlameBody.angularVelocity = 0f;

        Rigidbody2D mainFlameBody = this.gameObject.GetComponent<Rigidbody2D>();
        mainFlameBody.linearVelocity = Vector2.zero;
        mainFlameBody.angularVelocity = 0f;

        // Optionally, transfer control to the new flame by disabling current FlameController and enabling it on the new flame
        FlameController newFlameController = newMiniflame.GetComponent<FlameController>();
        newFlameController.enabled = true;
        newFlameController.flameColour = flameColour;


        newFlameController.ChangeColor(flameColour);


        this.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(4.84f, 4.84f);
        this.enabled = false;  // Disable the current flame's controller
    }
}
