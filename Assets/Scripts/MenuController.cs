using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private GameObject GameController;

    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.Find("GameController");
        showCompletedLevels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startButtonClicked()
    {
        DontDestroyOnLoad(GameController);
        SceneManager.LoadScene("Level1");
    }

    private void showCompletedLevels()
    {
        //Highlight the levels that have been selected
    }
}
