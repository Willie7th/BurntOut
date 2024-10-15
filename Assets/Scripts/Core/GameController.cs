//This script keeps information that will be needed by all the scenes and is used to keep track of completed levels
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStats {

    public double remainingEnergy;
    public float remainingTime;
    
}

public class GameController : MonoBehaviour
{
    public List<Level> levels;
    private Level currentLevel = null;
    public GameStats stats;
    public bool pauseMenuOpen = false;
    public GameObject gameUIPrefab;
    private GameObject GameControllerObj;
    private FlameController _flameController;
    private GameUIController _gameUIController;
    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {
        GameControllerObj = GameObject.Find("GameController");
        DontDestroyOnLoad(GameControllerObj);
        if (_soundManager == null)
            _soundManager = FindAnyObjectByType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentLevel != null && !pauseMenuOpen) //If we are not on the main menu and pause menu is not open
            {
                //DontDestroyOnLoad(GameControllerObj);
                SceneManager.LoadScene("Pause", LoadSceneMode.Additive); //Change to the level selected
                pauseMenuOpen = true;
            }
        }
    }

    public List<Level> GetCompletedLevels()
    {
        return levels.Where(lvl => lvl.complete).ToList();
    }

    public int GetCurrentLevelIndex()
    {
        if (currentLevel == null) return -1;
        return currentLevel.index;
    }

    public int GetMaxLevelIndex()
    {
        return levels.Count + 1;
    }

    public void LoadLevel(int level)
    {
        LoadLevel(GetLevel(level));
    }

    public void LoadLevel(Level level)
    {
        pauseMenuOpen = false;
        Debug.Log("Loading level " + level.index);
        StartCoroutine(WaitForSceneLoad(level));
        currentLevel = level;
    }

    IEnumerator LevelTransition(string sceneName)
    {
        Debug.Log("Level transition attempted");
        LevelLoaderController _levelLoaderController = FindAnyObjectByType<LevelLoaderController>();
        _levelLoaderController.playAnimation();
        Debug.Log("Played animation");
        yield return new WaitForSeconds(1f);
        Debug.Log("Trying to load scene");
        SceneManager.LoadScene(sceneName);
        Debug.Log("Scene loaded");
    }

    IEnumerator WaitForSceneLoad(Level level)
    {
        Debug.Log("Scene '" + level.name + "' busy loading");
        LevelLoaderController _levelLoaderController = FindAnyObjectByType<LevelLoaderController>();
        _levelLoaderController.playAnimation();
        yield return new WaitForSeconds(1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level.level_name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        _gameUIController = FindAnyObjectByType<GameUIController>();
        _gameUIController.SetTimer((float)level.time * 60);
        _flameController = FindAnyObjectByType<FlameController>();
        _flameController.SetStartEnergy(level.startEnergy);
        _soundManager.PlayBackgroundMusic("Audio/BackgroundMusic/LevelBackground", 0.033f);
        Debug.Log("Scene '" + level.name + "' loaded successfully.");
    }

    public Level GetLevel(int levelIndex)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].index == levelIndex) return levels[i];
        }
        return null;
    }

    private GameStats CalculateStats()
    {
        return new GameStats {
            remainingEnergy =_flameController.RemainingEnergy(),
            remainingTime = currentLevel.time - _gameUIController.getCurrentTime()
        };
    }

    public void GameOver(string gameOverMessage)
    {
        _soundManager.StopMovementSound();
        _soundManager.StopBackgroundMusic();
        Debug.Log(gameOverMessage);
        StartCoroutine(LevelTransition("Gameover"));
        pauseMenuOpen = true;
        _soundManager.PlayBackgroundMusic("Audio/BackgroundMusic/GameOver", 0.138f);
    }

    public void OpenMainMenu()
    {
        Debug.Log("Loading Main Menu");
        _soundManager.StopMovementSound();
        _soundManager.StopBackgroundMusic();
        StartCoroutine(LevelTransition("Menu"));
        pauseMenuOpen = false;
        currentLevel = null;
        _soundManager.PlayBackgroundMusic("Audio/BackgroundMusic/MenuBackground", 0.2f);
    }

    public void RetryLevel()
    {
        _soundManager.StopMovementSound();
        _soundManager.StopBackgroundMusic();
        LoadLevel(currentLevel);
    }

    public void FinishLevel()
    {
        _soundManager.StopMovementSound();
        _soundManager.StopBackgroundMusic();
        _soundManager.PlayBackgroundMusic("Audio/BackgroundMusic/LevelComplete1", 0.25f);
        stats = CalculateStats(); //Must be called before new scene is loaded and timer is destroyed
        _gameUIController.setTimerRunning(false);
        StartCoroutine(LevelTransition("LevelComplete"));
        pauseMenuOpen = true;
        currentLevel.complete = true;
        Debug.Log("Level Complete");
    }

    public void LoadNextLevel()
    {
        int nextLevelIndex = GetCurrentLevelIndex() + 1;
        _soundManager.StopBackgroundMusic();
        if (nextLevelIndex <= GetMaxLevelIndex())
        {
            LoadLevel(nextLevelIndex); //Would actually be levelLoaded(currentLevel + 1);
        }
        else
        {
            OpenMainMenu();
        }
    }
}
