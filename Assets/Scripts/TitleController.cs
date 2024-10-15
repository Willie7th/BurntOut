using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleController : MonoBehaviour
{

    public GameObject soundManagerObj;
    public GameObject gameControllerObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(soundManagerObj);
        DontDestroyOnLoad(gameControllerObj);
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
