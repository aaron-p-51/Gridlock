using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] m_VehiclePrefabs;
    [SerializeField] private BoxCollider m_SpawnCollider;
    [SerializeField] private Transform m_SpawnPoint;
    [SerializeField] private Vector2 m_SpawnRange;
    [SerializeField] private float m_SpawnCheckDistance;
    [SerializeField] private LayerMask m_LayerMask;
    [SerializeField][Range(0f, 1f)] private float m_SpawnChance;

    private GameObject[] m_DefaultVehiclePrefabs;

    [System.Serializable]
    public struct Config
    {
        public float levelTime;
        public Vector2 spawnIntervalRange;
        public float spawnProbability;
        public GameObject[] vehiclePrefabsOverrides;
    }

    private Config m_Config;

    private void OnValidate()
    {
        m_SpawnCollider = GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(GetSpawnDelay());

            if (CanSpawn())
            {
                SpawnVehicle();
            }
        }
    }

    public bool CanSpawn()
    {
        return UnityEngine.Random.Range(0f, 1f) <= m_Config.spawnProbability &&
            Physics.OverlapBox(transform.TransformPoint(m_SpawnCollider.center), m_SpawnCollider.size * 0.5f, Quaternion.identity, m_LayerMask).Length == 0;
    }

    public float GetSpawnDelay()
    {
        return UnityEngine.Random.Range(m_Config.spawnIntervalRange.x, m_Config.spawnIntervalRange.y);
    }

    public void SpawnVehicle()
    {
        int prefabIndex = UnityEngine.Random.Range(0, m_VehiclePrefabs.Length);
        Instantiate(m_VehiclePrefabs[prefabIndex], m_SpawnPoint.position, transform.rotation, transform);
    }

    public void SetDefaultVehiclePrefabs(GameObject[] vehiclePrefabs)
    {
        m_DefaultVehiclePrefabs = vehiclePrefabs;
    }

    public void SetSpawnerConfig(VehicleSpawner.Config spawnerConfig)
    {
        m_Config = spawnerConfig;
        if (m_Config.vehiclePrefabsOverrides.Length > 0)
        {
            m_VehiclePrefabs = m_Config.vehiclePrefabsOverrides;
        }
        else
        {
            m_VehiclePrefabs = m_DefaultVehiclePrefabs;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        DrawArrow.ForGizmo(m_SpawnPoint.position, transform.forward * 8f, 2f, 20f);

        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(m_SpawnCollider.center, m_SpawnCollider.size);
    }


}
