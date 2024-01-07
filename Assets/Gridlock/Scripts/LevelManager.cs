using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UIController;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelSettings m_LevelSettings;
    [SerializeField] private UIController m_UIController;


    private static List<IntersectionManager> m_Intersections = new List<IntersectionManager>();
    private static List<Vehicle> m_SpawnedVehicles = new List<Vehicle>();
    private static List<VehicleSpawner> m_VehicleSpawners = new List<VehicleSpawner>();


    public enum LevelState { Intro, InGame, GameOver };

    private int m_CurrentSpawnerControlIndex = 0;

    private float m_TimeElapsed;

    private void Awake()
    {
        SetupLevel();
        SetupUI();

        EventManager.OnVehicleEnterScoreTrigger += HandleOnVehicleEnterScoreTrigger;
        m_UIController.OnButtonClicked += HandleOnUIControllerButtonClicked;
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

    private void SetupUI()
    {
        m_UIController.SetUIPageActive(UIPage.IntroPage);
    }

    private void OnDestroy()
    {
        EventManager.OnVehicleEnterScoreTrigger -= HandleOnVehicleEnterScoreTrigger;
        m_UIController.OnButtonClicked -= HandleOnUIControllerButtonClicked;
    }

    private void Start()
    {
        EventManager.RaiseOnLevelStateChange(LevelState.Intro);
    }

    private void ChangeLevelState(LevelState newLevelState)
    {

    }


    // Update is called once per frame
    void Update()
    {
        m_TimeElapsed += Time.deltaTime;

        if (m_LevelSettings.vehicleSpawnerConfigs.Length > 0 && 
            m_CurrentSpawnerControlIndex < m_LevelSettings.vehicleSpawnerConfigs.Length &&
            m_TimeElapsed > m_LevelSettings.vehicleSpawnerConfigs[m_CurrentSpawnerControlIndex].levelTime)
        {
            foreach(VehicleSpawner spawner in m_VehicleSpawners)
            {
                spawner.SetSpawnerConfig(m_LevelSettings.vehicleSpawnerConfigs[m_CurrentSpawnerControlIndex]);
            }

            m_CurrentSpawnerControlIndex++;
        }
    }

    private void HandleOnUIControllerButtonClicked(UIButton button)
    {
        //switch
    }


    private void HandleOnVehicleEnterScoreTrigger(Vehicle vehicle)
    {
        //m_Score += m_ScoreVsTimeWaitedAtIntersection.Evaluate(vehicle.m_TimeWaitingAtIntersection) + m_BaseVehicleScore;
        //EventManager.RaiseOnScoreChange(m_Score);
    }

    public static IReadOnlyList<IntersectionManager> GetAllIntersections() => m_Intersections;

    public static IReadOnlyList<VehicleSpawner> GetAllVehicleSpawners() => m_VehicleSpawners;

    public static IReadOnlyList<Vehicle> GetAllVehicles() => m_SpawnedVehicles;

    public static IReadOnlyList<Vehicle> GetAllUncoredVehciels() => m_SpawnedVehicles.FindAll(vehicle => !vehicle.m_Scored);

       

}
