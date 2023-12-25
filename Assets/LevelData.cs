using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    
    private static List<IntersectionManager> m_Intersections = new List<IntersectionManager>();
    private static List<Vehicle> m_SpawnedVehicles = new List<Vehicle>();
    



    private void Awake()
    {
        m_Intersections = FindObjectsOfType<IntersectionManager>().ToList();
        EventManager.OnVehicleSpawned += HandleOnVehicleSpawned;
        EventManager.OnVehicleDestroyed += HandleOnVehicleDestroyed;
    }

    private void OnDestroy()
    {
        EventManager.OnVehicleSpawned -= HandleOnVehicleSpawned;
        EventManager.OnVehicleDestroyed -= HandleOnVehicleDestroyed;
    }

    public static IReadOnlyList<Vehicle> GetSpawnedVehicles()
    {
        return m_SpawnedVehicles;
    }

    public static IReadOnlyList<IntersectionManager> GetIntersections()
    {
        return m_Intersections;
    }

    private void HandleOnVehicleSpawned(Vehicle vehicle) => m_SpawnedVehicles.Add(vehicle);
    private void HandleOnVehicleDestroyed(Vehicle vehicle) => m_SpawnedVehicles.Remove(vehicle);


  
}
