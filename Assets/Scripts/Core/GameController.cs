//This script keeps information that will be needed by all the scenes and is used to keep track of completed levels
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    private List<int> completedLevels = new List<int>{}; //Will store the completed levels and send them to the menu controller
    private int maxLevel = 7; //change as needed

    public int currentLevelIndex = 0;

    private int currentLevel = 0;

    public bool pauseMenuOpen = false;

    public GameObject gameUIPrefab;

    private GameObject GameControllerObj;
    private GameObject mainFlameObj;

    private FlameType flameState;

    public void changeFlameState(){
        // Assuming this script is on the Grid object
        TilemapCollider2D crackedWalls = transform.Find("CrackedWalls").GetComponent<TilemapCollider2D>();
        if (flameState == FlameType.mainFlame){
            flameState = FlameType.miniFlame;
            crackedWalls.enabled = false;
        }
        else {
            flameState = FlameType.mainFlame;
            crackedWalls.enabled = true;
        }
    }

    private FlameController _flameController;
    private GameUIController _gameUIController;
    private SoundManager _soundManager;
    private string saveFilePath;

    private float levelStartTime;
    private float remainingTime;
    private double remainingEnergy;


    // Start is called before the first frame update
    void Start()
    {
        GameControllerObj = GameObject.Find("GameController");

        //TODO
        flameState = FlameType.mainFlame;

        DontDestroyOnLoad(GameControllerObj);

        if (_soundManager == null)
            _soundManager = FindAnyObjectByType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentLevel != 0 && !pauseMenuOpen) //If we are not on the main menu and pause menu is not open
            {
                //DontDestroyOnLoad(GameControllerObj);
                SceneManager.LoadScene("Pause", LoadSceneMode.Additive); //Change to the level selected
                pauseMenuOpen = true;
            }
            
        }
    }

    private void updateCompletedLevels()
    {
        saveFilePath = Path.Combine(Application.dataPath, "SaveData/levels.txt");
        if (File.Exists(saveFilePath))
        {
            string levelsCompletedData = File.ReadAllText(saveFilePath);
            completedLevels = ParseCompletedLevels(levelsCompletedData);
        }
        else
        {
            Debug.LogWarning("levels.txt not found!");
        }
    }

    public void setMainFlame(GameObject flame)  //set a reference to the main flame
    {
        mainFlameObj = flame;
    }

    public void miniFlameDead()
    {
        FlameController newFlameController = mainFlameObj.GetComponent<FlameController>();
        newFlameController.enabled = true;

        Camera newFlameCamera = mainFlameObj.GetComponentInChildren<Camera>();
        newFlameCamera.enabled = true;
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

    public List<int> getCompletedLevels()
    {
        updateCompletedLevels();
        return completedLevels;
    }

    public int getCurrentLevel()
    {
        return currentLevel;
    }

    public void setCurrentLevel(int level)
    {
        currentLevel = level;
        Debug.Log("Current level: " + currentLevel);
    }

    public void levelLoaded(int level)
    {
        pauseMenuOpen = false;
        Debug.Log("Attempting to load scene: " + level);
        StartCoroutine(WaitForSceneLoad(level));
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

    IEnumerator WaitForSceneLoad(int level)
    {
        LevelLoaderController _levelLoaderController = FindAnyObjectByType<LevelLoaderController>();
        _levelLoaderController.playAnimation();

        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad;
        GameObject gameUIInstance;

        switch(level)
        {
            case 1: 
                Debug.Log("Scene '" + level + "' busy loading");
                asyncLoad = SceneManager.LoadSceneAsync("Level_1");
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                Debug.Log("Scene '" + level + "' loaded successfully.");
                gameUIInstance = Instantiate(gameUIPrefab);

                //Unique code:
                //GameUIController gameUIController = gameUIInstance.GetComponent<GameUIController>();
                _gameUIController = FindAnyObjectByType<GameUIController>();

                //Set timer to 10 minutes (600 seconds)
                _gameUIController.SetTimer(180f);  // 10 minutes in seconds
                levelStartTime = 180f;

                _flameController = FindAnyObjectByType<FlameController>();
                _flameController.SetStartEnergy(500);
                currentLevel = level;
                
                break;
            case 2: 
                Debug.Log("Scene '" + level + "' busy loading");
                asyncLoad = SceneManager.LoadSceneAsync("Level_2");
                while (!asyncLoad.isDone)
                {
                    Debug.Log("Async not done");
                    yield return null;
                }
                Debug.Log("Scene '" + level + "' loaded successfully.");
                gameUIInstance = Instantiate(gameUIPrefab);

                //Unique code:
                //GameUIController gameUIController = gameUIInstance.GetComponent<GameUIController>();
                _gameUIController = FindAnyObjectByType<GameUIController>();

                //Set timer to 10 minutes (600 seconds)
                _gameUIController.SetTimer(300f);  // 10 minutes in seconds
                levelStartTime = 300f;

                _flameController = FindAnyObjectByType<FlameController>();
                _flameController.SetStartEnergy(500);
                currentLevel = level;
                
                break;
                case 3: 
                Debug.Log("Scene '" + level + "' busy loading");
                asyncLoad = SceneManager.LoadSceneAsync("Level_3");
                while (!asyncLoad.isDone)
                {
                    Debug.Log("Async not done");
                    yield return null;
                }
                Debug.Log("Scene '" + level + "' loaded successfully.");
                gameUIInstance = Instantiate(gameUIPrefab);

                //Unique code:
                //GameUIController gameUIController = gameUIInstance.GetComponent<GameUIController>();
                _gameUIController = FindAnyObjectByType<GameUIController>();

                //Set timer to 10 minutes (600 seconds)
                _gameUIController.SetTimer(300f);  // 10 minutes in seconds
                levelStartTime = 300f;

                _flameController = FindAnyObjectByType<FlameController>();
                _flameController.SetStartEnergy(750);
                currentLevel = level;
                
                break;
                case 4: 
                Debug.Log("Scene '" + level + "' busy loading");
                asyncLoad = SceneManager.LoadSceneAsync("Level_4");
                while (!asyncLoad.isDone)
                {
                    Debug.Log("Async not done");
                    yield return null;
                }
                Debug.Log("Scene '" + level + "' loaded successfully.");
                gameUIInstance = Instantiate(gameUIPrefab);

                //Unique code:
                //GameUIController gameUIController = gameUIInstance.GetComponent<GameUIController>();
                _gameUIController = FindAnyObjectByType<GameUIController>();

                //Set timer to 10 minutes (600 seconds)
                _gameUIController.SetTimer(350f);  // 10 minutes in seconds
                levelStartTime = 350f;

                _flameController = FindAnyObjectByType<FlameController>();
                _flameController.SetStartEnergy(1000);
                currentLevel = level;
                
                break;
                case 5: 
                Debug.Log("Scene '" + level + "' busy loading");
                asyncLoad = SceneManager.LoadSceneAsync("Level_5");
                while (!asyncLoad.isDone)
                {
                    Debug.Log("Async not done");
                    yield return null;
                }
                Debug.Log("Scene '" + level + "' loaded successfully.");
                gameUIInstance = Instantiate(gameUIPrefab);

                //Unique code:
                //GameUIController gameUIController = gameUIInstance.GetComponent<GameUIController>();
                _gameUIController = FindAnyObjectByType<GameUIController>();

                //Set timer to 10 minutes (600 seconds)
                _gameUIController.SetTimer(450f);  // 10 minutes in seconds
                levelStartTime = 450f;

                _flameController = FindAnyObjectByType<FlameController>();
                _flameController.SetStartEnergy(1000);
                currentLevel = level;
                
                break;
                case 6: 
                Debug.Log("Scene '" + level + "' busy loading");
                asyncLoad = SceneManager.LoadSceneAsync("Level_6");
                while (!asyncLoad.isDone)
                {
                    Debug.Log("Async not done");
                    yield return null;
                }
                Debug.Log("Scene '" + level + "' loaded successfully.");
                gameUIInstance = Instantiate(gameUIPrefab);

                //Unique code:
                //GameUIController gameUIController = gameUIInstance.GetComponent<GameUIController>();
                _gameUIController = FindAnyObjectByType<GameUIController>();

                //Set timer to 10 minutes (600 seconds)
                _gameUIController.SetTimer(750f);  // 10 minutes in seconds
                levelStartTime = 750f;

                _flameController = FindAnyObjectByType<FlameController>();
                _flameController.SetStartEnergy(1100);
                currentLevel = level;
                
                break;
                case 7: 
                Debug.Log("Scene '" + level + "' busy loading");
                asyncLoad = SceneManager.LoadSceneAsync("Level_7");
                while (!asyncLoad.isDone)
                {
                    Debug.Log("Async not done");
                    yield return null;
                }
                Debug.Log("Scene '" + level + "' loaded successfully.");
                gameUIInstance = Instantiate(gameUIPrefab);

                //Unique code:
                //GameUIController gameUIController = gameUIInstance.GetComponent<GameUIController>();
                _gameUIController = FindAnyObjectByType<GameUIController>();

                //Set timer to 10 minutes (600 seconds)
                _gameUIController.SetTimer(900);  // 10 minutes in seconds
                levelStartTime = 900;

                _flameController = FindAnyObjectByType<FlameController>();
                _flameController.SetStartEnergy(1300);
                currentLevel = level;
                
                break;
            default:
                break;
        }
        
        _soundManager.PlayBackgroundMusic("Audio/BackgroundMusic/LevelBackground", 0.033f);
    }

    public double getEnergySpent()
    {
        return _flameController.RemainingEnergy();
    }

    public float getTimeTaken()
    {
        return remainingTime;
    }

    private void calculateStats()  //Calculate stats before GameUI is destroyed
    {
        remainingEnergy = _flameController.RemainingEnergy();
        remainingTime = levelStartTime - _gameUIController.getCurrentTime();
    }

    public void timeout()
    {
        
        _soundManager.StopMovementSound();
        _soundManager.StopBackgroundMusic();
        Debug.Log("Time is up");
        StartCoroutine(LevelTransition("Gameover"));
        //SceneManager.LoadScene("Gameover"); //Change to the level selected
        pauseMenuOpen = true;

        //GameObject flame = GameObject.Find("Flame");
        //flame.SetActive(false);
        _soundManager.PlayBackgroundMusic("Audio/BackgroundMusic/GameOver", 0.138f);
    }

    public void waterDeath()
    {
        
        _soundManager.StopMovementSound();
        _soundManager.StopBackgroundMusic();
        Debug.Log("Time is up");
        StartCoroutine(LevelTransition("Gameover"));
        //SceneManager.LoadScene("Gameover"); //Change to the level selected
        pauseMenuOpen = true;

        //GameObject flame = GameObject.Find("Flame");
        //flame.SetActive(false);
        _soundManager.PlayBackgroundMusic("Audio/BackgroundMusic/GameOver", 0.138f);
    }

    public void openMainMenu()
    {
        _soundManager.StopMovementSound();
        _soundManager.StopBackgroundMusic();
        //DontDestroyOnLoad(GameControllerObj);
        StartCoroutine(LevelTransition("Menu"));
        //SceneManager.LoadScene(0, LoadSceneMode.Single);
        Debug.Log("Loading Main Menu");
        resetStats();
        _soundManager.PlayBackgroundMusic("Audio/BackgroundMusic/MenuBackground", 0.2f);
    }

    public void retryLevel()
    {
        _soundManager.StopMovementSound();
        _soundManager.StopBackgroundMusic();
        //DontDestroyOnLoad(GameControllerObj);
        Debug.Log("Current Level: " + currentLevel);
        //LevelTransition();
        levelLoaded(currentLevel);
    }

    public void finishLevel()
    {
        _soundManager.StopMovementSound();
        calculateStats(); //Must be called before new scene is loaded and gameUI is destroyed
        _soundManager.StopBackgroundMusic();
        Debug.Log("Level Complete");

        
        _gameUIController.setTimerRunning(false);
        StartCoroutine(LevelTransition("LevelComplete"));
        //SceneManager.LoadScene("LevelComplete"); //Change to the level selected
        pauseMenuOpen = true;

        //GameObject flame = GameObject.Find("Flame");
        //flame.SetActive(false);

        _soundManager.PlayBackgroundMusic("Audio/BackgroundMusic/LevelComplete1", 0.25f);

        /*if currentLevel in completedLevels do nothing. Else add current level + ";" to text file of levels.
        if (!(completedLevels.Contains(currentLevel)))
        {
            
        }
        */

        // Check if currentLevel is in completedLevels
        if (!completedLevels.Contains(currentLevel))
        {
            // Append currentLevel to levels.txt
            AppendLevelToSaveFile(currentLevel);
            
            // Add currentLevel to the completedLevels list
            completedLevels.Add(currentLevel);
        }
    }

    private void AppendLevelToSaveFile(int level)
    {
        saveFilePath = Path.Combine(Application.dataPath, "SaveData/levels.txt");

        if (File.Exists(saveFilePath))
        {
            // Append the current level followed by ";" to the first line of the file
            using (StreamWriter writer = new StreamWriter(saveFilePath, append: true))
            {
                writer.Write(level + ";");
            }
            Debug.Log("Level " + level + " added to levels.txt.");
        }
        else
        {
            Debug.LogWarning("levels.txt not found!");
        }
    }

    public void startNextLevel()
    {
        _soundManager.StopBackgroundMusic();
        if (currentLevel + 1 <= maxLevel)
        {
            Debug.Log("Loading level " + currentLevel + 1);
            levelLoaded(currentLevel + 1); //Would actually be levelLoaded(currentLevel + 1);
        }
        else{
            openMainMenu();
        }
    }

    private void resetStats()
    {
        currentLevel = 0;
        currentLevelIndex = 0;
        pauseMenuOpen = false;
        levelStartTime = 0f;
    }

    
    

    //Should always listen for 'esc` to pause a game
}