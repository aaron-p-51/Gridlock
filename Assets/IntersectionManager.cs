using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionManager : MonoBehaviour
{
    [SerializeField] private GameObject StopLightX;
    [SerializeField] private GameObject StopLightZ;
    [SerializeField] private BoxCollider m_Trigger;

    public int InIntersection;

    private bool m_StopLightXActive = false;

    HashSet<Vehicle> m_VehiclesInIntersection = new HashSet<Vehicle>();

    // Start is called before the first frame update
    void Start()
    {
        StopLightX.SetActive(m_StopLightXActive);
        StopLightZ.SetActive(!m_StopLightXActive);
    }

    public int NumVehiclsInIntersection(WorldSpawnDirection spawnDirection = WorldSpawnDirection.Either)
    {
        int numVehicles = 0;
        if (spawnDirection == WorldSpawnDirection.Either)
        {
            numVehicles = m_VehiclesInIntersection.Count;
        }
        else if (spawnDirection == WorldSpawnDirection.X)
        {
            numVehicles = 0;
            foreach(Vehicle vehicle in m_VehiclesInIntersection)
            {
                if (vehicle.m_WorldSpawnDirection == WorldSpawnDirection.X)
                {
                    ++numVehicles;
                }
            }
        }
        else if (spawnDirection == WorldSpawnDirection.Z)
        {
            numVehicles = 0;
            foreach (Vehicle vehicle in m_VehiclesInIntersection)
            {
                if (vehicle.m_WorldSpawnDirection == WorldSpawnDirection.Z)
                {
                    ++numVehicles;
                }
            }
        }

        return numVehicles;
    }


    public void TrySwitchStoplights()
    {

        if (m_VehiclesInIntersection.Count > 0) return;

        m_StopLightXActive = !m_StopLightXActive;
        StopLightX.SetActive(m_StopLightXActive);
        StopLightZ.SetActive(!m_StopLightXActive);

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

    public void VehicleLeftIntersection(Vehicle vehicle)
    {
        //vehiclesInIntersection.Remove(vehicle);
    }
}
