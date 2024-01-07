using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridlockWatchdog : MonoBehaviour
{
    [SerializeField] private float m_CheckInterval = 1.5f;
    [SerializeField] private int m_SequencialChecksRequired = 2;

    private int m_SequencialChecks;

    private void OnEnable()
    {
        StartCoroutine(CheckGridlock());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        m_SequencialChecks = 0;
    }

    private IEnumerator CheckGridlock()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(m_CheckInterval);
            if (AllIntersectionsGridlocked() && AllUnscoredVehiclesStopped())
            {
                m_SequencialChecks++;
                if (m_SequencialChecks >= m_SequencialChecksRequired)
                {
                    EventManager.RaiseOnGridlockDetected();
                }
                else
                {
                    m_SequencialChecks = 0;
                }
            }
        }
    }

    public bool AllIntersectionsGridlocked()
    {
        IReadOnlyList<IntersectionManager> intersectionManagers = LevelManager.GetAllIntersections();
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
        IReadOnlyList<Vehicle> vehicles = LevelManager.GetAllVehicles();
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
