using UnityEngine;

public class LevelLoaderController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator transition;

    public void playAnimation()
    {
        transition.SetTrigger("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
