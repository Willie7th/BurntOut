using UnityEngine;

public class MetalWall : MonoBehaviour
{
    public Sprite solidSprite;
    public Sprite[] meltingSprites;
    public Sprite meltedSprite;
    public float meltingDuration = 3.0f;

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
            int frameIndex = Mathf.FloorToInt(progressRatio * meltingSprites.Length);

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
        // if (collision.gameObject.CompareTag("Player") && !isMelting)
        // {
            StartMelting();
        // }
    }

    void StartMelting()
    {
        isMelting = true;
    }
}
