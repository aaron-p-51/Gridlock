using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum WorldSpawnDirection { X, Z, Either };

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] m_VehiclePrefabs;
    [SerializeField] private BoxCollider m_SpawnCollider;
    [SerializeField] private Transform m_SpawnPoint;
    [SerializeField] private Vector2 m_SpawnRange;
    [SerializeField] private float m_SpawnCheckDistance;
    [SerializeField] private LayerMask m_LayerMask;
    [SerializeField][Range(0f, 1f)] private float m_SpawnChance;

    //public WorldSpawnDirection m_WorldSpawnDirection;

    RaycastHit m_Hit;
    bool m_HitDetected;

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
            yield return new WaitForSeconds(Random.Range(m_SpawnRange.x, m_SpawnRange.y));

            if (Random.Range(0f, 1f) < m_SpawnChance)
            {
                RaycastHit hit;
                Collider[] hitColliders = Physics.OverlapBox(transform.TransformPoint(m_SpawnCollider.center), m_SpawnCollider.size * 0.5f, Quaternion.identity, m_LayerMask);
                if (hitColliders.Length == 0)
                {
                    int prefabIndex = Random.Range(0, m_VehiclePrefabs.Length);
                    Instantiate(m_VehiclePrefabs[prefabIndex], m_SpawnPoint.position, transform.rotation, transform);
                }
                else
                {
                    Debug.LogError($"Fail To Spawn: hit {hitColliders[0].name}");
                }
            }
        }
    }

    public void SetNewSpawnerRates(Vector2 newSpawnRange, float newSpawnChance)
    {
        m_SpawnRange = newSpawnRange;
        m_SpawnChance = newSpawnChance;
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
