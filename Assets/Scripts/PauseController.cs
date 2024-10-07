using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public TextMeshProUGUI levelLabel;

    private GameController _gameController;
    private GameObject GameControllerObj;

    // Start is called before the first frame update
    void Start()
    {
        if (_gameController == null)
            _gameController = FindObjectOfType<GameController>();

        GameControllerObj = GameObject.Find("GameController");
        string label = "Paused - Level " + _gameController.getCurrentLevel();
        levelLabel.text = label;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resumeButton()
    {
        Debug.Log("Resume button clicked");
        SceneManager.UnloadSceneAsync(1);
        _gameController.pauseMenuOpen = false;
    }

    public void retryButton()
    {
        Debug.Log("Retry button clicked");
        _gameController.pauseMenuOpen = false;
        SceneManager.UnloadSceneAsync(_gameController.currentLevelIndex);
        SceneManager.LoadScene(_gameController.currentLevelIndex, LoadSceneMode.Single);
    }

    public void MenuButton()
    {
        Debug.Log("Menu button clicked");
        DontDestroyOnLoad(GameControllerObj);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Debug.Log("Loading Main Menu");
        _gameController.pauseMenuOpen = false;
    }
}
