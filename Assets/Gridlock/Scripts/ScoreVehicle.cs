using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreVehicle : MonoBehaviour
{
    [SerializeField] private BoxCollider m_ScoreVolume;

    public static Action<Vehicle> OnVehicleEnterScoreVolue;

    private void OnValidate()
    {
        m_ScoreVolume = GetComponent<BoxCollider>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        Vehicle vehicle = other.GetComponent<Vehicle>();
        if (vehicle && !vehicle.m_Scored)
        {
            OnVehicleEnterScoreVolue?.Invoke(vehicle);
            vehicle.m_Scored = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(m_ScoreVolume.center, m_ScoreVolume.size);
    }
}
