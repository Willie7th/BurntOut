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
            _gameController = FindAnyObjectByType<GameController>();

        if (_gameUIController == null)
            _gameUIController = FindAnyObjectByType<GameUIController>();

        if (_soundManager == null)
            _soundManager = FindAnyObjectByType<SoundManager>();

        string label = "Paused - Level " + _gameController.GetCurrentLevelIndex();
        levelLabel.text = label;
        _gameUIController.setTimerRunning(false);

    }

    public void ResumeButton()
    {
        Debug.Log("Resume button clicked");
         _soundManager.PlaySound(buttonClickAudio, 0.5f);
        SceneManager.UnloadSceneAsync(1);
        _gameController.pauseMenuOpen = false;
        _gameUIController.setTimerRunning(true);
    }

    public void RetryButton()
    {
        Debug.Log("Retry button clicked");
        _soundManager.PlaySound(retryClickAudio, 0.5f);
        _gameController.RetryLevel();
    }

    public void MenuButton()
    {
        Debug.Log("Menu button clicked");
        _soundManager.PlaySound(buttonClickAudio, 0.5f);
        _gameController.OpenMainMenu();
    }
}
