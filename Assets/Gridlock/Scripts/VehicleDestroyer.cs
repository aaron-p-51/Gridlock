using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDestroyer : MonoBehaviour
{
    [SerializeField] private BoxCollider m_DestroyVolume;

    private void OnValidate()
    {
        m_DestroyVolume = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(m_DestroyVolume.center, m_DestroyVolume.size);
    }
}
