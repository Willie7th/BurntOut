using TMPro;
using UnityEngine;

public class LevelCompleteController : MonoBehaviour
{
    private GameController _gameController;
    private SoundManager _soundManager;
    [SerializeField] private AudioClip buttonClickAudio;
    [SerializeField] private AudioClip retryClickAudio;
    public TextMeshProUGUI timeLabel;
    public TextMeshProUGUI energyLabel;

    // Start is called before the first frame update
    void Start()
    {
        if (_gameController == null)
            _gameController = FindAnyObjectByType<GameController>();
        if (_soundManager == null)
            _soundManager = FindAnyObjectByType<SoundManager>();
        _gameController.pauseMenuOpen = true;  //Just so pause menu can't be activated
        SetGameStats(_gameController.stats);
    }

    private void SetGameStats(GameStats stats)  //Set the energy and time taken
    {
        int minutes = Mathf.FloorToInt(stats.remainingTime / 60);
        int seconds = Mathf.FloorToInt(stats.remainingTime % 60);
        string timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeLabel.text = timerText;
        energyLabel.text = stats.remainingEnergy + "";
    }

    public void RetryButton()
    {
        _soundManager.PlaySound(retryClickAudio, 0.5f);
        Debug.Log("Retry button clicked");
        _gameController.RetryLevel();
    }

    public void MenuButton()
    {
        _soundManager.PlaySound(buttonClickAudio, 0.5f);
        Debug.Log("Menu button clicked");
        _gameController.OpenMainMenu();
    }

    public void NextLevelButton()
    {
        _soundManager.PlaySound(buttonClickAudio, 0.5f);
        Debug.Log("Menu button clicked");
        _gameController.LoadNextLevel();
    }
}
