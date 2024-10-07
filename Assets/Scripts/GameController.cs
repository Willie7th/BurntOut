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

    private GameObject GameControllerObj;

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
    

    //Should always listen for 'esc` to pause a game
}
