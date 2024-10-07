using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private GameController _gameController;
    private GameObject GameControllerObj;

    // Start is called before the first frame update
    void Start()
    {
        if (_gameController == null)
            _gameController = FindObjectOfType<GameController>();
        _gameController.setCurrentLevel(0);

        GameControllerObj = GameObject.Find("GameController");
        showCompletedLevels();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startButtonClicked()
    {
        DontDestroyOnLoad(GameControllerObj);
        _gameController.setCurrentLevel(1);
        _gameController.currentLevelIndex = 2;
        SceneManager.LoadScene("Level1"); //Change to the level selected
    }

    private void showCompletedLevels()
    {
        //Highlight the levels that have been selected
    }
}
