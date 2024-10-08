using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // Assign this in the Unity Inspector
    private float currentTime;
    private bool isTimerRunning = false;
    private GameController _gameController;

    // Method to set the timer to a certain time (in seconds)
    void Start()
    {
        if (_gameController == null)
            _gameController = FindObjectOfType<GameController>();
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
                currentTime -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                currentTime = 0;
                isTimerRunning = false;
                UpdateTimerDisplay();

                // Notify GameController when time runs out
                _gameController.timeout();
            }
        }
    }

    // Method to update the timer display
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
