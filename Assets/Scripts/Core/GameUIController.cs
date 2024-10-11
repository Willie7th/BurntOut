using System.Drawing;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // Assign this in the Unity Inspector
    public TextMeshProUGUI energyText;
    private float currentTime;
    private bool isTimerRunning = false;
    private bool timeTicking = false;
    private GameController _gameController;
    private FlameController _flameController;
    private SoundManager _soundManager;

    [SerializeField] private AudioClip timeTickAudio;

    // Method to set the timer to a certain time (in seconds)
    void Start()
    {
        if (_gameController == null)
            _gameController = FindObjectOfType<GameController>();

        _flameController = FindObjectOfType<FlameController>();

        if (_soundManager == null)
            _soundManager = FindObjectOfType<SoundManager>();
    }

    public void setTimerRunning(bool state)
    {
        isTimerRunning = state;
    }
    
    public void SetTimer(float timeInSeconds)
    {
        currentTime = timeInSeconds;
        isTimerRunning = true;
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            if (currentTime > 0)
            {
                if(currentTime < 5 && !timeTicking)
                {

                    timerText.fontSize = 35;
                    _soundManager.PlaySound(timeTickAudio, 1f);
                    timeTicking = true;
                    timerText.color = UnityEngine.Color.red;
                }
                currentTime -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                currentTime = 0;
                isTimerRunning = false;
                //UpdateTimerDisplay();

                // Notify GameController when time runs out
                _gameController.timeout();
            }
        }

        energyText.text = "Energy level: " + _flameController.getFlameEnergy();
    }

    // Method to update the timer display
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        
        if(currentTime <= 0)
        {
            timerText.text = "Time Is Up";
        }
        else{
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public float getCurrentTime()
    {
        return currentTime;
    }

}
