using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoplight : MonoBehaviour
{

    [SerializeField] IntersectionManager m_IntersectionManager;

    public void VehicleEnteredIntersection(Vehicle vehicle)
    {
        m_IntersectionManager.VehicleEnteredIntersection(vehicle);
    }

    public bool VehiclesInIntersection(WorldSpawnDirection spawnDirection)
    {
        return m_IntersectionManager.NumVehiclsInIntersection(spawnDirection) > 0;
    }

   
}
