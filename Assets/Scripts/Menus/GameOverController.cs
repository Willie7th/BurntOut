using UnityEngine;

public class GameOverController : MonoBehaviour
{
    private GameController _gameController;
    private SoundManager _soundManager;

    [SerializeField] private AudioClip buttonClickAudio;
    [SerializeField] private AudioClip retryClickAudio;

    // Start is called before the first frame update
    void Start()
    {
        if (_gameController == null)
            _gameController = FindAnyObjectByType<GameController>();
        if (_soundManager == null)
            _soundManager = FindAnyObjectByType<SoundManager>();
        _gameController.pauseMenuOpen = true;  //Just so pause menu can't be activated
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
}
