using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnerControls
{
    public float levelTime;
    public Vector2 spawnDelayTime;
    public float spawnProgability;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float m_LevelTime;
    [SerializeField] private VehicleSpawner[] m_VehicleSpawners;
    [SerializeField] private List<SpawnerControls> m_SpawnerControls = new List<SpawnerControls>();
    [SerializeField] private IntersectionManager[] m_IntersectionManagers;
    [SerializeField] private AnimationCurve m_ScoreVsTimeWaitedAtIntersection;
    [SerializeField] private float m_BaseVehicleScore;

    public float m_Score { get; private set; }
    //public static Action<float> OnScoreChange;


    private List<Vehicle> m_SpawnedVehicles = new List<Vehicle>();


    private float m_TimeElapsed;

    private void Awake()
    {
        EventManager.OnVehicleSpawned += HandleOnVehicleSpawned;
        EventManager.OnVehicleDestroyed += HandleOnVehicleDestroyed;
        EventManager.OnVehicleEnterScoreTrigger += HandleOnVehicleEnterScoreTrigger;
    }

    private void OnDestroy()
    {
        EventManager.OnVehicleSpawned -= HandleOnVehicleSpawned;
        EventManager.OnVehicleDestroyed -= HandleOnVehicleDestroyed;
        EventManager.OnVehicleEnterScoreTrigger -= HandleOnVehicleEnterScoreTrigger;
    }

    private void Start()
    {
        StartCoroutine(CheckForGridlock());
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        m_TimeElapsed += Time.deltaTime;
        if (m_SpawnerControls.Count > 0 && m_TimeElapsed > m_SpawnerControls[0].levelTime)
        {
            foreach(VehicleSpawner spawner in m_VehicleSpawners)
            {
                spawner.SetNewSpawnerRates(m_SpawnerControls[0].spawnDelayTime, m_SpawnerControls[0].spawnProgability);
            }

            m_SpawnerControls.RemoveAt(0);
        }
    }

    private void HandleOnVehicleSpawned(Vehicle vehicle)
    {
        m_SpawnedVehicles.Add(vehicle);
    }

    private void HandleOnVehicleDestroyed(Vehicle vehicle)
    {
        m_SpawnedVehicles.Remove(vehicle);
    }

    private void HandleOnVehicleEnterScoreTrigger(Vehicle vehicle)
    {
        m_Score += m_ScoreVsTimeWaitedAtIntersection.Evaluate(vehicle.m_TimeWaitingAtIntersection) + m_BaseVehicleScore;
        EventManager.RaiseOnScoreChange(m_Score);
    }

    private IEnumerator CheckForGridlock()
    {
        while (true)
        {
            if (AllIntersectionsGridlocked() && AllUnscoredVehiclesStopped())
            {
                Debug.LogError("Grid Lock!!!");
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    public bool AllIntersectionsGridlocked()
    {
        foreach (IntersectionManager intersectionManager in m_IntersectionManagers)
        {
            if (!intersectionManager.IsGridlocked())
            {
                return false;
            }
        }

        return true;
    }

    public bool AllUnscoredVehiclesStopped()
    {
        foreach (Vehicle vehicle in m_SpawnedVehicles)
        {
            if (!vehicle.IsStopped && !vehicle.m_Scored)
            {
                return false;
            }
        }

        return true;
    }
}
