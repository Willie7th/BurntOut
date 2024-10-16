using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleController : MonoBehaviour
{

    public GameObject soundManagerObj;
    public GameObject gameControllerObj;

    private SoundManager _soundManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(soundManagerObj);
        DontDestroyOnLoad(gameControllerObj);

        if (_soundManager == null)
            _soundManager = FindAnyObjectByType<SoundManager>();


        _soundManager.PlayBackgroundMusic("Audio/BackgroundMusic/MenuBackground", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        Debug.Log("Menu loaded");
        SceneManager.LoadScene("Menu");
    }

    public void viewTutorial()
    {
        Debug.Log("Tutorial Loaded");
        SceneManager.LoadScene("Tutorial");
    }
}
