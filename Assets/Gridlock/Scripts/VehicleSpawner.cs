using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldSpawnDirection { X, Z, Either };

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] m_VehiclePrefabs;
    [SerializeField] private Vector2 m_SpawnRange;
    [SerializeField] private float m_SpawnCheckDistance;
    [SerializeField] private LayerMask m_LayerMask;
    [SerializeField][Range(0f, 1f)] private float m_SpawnChance;

    public WorldSpawnDirection m_WorldSpawnDirection;

    // Start is called before the first frame update
    void Start()
    {
        float dot = Vector3.Dot(Vector3.forward, transform.forward);
        m_WorldSpawnDirection = Mathf.Abs(dot) > 0.1f ? WorldSpawnDirection.Z : WorldSpawnDirection.X;
        Debug.Log($"Dot:{dot}");

        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(m_SpawnRange.x, m_SpawnRange.y));

            if (Random.Range(0f, 1f) < m_SpawnChance)
            {
                if (!Physics.Raycast(transform.position, transform.forward, m_SpawnCheckDistance, m_LayerMask))
                {
                    int prefabIndex = Random.Range(0, m_VehiclePrefabs.Length);

                    GameObject go = Instantiate(m_VehiclePrefabs[prefabIndex], transform.position, transform.rotation, transform);
                    if (go != null)
                    {
                        go.GetComponent<Vehicle>().m_WorldSpawnDirection = m_WorldSpawnDirection;
                    }

                }
            }
        }
    }

    public void SetNewSpawnerRates(Vector2 newSpawnRange, float newSpawnChance)
    {
        m_SpawnRange = newSpawnRange;
        m_SpawnChance = newSpawnChance;
    }


}
