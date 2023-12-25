using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelSettings m_LevelSettings;




    //[SerializeField] private float m_LevelTime;
    //[SerializeField] private VehicleSpawner[] m_VehicleSpawners;
    //[SerializeField] private List<SpawnerControls> m_SpawnerControls = new List<SpawnerControls>();
    //[SerializeField] private IntersectionManager[] m_IntersectionManagers;
    //[SerializeField] private AnimationCurve m_ScoreVsTimeWaitedAtIntersection;
    //[SerializeField] private float m_BaseVehicleScore;

    private static List<IntersectionManager> m_Intersections = new List<IntersectionManager>();
    private static List<Vehicle> m_SpawnedVehicles = new List<Vehicle>();
    private static List<VehicleSpawner> m_VehicleSpawners = new List<VehicleSpawner>();


    public enum LevelState { Intro, InGame, GameOver };

    private int m_CurrentSpawnerControlIndex = 0;

    public float m_Score { get; private set; }
    //public static Action<float> OnScoreChange;


    //private List<Vehicle> m_SpawnedVehicles = new List<Vehicle>();


    private float m_TimeElapsed;

    private void Awake()
    {
        SetupLevel();

        EventManager.OnVehicleEnterScoreTrigger += HandleOnVehicleEnterScoreTrigger;

    }


    private void SetupLevel()
    {
        m_Intersections = FindObjectsOfType<IntersectionManager>().ToList();
        m_VehicleSpawners = FindObjectsOfType<VehicleSpawner>().ToList();

        foreach (VehicleSpawner vehicleSpawner in m_VehicleSpawners)
        {
            vehicleSpawner.SetDefaultVehiclePrefabs(m_LevelSettings.spawnableVehicles);
            vehicleSpawner.SetSpawnerConfig(m_LevelSettings.vehicleSpawnerConfigs.First());
        }

        m_CurrentSpawnerControlIndex = 0;
    }

    private void OnDestroy()
    {
        EventManager.OnVehicleEnterScoreTrigger -= HandleOnVehicleEnterScoreTrigger;
    }

    private void Start()
    {
        EventManager.RaiseOnLevelStateChange(LevelState.Intro);
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        m_TimeElapsed += Time.deltaTime;
        if (m_LevelSettings.vehicleSpawnerConfigs.Length > 0 && 
            m_TimeElapsed > m_LevelSettings.vehicleSpawnerConfigs[m_CurrentSpawnerControlIndex].levelTime)
        {
            foreach(VehicleSpawner spawner in m_VehicleSpawners)
            {
                spawner.SetSpawnerConfig(m_LevelSettings.vehicleSpawnerConfigs[m_CurrentSpawnerControlIndex]);
            }

            m_CurrentSpawnerControlIndex++;
        }
    }

    

    private void HandleOnVehicleEnterScoreTrigger(Vehicle vehicle)
    {
        //m_Score += m_ScoreVsTimeWaitedAtIntersection.Evaluate(vehicle.m_TimeWaitingAtIntersection) + m_BaseVehicleScore;
        //EventManager.RaiseOnScoreChange(m_Score);
    }

    

    
}
