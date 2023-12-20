using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldTravelDirection { X, Z, Unset };

public class Vehicle : MonoBehaviour
{
    [SerializeField] private float m_Speed = 5f;
    [SerializeField] private float m_FollowDistance = 10f;
    [SerializeField] private BoxCollider m_Collider;
    [SerializeField] private float m_AccelerateTime = 10f;
    [SerializeField] private float m_LinecastHitAdjustOffset = 0.25f;
    [SerializeField] private LayerMask m_LayerMaskVehicle;
    [SerializeField] private LayerMask m_LayerMaskIntersection;
    

    bool b_IsStopped = false;
    private float m_CurrentSpeed;
    private float m_LerpAccelerateTime = 0f;
    public WorldTravelDirection m_WorldTravelDirection { get; private set; }


    private IntersectionManager m_WaitingAtIntersection;

    private void Start()
    {
        m_WorldTravelDirection = ComputeWorldTravelDirection();
    }

    public WorldTravelDirection ComputeWorldTravelDirection()
    {
        float dot = Vector3.Dot(Vector3.forward, transform.forward);
        return Mathf.Abs(dot) > 0.1f ? WorldTravelDirection.Z : WorldTravelDirection.X;
    }

    // Update is called once per frame
    private void Update()
    {
        if (b_IsStopped && !IsBlockedByCar() && !WaitAtIntersection())
        {
            b_IsStopped = false;
            return;
        }

        if (!b_IsStopped)
        {
            if (m_WaitingAtIntersection != null)
            {
                m_WaitingAtIntersection.VehicleEnteredIntersection(this);
                m_WaitingAtIntersection = null;
            }

            bool moveBlocked = Move();
            if (moveBlocked)
            {
                m_CurrentSpeed = 0f;
                m_LerpAccelerateTime = 0f;
                b_IsStopped = true;
            }
        }       
    }

    private bool IsBlockedByCar()
    {
        Vector3 lineCastStart = transform.position + (transform.forward * (m_FollowDistance + m_Collider.size.z / 2f));
        Vector3 lineCastEnd = lineCastStart + transform.forward * m_LinecastHitAdjustOffset * 2f;
        return Physics.Linecast(lineCastStart, lineCastEnd, m_LayerMaskVehicle);
    }

    private bool TryFindIntersection()
    {
        Vector3 lineCastStart = transform.position; //transform.position + (transform.forward * m_Collider.size.z / 2f);
        Vector3 lineCastEnd = lineCastStart + transform.forward * m_LinecastHitAdjustOffset * 2f + (transform.forward * m_Collider.size.z / 2f);

        RaycastHit hit;
        if (Physics.Linecast(lineCastStart, lineCastEnd, out hit, m_LayerMaskIntersection))
        {
            m_WaitingAtIntersection = hit.collider.GetComponent<IntersectionManager>();
            return true;
        }

        return false;
    }


    private bool WaitAtIntersection()
    {
        if (m_WaitingAtIntersection == null)
        {
           TryFindIntersection();
        }

        if (m_WaitingAtIntersection != null)
        {
            return m_WorldTravelDirection != WorldTravelDirection.Unset &&
                m_WaitingAtIntersection.m_TrafficFlowDirection != m_WorldTravelDirection;
        }

        return false;


        //WorldSpawnDirection otherVehicleDirection = m_WorldSpawnDirection == WorldSpawnDirection.X ? WorldSpawnDirection.Z : WorldSpawnDirection.X;
        //if (m_WaitingAtStoplight != null && m_WaitingAtStoplight.VehiclesInIntersection(otherVehicleDirection))
        //{
        //    return true;
        //}

        ////Vector3 lineCastStart = transform.position; //transform.position + (transform.forward * m_Collider.size.z / 2f);
        ////Vector3 lineCastEnd = lineCastStart + transform.forward * m_LinecastHitAdjustOffset * 2f + (transform.forward * m_Collider.size.z / 2f);

        //RaycastHit hit;
        //if (Physics.Linecast(lineCastStart, lineCastEnd, out hit, m_LayerMaskIntersection))
        //{
        //    m_WaitingAtStoplight = hit.collider.GetComponent<Stoplight>();
        //    return true;
        //}

        //return false;
    }

    public Vector3 GetColliderFrontPosition()
    {
        return transform.position;
    }

    private bool Move()
    {
        m_CurrentSpeed = GetCurrentSpeed();

        Vector3 desiredPosition = transform.forward * Time.deltaTime * m_CurrentSpeed;


        // Check for other vehicle in font of the vehicle
        Vector3 lineCastStart = transform.position + (transform.forward * (m_FollowDistance + m_Collider.size.z / 2f));
        Vector3 lineCastEnd = lineCastStart + desiredPosition;
        //Vector3 lineCastStart = transform.position;// + (transform.forward * m_Collider.size.z / 2f);
        //Vector3 lineCastEnd = lineCastStart + desiredPosition + (transform.forward * (m_FollowDistance + m_Collider.size.z / 2f));

        bool moveBlocked = false;
        RaycastHit hit;

        if (Physics.Linecast(lineCastStart, lineCastEnd, out hit, m_LayerMaskVehicle))
        {
            float hitDistance = hit.distance / Vector3.Distance(lineCastStart, lineCastEnd);
            desiredPosition = desiredPosition * hitDistance - (transform.rotation * Vector3.forward * m_LinecastHitAdjustOffset);
            moveBlocked = true;
            transform.position += desiredPosition;
            return moveBlocked;
        }


        // check for intersection
        lineCastStart = transform.position + (transform.forward * m_Collider.size.z / 2f);
        lineCastEnd = lineCastStart + desiredPosition;

        if (Physics.Linecast(lineCastStart, lineCastEnd, out hit, m_LayerMaskIntersection))
        {
            IntersectionManager intersectionManager = hit.collider.GetComponent<IntersectionManager>();
            if (intersectionManager != null && intersectionManager.m_TrafficFlowDirection != m_WorldTravelDirection)
            {
                float hitDistance = hit.distance / Vector3.Distance(lineCastStart, lineCastEnd);
                desiredPosition = desiredPosition * hitDistance - (transform.rotation * Vector3.forward * m_LinecastHitAdjustOffset);
                moveBlocked = true;
                transform.position += desiredPosition;
                return moveBlocked;
            }
        }

        transform.position += desiredPosition;
        return moveBlocked;
    }





    private float GetCurrentSpeed()
    {
        float currentSpeed = 0f;
        if (m_AccelerateTime > 0f)
        {
            m_LerpAccelerateTime += Time.deltaTime;
            currentSpeed = Mathf.Lerp(0f, m_Speed, Mathf.Clamp(m_LerpAccelerateTime / m_AccelerateTime, 0f, 1f));
        }
        else
        {
            currentSpeed = m_Speed;
        }

        return currentSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 stopStart = transform.position + transform.forward * m_Collider.size.z / 2f;
        Gizmos.DrawSphere(stopStart, 0.25f);

        Gizmos.DrawLine(stopStart, stopStart + (transform.rotation * Vector3.forward * m_FollowDistance));

        Vector3 lineCastStart = transform.position + (transform.forward * (m_FollowDistance + m_Collider.size.z / 2f));// + (m_Collider.bounds.center * m_Collider.size.z / 2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(lineCastStart, 0.25f);
    }
}
