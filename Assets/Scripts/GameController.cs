//This script keeps information that will be needed by all the scenes and is used to keep track of completed levels
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<int> levelsComplete = new List<int>{};  //Will store the completed levels and send them to the menu controller

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Should always listen for 'esc` to pause a game
}
