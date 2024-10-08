//This script keeps information that will be needed by all the scenes and is used to keep track of completed levels
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private List<int> levelsComplete = new List<int>{};  //Will store the completed levels and send them to the menu controller

    public int currentLevelIndex = 0;

    private int currentLevel = 0;

    public bool pauseMenuOpen = false;

    public GameObject gameUIPrefab;

    private GameObject GameControllerObj;

    private FlameController _flameController;


    // Start is called before the first frame update
    void Start()
    {
        GameControllerObj = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentLevel != 0 && !pauseMenuOpen) //If we are not on the main menu and pause menu is not open
            {
                DontDestroyOnLoad(GameControllerObj);
                SceneManager.LoadScene("Pause", LoadSceneMode.Additive); //Change to the level selected
                pauseMenuOpen = true;
            }
            
        }
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
        Debug.Log("Attempting to load scene: " + level);
        StartCoroutine(WaitForSceneLoad(level));
    }

    IEnumerator WaitForSceneLoad(int level)
    {
        switch(level)
        {
            case 1: 
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                Debug.Log("Scene '" + level + "' loaded successfully.");
                GameObject gameUIInstance = Instantiate(gameUIPrefab);
                currentLevel = 1;

                //Unique code:
                GameUIController gameUIController = gameUIInstance.GetComponent<GameUIController>();

                //Set timer to 10 minutes (600 seconds)
                gameUIController.SetTimer(300f);  // 10 minutes in seconds

                _flameController = FindObjectOfType<FlameController>();
                _flameController.setFlameEnergy(500);
                
                break;
            default:
                break;
        }
    }

    public void timeout()
    {
        Debug.Log("Time is up");
    }
    

    //Should always listen for 'esc` to pause a game
}
