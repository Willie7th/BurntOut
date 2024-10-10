using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private GameController _gameController;
    private GameObject GameControllerObj;
    private SoundManager _soundManager;
    private GameObject SoundManagerObj;

    [SerializeField] private AudioClip buttonClickAudio;
    [SerializeField] private AudioClip startClickAudio;
    [SerializeField] private AudioClip flameIconAudio;

    public Sprite flameSprite;
    public float delayBetweenFlameChanges = 0.5f;
    public List<Button> levelButtons;
    private string saveFilePath;
    public Button startButton;

    private int selectedLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (_gameController == null)
            _gameController = FindObjectOfType<GameController>();
        _gameController.setCurrentLevel(0);

        if (_soundManager == null)
            _soundManager = FindObjectOfType<SoundManager>();

        GameControllerObj = GameObject.Find("GameController");
        SoundManagerObj = GameObject.Find("SoundManager");

        saveFilePath = Path.Combine(Application.dataPath, "SaveData/levels.txt");

        if (File.Exists(saveFilePath))
        {
            string levelsCompletedData = File.ReadAllText(saveFilePath);
            List<int> completedLevels = ParseCompletedLevels(levelsCompletedData);

            int maxInteractableLevel = Mathf.Min(completedLevels.Count + 1, 7); // Current level is the next after completed ones

            // Make buttons interactable for completed and current levels
            for (int i = 0; i < levelButtons.Count; i++)
            {
                levelButtons[i].interactable = (i + 1) <= maxInteractableLevel;
            }

            // Start the coroutine to tick off completed levels one by one
            StartCoroutine(ChangeFlameSprites(completedLevels));
        }
        else
        {
            Debug.LogWarning("levels.txt not found!");
        }
    }

    private IEnumerator ChangeFlameSprites(List<int> completedLevels)
    {
        foreach (int level in completedLevels)
        {
            yield return new WaitForSeconds(delayBetweenFlameChanges);
            
            Button button = levelButtons[level - 1]; // Get the button for the completed level
            Image buttonImage = button.GetComponent<Image>();

            if (buttonImage != null && flameSprite != null)
            {
                _soundManager.PlaySound(flameIconAudio, 0.75f);
                buttonImage.sprite = flameSprite;
            }

            // Wait for the specified delay before proceeding to the next one
            
        }
    }

    private List<int> ParseCompletedLevels(string data)
    {
        List<int> completedLevels = new List<int>();

        string[] levelStrings = data.Split(';');
        foreach (string levelStr in levelStrings)
        {
            if (int.TryParse(levelStr, out int level))
            {
                completedLevels.Add(level);
            }
        }

        return completedLevels;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startButtonClicked()
    {
        if(selectedLevel > 0)
        {
            _soundManager.PlaySound(startClickAudio, 1f);
             DontDestroyOnLoad(GameControllerObj);
             DontDestroyOnLoad(SoundManagerObj);
            //_gameController.setCurrentLevel(1);
            _gameController.currentLevelIndex = selectedLevel;
            //SceneManager.LoadScene("Level1"); //Change to the level selected
            _gameController.levelLoaded(selectedLevel);
        }
       
    }

    public void levelClicked(int level)
    {
        _soundManager.PlaySound(buttonClickAudio, 0.5f); //Play sound at half volume
        selectedLevel = level;
        startButton.GetComponentInChildren<TextMeshProUGUI>().text = "START LEVEL - " + level;
    }

}
