using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle", menuName = "ScriptableObjects/ObstacleScriptableObject", order = 1)]
public class ObstacleScriptableObject : ScriptableObject
{
    public GameObject obstacle;
    public int poolSize = 10;
}
