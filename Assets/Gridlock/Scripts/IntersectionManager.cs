using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionManager : MonoBehaviour
{

    [SerializeField] private BoxCollider m_IntersectionTrigger;
    [SerializeField] private TrafficLight[] m_TrafficLights;
    [SerializeField] private WorldTravelDirection m_StartingTrafficDirection;
    [SerializeField] private float m_MinTimeBetweenLightChanges = 1f;


    public WorldTravelDirection m_TrafficFlowDirection = WorldTravelDirection.X;

    public bool m_IsGridlocked { get; private set; }
    public int InIntersection;
    public bool m_IsGridlockedInspector;
    private bool m_StopLightXActive = false;

    HashSet<Vehicle> m_VehiclesInIntersection = new HashSet<Vehicle>();

    private float m_LastLightChangeTime = 0f;

    private void OnValidate()
    {
        if (m_StartingTrafficDirection == WorldTravelDirection.Unset)
        {
            Debug.LogWarning($"{gameObject.name} has StartingTrafficDirection set to Unset");
            return;
        }

        m_TrafficFlowDirection = m_StartingTrafficDirection;
        //UpdateTrafficLights();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTrafficLights();
    }

    public int NumVehiclsInIntersection(WorldTravelDirection spawnDirection = WorldTravelDirection.Unset)
    {
        int numVehicles = 0;
        if (spawnDirection == WorldTravelDirection.Unset)
        {
            numVehicles = m_VehiclesInIntersection.Count;
        }
        else if (spawnDirection == WorldTravelDirection.X)
        {
            numVehicles = 0;
            foreach(Vehicle vehicle in m_VehiclesInIntersection)
            {
                if (vehicle.m_WorldTravelDirection == WorldTravelDirection.X)
                {
                    ++numVehicles;
                }
            }
        }
        else if (spawnDirection == WorldTravelDirection.Z)
        {
            numVehicles = 0;
            foreach (Vehicle vehicle in m_VehiclesInIntersection)
            {
                if (vehicle.m_WorldTravelDirection == WorldTravelDirection.Z)
                {
                    ++numVehicles;
                }
            }
        }

        return numVehicles;
    }

    public bool CanEnterIntersection(Vehicle vehicle)
    {
        if (m_VehiclesInIntersection.Contains(vehicle)) return true;
        
        if (m_TrafficFlowDirection == WorldTravelDirection.Unset || m_TrafficFlowDirection != vehicle.m_WorldTravelDirection) return false;
        // get the other vehicle travel direction
        WorldTravelDirection otherVehicleTravelDirection = GetOtherWorldTravelDirection(vehicle.m_WorldTravelDirection);
        return NumVehiclsInIntersection(otherVehicleTravelDirection) <= 0 && m_TrafficFlowDirection == vehicle.m_WorldTravelDirection;
    }

    public bool IsGridlocked()
    {
        if (m_VehiclesInIntersection.Count == 0) return false;

        foreach (Vehicle vehicle in m_VehiclesInIntersection)
        {
            if (!vehicle.m_IsStopped)
            {
                return false;
            }
        }

        return true;
    }

    public static WorldTravelDirection GetOtherWorldTravelDirection(WorldTravelDirection worldTravelDirection)
    {
        return worldTravelDirection == WorldTravelDirection.X ? WorldTravelDirection.Z : WorldTravelDirection.X;
    }
    public void TrySwitchTrafficFlow()
    {
        //if (m_VehiclesInIntersection.Count == 0)
        //{
        if (Time.time > (m_LastLightChangeTime + m_MinTimeBetweenLightChanges))
        {
            m_TrafficFlowDirection = m_TrafficFlowDirection == WorldTravelDirection.X ? WorldTravelDirection.Z : WorldTravelDirection.X;
            UpdateTrafficLights();

            m_LastLightChangeTime = Time.time;
        }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        InIntersection = m_VehiclesInIntersection.Count;
    }

    public bool VehicleEnteredIntersection(Vehicle vehicle)
    {
        return m_VehiclesInIntersection.Add(vehicle); 
    }

    public bool VehicleInIntersection(Vehicle vehicle)
    {
        return m_VehiclesInIntersection.Contains(vehicle);
    }


    private void OnTriggerExit(Collider other)
    {
        Vehicle vehicle = other.GetComponent<Vehicle>();
        if (vehicle != null && m_VehiclesInIntersection.Contains(vehicle))
        {
            Vector3 currentVehicleLocation = vehicle.transform.position;
            
            if (Vector3.SqrMagnitude(currentVehicleLocation - vehicle.m_LocationWhenEnterendLastIntersection) > 100f)
            {
                m_VehiclesInIntersection.Remove(other.GetComponent<Vehicle>());
            }
        }
    }

    public void UpdateTrafficLights()
    {
        foreach (TrafficLight trafficLight in m_TrafficLights)
        {
            if (trafficLight.m_TrafficFlowDirection == m_TrafficFlowDirection)
            {
                trafficLight.SetGreenLight();
            }
            else
            {
                trafficLight.SetRedLight();
            }
        }
    }
}
