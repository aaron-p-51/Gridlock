using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridlockWatchdog : MonoBehaviour
{
    [SerializeField] private float m_CheckInterval = 1.5f;
    [SerializeField] private int m_SequencialChecksRequired = 2;

    private float m_NextCheckTime;
    private int m_SequencialChecks;

    private void OnEnable()
    {
        m_NextCheckTime = Time.time + m_CheckInterval;
        m_SequencialChecks = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_NextCheckTime > Time.time)
        {
            if (AllIntersectionsGridlocked() && AllUnscoredVehiclesStopped())
            {
                m_SequencialChecks++;
                if (m_SequencialChecks >= m_SequencialChecksRequired)
                {
                    EventManager.RaiseOnGridlockDetected();
                }
            }
            else
            {
                m_SequencialChecks = 0;
            }

            m_NextCheckTime = Time.time + m_CheckInterval;
        }
    }

    public bool AllIntersectionsGridlocked()
    {
        IReadOnlyList<IntersectionManager> intersectionManagers = LevelData.GetIntersections();
        foreach (IntersectionManager intersectionManager in intersectionManagers)
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
        IReadOnlyList<Vehicle> vehicles = LevelData.GetSpawnedVehicles();
        foreach (Vehicle vehicle in vehicles)
        {
            if (!vehicle.IsStopped && !vehicle.m_Scored)
            {
                return false;
            }
        }

        return true;
    }
}
