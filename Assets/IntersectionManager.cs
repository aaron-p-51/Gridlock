using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionManager : MonoBehaviour
{

    [SerializeField] private BoxCollider m_IntersectionTrigger;
    public WorldTravelDirection m_TrafficFlowDirection = WorldTravelDirection.X;


    public int InIntersection;

    private bool m_StopLightXActive = false;

    HashSet<Vehicle> m_VehiclesInIntersection = new HashSet<Vehicle>();

    // Start is called before the first frame update
    void Start()
    {
       
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


    public void TrySwitchTrafficFlow()
    {

        if (m_VehiclesInIntersection.Count > 0) return;
        m_TrafficFlowDirection = m_TrafficFlowDirection == WorldTravelDirection.X ? WorldTravelDirection.Z : WorldTravelDirection.X;
    }

    // Update is called once per frame
    void Update()
    {
        InIntersection = m_VehiclesInIntersection.Count;
    }

    public void VehicleEnteredIntersection(Vehicle vehicle)
    {
        m_VehiclesInIntersection.Add(vehicle);
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    m_VehiclesInIntersection.Remove(collision.gameObject.GetComponent<Vehicle>());
    //}

    private void OnTriggerEnter(Collider other)
    {
        m_VehiclesInIntersection.Add(other.GetComponent<Vehicle>());
    }

    private void OnTriggerExit(Collider other)
    {
        m_VehiclesInIntersection.Remove(other.GetComponent<Vehicle>());
    }
}
