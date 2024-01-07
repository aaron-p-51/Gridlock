using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/LevelSettings", order = 1)]
public class LevelSettings : ScriptableObject
{
    [Serializable]
    public struct LevelNumber
    {
        public int main;
        public int sub;
    }

    public LevelNumber levelNumber;
    public string LevelName;
    public float m_TimeLimit;
    public int m_PassingScore;
    public float m_BaseVehicleScore;
    public AnimationCurve m_ScoreVsTimeWaitedAtIntersection;
    public GameObject[] spawnableVehicles;
    public VehicleSpawner.Config[] vehicleSpawnerConfigs;
}
