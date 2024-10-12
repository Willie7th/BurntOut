using UnityEngine;

public class MetalWall : MonoBehaviour
{
    public Sprite solidSprite;
    public Sprite[] meltingSprites;
    public Sprite meltedSprite;
    public float meltingDuration = 3.0f;

    public FlameColour metalType;

    private SpriteRenderer spriteRenderer;
    private float meltingProgress = 0f;
    private bool isMelting = false;
    private int currentMeltingFrame = 0;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = solidSprite;
    }

    void Update()
    {
        if (isMelting)
        {
            meltingProgress += Time.deltaTime;
            float progressRatio = meltingProgress / meltingDuration;
            int frameIndex = Mathf.FloorToInt(progressRatio * (meltingSprites.Length-1));
            if (frameIndex != currentMeltingFrame)
            {
                currentMeltingFrame = frameIndex;
                spriteRenderer.sprite = meltingSprites[frameIndex];
            }

            if (meltingProgress >= meltingDuration)
            {
                spriteRenderer.sprite = meltedSprite;
                isMelting = false;
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isMelting)
        {
            FlameController flameController = collision.gameObject.GetComponent<FlameController>();
            Debug.Log(metalType+" : " +flameController.flameColour);
            if (flameController != null && flameController.flameColour == metalType){
                // flameController.fla
                StartMelting();
            }

            //SUBTRACT FLAME HEATLH
            // FlameController flame = collision.gameObject.GetComponent<Flame>();
            // if (flame != null)
            // {
            //     // flame.ReduceHealth(damageAmount);
            // }
        }
    }

    void StartMelting()
    {
        isMelting = true;
    }
}
