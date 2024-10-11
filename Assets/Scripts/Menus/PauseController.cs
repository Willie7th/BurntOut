using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public TextMeshProUGUI levelLabel;

    private GameController _gameController;
    private GameUIController _gameUIController;
    private SoundManager _soundManager;

    [SerializeField] private AudioClip buttonClickAudio;
    [SerializeField] private AudioClip retryClickAudio;

    // Start is called before the first frame update
    void Start()
    {
        if (_gameController == null)
            _gameController = FindObjectOfType<GameController>();

        if (_gameUIController == null)
            _gameUIController = FindObjectOfType<GameUIController>();

        if (_soundManager == null)
            _soundManager = FindObjectOfType<SoundManager>();

        string label = "Paused - Level " + _gameController.getCurrentLevel();
        levelLabel.text = label;
        _gameUIController.setTimerRunning(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resumeButton()
    {
        Debug.Log("Resume button clicked");
         _soundManager.PlaySound(buttonClickAudio, 0.5f);
        SceneManager.UnloadSceneAsync(1);
        _gameController.pauseMenuOpen = false;
        _gameUIController.setTimerRunning(true);
    }

    public void retryButton()
    {
        Debug.Log("Retry button clicked");
        /*
        _gameController.pauseMenuOpen = false;
        SceneManager.UnloadSceneAsync(_gameController.currentLevelIndex);
        Debug.Log("Current Level: " + _gameController.currentLevelIndex);
        _gameController.levelLoaded(_gameController.currentLevelIndex);
        //SceneManager.LoadScene(_gameController.currentLevelIndex, LoadSceneMode.Single);
        _gameUIController.setTimerRunning(true);
        */
        _soundManager.PlaySound(retryClickAudio, 0.5f);
        _gameController.retryLevel();
    }

    public void MenuButton()
    {
        Debug.Log("Menu button clicked");
        _soundManager.PlaySound(buttonClickAudio, 0.5f);
        /*
        DontDestroyOnLoad(GameControllerObj);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Debug.Log("Loading Main Menu");
        _gameController.pauseMenuOpen = false;
        _gameUIController.setTimerRunning(true);
        */
        _gameController.openMainMenu();
    }
}
