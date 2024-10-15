using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    public int index;
    public int time;
    public string level_name;
    public int startEnergy = 500;
    public bool complete = false;
}