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


    private float m_TimeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
}
