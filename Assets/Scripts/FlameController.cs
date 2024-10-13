using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;


public class FlameController : MonoBehaviour
{
    public float speed = 5f;  //maybe add a sprint mechanic
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
    private SoundManager _soundManager;

    public GameObject miniflamePrefab;
    private Camera flameCamera;

    private bool isMoving = false;
    public bool mainFlame;

    private bool isJumping = false;
    private Vector2 newTarget;

    [SerializeField] private AudioClip flameMoveAudio;

    public TextMeshProUGUI energyLabel;

    public double energy = 1000;  //Some stage make private set but public get

    // public string flameType = "default";

    public FlameType flameType = FlameType.mainFlame;
    public FlameColour flameColour = FlameColour.iron;
    private readonly Dictionary<FlameColour, Color> lightColorTable = new();
    public Color AluminumColor = new(0.14f, 0.76f, 0.92f, 1.0f);
    public Color CopperColor = new(0.15f, 0.6f, 0.08f, 1.0f);
    public Color IronColor = new(1.0f, 0.68f, 0.125f, 1.0f);

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

        // Add flameColor - lightColor mapping
        lightColorTable.Add(FlameColour.alluminium, AluminumColor);
        lightColorTable.Add(FlameColour.copper, CopperColor);
        lightColorTable.Add(FlameColour.iron, IronColor);
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
        if(isJumping)
        {
            characterBody.MovePosition(newTarget);
            
            isJumping = false;
        }
        else{
            Vector2 delta = inputMovement * velocity * Time.deltaTime;
            Vector2 newPosition = characterBody.position + delta;
            characterBody.MovePosition(newPosition);
        }
        
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
            changeColour(FlameColour.alluminium);
        }
        else if (colName == "IronOre")
        {
            changeColour(FlameColour.iron);
        }
        else if (colName == "CopperOre")
        {
            changeColour(FlameColour.copper);
        }
        else if (colName == "Finish")
        {
            _gameController.finishLevel();
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
        else if(colName == "Water")
        {
            Debug.Log("Player Dies");
        }

    }

    public void meltFlame()
    {

    }

    private void flameJump()
    {
        if (energy < 30)
        {
            Debug.Log("Too small to jump");
            return;
        }

        isJumping = true;

        
        Vector2 offset = previousInputMovement * 2f;
        newTarget = characterBody.position + offset;
        //Debug.Log("New position " + characterBody.position);
        //this.gameObject.transform.position = 
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
        //Debug.Log(previousInputMovement);
        GameObject emberInstance = Instantiate(emberPrefab, this.gameObject.transform.position + offset, this.gameObject.transform.rotation);  //drop ember at player position
        emberInstance.name = "Ember";
    }

    private void pickUPEmber(GameObject ember)
    {
        Debug.Log("Ember picked up");
        Destroy(ember); //Destroy the ember
        energy = energy + 10;
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
        energy = energy + 30;

        // Destroy the mini flame (this game object)
        Destroy(this.gameObject);
    }

    private void changeColour(FlameColour flameColor)
    {
        Debug.Log("Changing colour " + FlameColour.GetName(flameColor.GetType(), flameColor));
        Color lightColor = lightColorTable[flameColour];
        Gradient grad = new ();
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
        this.flameColour = flameColor;
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

    public void setFlameEnergy(double inEnergy)
    {
        energy = inEnergy;
    }

    public double getFlameEnergy()
    {
        int tempEnergy = (int)energy;
        return tempEnergy;
    }

    private void SpawnMiniflame()
    {
        if (energy < 80)
        {
            Debug.Log("Too small to split flame");
            return;
        }

        Debug.Log("Spawning mini flame");

        // Reduce the energy of the current flame
        energy -= 50;

        // Calculate where to spawn the new flame
        Vector3 spawnPosition = transform.position + new Vector3(1.0f, 0.0f, 0.0f);  // Example offset for the new miniflame

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
        this.enabled = false;  // Disable the current flame's controller
    }
}
