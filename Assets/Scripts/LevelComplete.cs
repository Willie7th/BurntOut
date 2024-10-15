using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    
    private List<GameObject> emberPrefabs = new List<GameObject>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            //Debug.Log(child);
            emberPrefabs.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (GameObject ember in emberPrefabs)
        {
            ember.SetActive(true);
        }
        
    }
}
