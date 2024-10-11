using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    private GameController _gameController;
    private SoundManager _soundManager;

    [SerializeField] private AudioClip buttonClickAudio;
    [SerializeField] private AudioClip retryClickAudio;
    //private GameObject GameControllerObj;

    // Start is called before the first frame update
    void Start()
    {
        if (_gameController == null)
            _gameController = FindAnyObjectByType<GameController>();

        if (_soundManager == null)
            _soundManager = FindAnyObjectByType<SoundManager>();

        //GameControllerObj = GameObject.Find("GameController");

        _gameController.pauseMenuOpen = true;  //Just so pause menu can't be activated
    }

    public void retryButton()
    {
        /*
        DontDestroyOnLoad(GameControllerObj);
        Debug.Log("Retry button clicked");
        Debug.Log("Current Level: " + _gameController.currentLevelIndex);
        _gameController.levelLoaded(_gameController.currentLevelIndex);
        //SceneManager.LoadScene(_gameController.currentLevelIndex, LoadSceneMode.Single);
        //_gameController.pauseMenuOpen = true;  //So pause menu can be activated
        */
        _soundManager.PlaySound(retryClickAudio, 0.5f);
        Debug.Log("Retry button clicked");
        _gameController.retryLevel();
    }

    public void MenuButton()
    {
        /*
        Debug.Log("Menu button clicked");
        DontDestroyOnLoad(GameControllerObj);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Debug.Log("Loading Main Menu");
        //_gameController.pauseMenuOpen = false;
        */
        _soundManager.PlaySound(buttonClickAudio, 0.5f);
        Debug.Log("Menu button clicked");
        _gameController.openMainMenu();
    }
}
