using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private GameController _gameController;
    private SoundManager _soundManager;

    [SerializeField] private AudioClip buttonClickAudio;
    [SerializeField] private AudioClip startClickAudio;
    [SerializeField] private AudioClip flameIconAudio;

    public Sprite flameSprite;
    public float delayBetweenFlameChanges = 0.5f;
    public List<Button> levelButtons;
    public Button startButton;

    private int selectedLevel = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (_gameController == null)
            _gameController = FindAnyObjectByType<GameController>();
        if (_soundManager == null)
            _soundManager = FindAnyObjectByType<SoundManager>();
        LoadLevels();
        _soundManager.PlayBackgroundMusic("Audio/BackgroundMusic/MenuBackground", 0.1f);
    }

    private void LoadLevels()
    {
        List<Level> completedLevels = _gameController.GetCompletedLevels();

        int maxInteractableLevel = Mathf.Min(completedLevels.Count + 1, 7); // Current level is the next after completed ones

        // Make buttons interactable for completed and current levels
        for (int i = 0; i < levelButtons.Count; i++)
        {
            levelButtons[i].interactable = (i + 1) <= maxInteractableLevel;
        }
        // Start the coroutine to tick off completed levels one by one
        StartCoroutine(ChangeFlameSprites(completedLevels));
    }

    private IEnumerator ChangeFlameSprites(List<Level> completedLevels)
    {
        foreach (Level level in completedLevels)
        {
            yield return new WaitForSeconds(delayBetweenFlameChanges);

            Button button = levelButtons[level.index]; // Get the button for the completed level
            Image buttonImage = button.GetComponent<Image>();

            if (buttonImage != null && flameSprite != null)
            {
                _soundManager.PlaySound(flameIconAudio, 0.75f);
                buttonImage.sprite = flameSprite;
            }
        }
    }

    public void StartButtonClicked()
    {
        if (selectedLevel >= 0)
        {
            _soundManager.PlaySound(startClickAudio, 1f);
            _gameController.LoadLevel(selectedLevel);
        }
    }

    public void LevelButtonClicked(int level)
    {
        _soundManager.PlaySound(buttonClickAudio, 0.5f); //Play sound at half volume
        selectedLevel = level-1;  // Level-indices are 0-based
        startButton.GetComponentInChildren<TextMeshProUGUI>().text = "START LEVEL " + level;
    }
}
