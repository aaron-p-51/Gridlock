using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

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

    //public static Action<Vehicle> OnVehicleSpawned;
    //public static Action<Vehicle> OnVehicleDestroyed;

    private Vector3 hitLocation = Vector3.zero;
    public float m_TimeWaitingAtIntersection { get; private set; }
    public Vector3 m_LocationWhenEnterendLastIntersection { get; private set; }

    public bool m_IsStopped { get; private set; }
    private float m_CurrentSpeed;
    private float m_LerpAccelerateTime = 0f;
    public WorldTravelDirection m_WorldTravelDirection { get; private set; }

    public bool m_Scored = false;

    private IntersectionManager m_WaitingAtIntersection;

    private HashSet<IntersectionManager> m_EnteredIntersections = new HashSet<IntersectionManager>();

    private void Awake()
    {
        //OnVehicleSpawned?.Invoke(this);
        EventManager.OnVehicleSpawned?.Invoke(this);
        m_WorldTravelDirection = ComputeWorldTravelDirection();
        m_TimeWaitingAtIntersection = 0f;
    }

    public WorldTravelDirection ComputeWorldTravelDirection()
    {
        float dot = Vector3.Dot(Vector3.forward, transform.forward);
        return Mathf.Abs(dot) > 0.1f ? WorldTravelDirection.Z : WorldTravelDirection.X;
    }

    // Update is called once per frame
    private void Update()
    {
        
        Move();
        return;

        //if (m_IsStopped && m_WaitingAtIntersection != null)
        //{
        //    m_TimeWaitingAtIntersection += Time.deltaTime;
        //}

        //if (m_IsStopped && !IsBlockedByCar() && !WaitAtIntersection())
        //{
        //    m_IsStopped = false;
        //    return;
        //}

        //if (!m_IsStopped)
        //{
        //    if (m_WaitingAtIntersection != null)
        //    {
        //        m_WaitingAtIntersection.VehicleEnteredIntersection(this);
        //        m_LocationWhenEnterendLastIntersection = transform.position;
        //        m_WaitingAtIntersection = null;
        //    }

        //    bool moveBlocked = Move();
        //    if (moveBlocked)
        //    {
        //        m_CurrentSpeed = 0f;
        //        m_LerpAccelerateTime = 0f;
        //        m_IsStopped = true;
        //    }
        //}       
    }

    private void UpdateCurrentSpeed()
    {
        if (m_AccelerateTime > 0f)
        {
            m_LerpAccelerateTime += Time.deltaTime;
            m_CurrentSpeed = Mathf.Lerp(0f, m_Speed, Mathf.Clamp(m_LerpAccelerateTime / m_AccelerateTime, 0f, 1f));
        }
        else
        {
            m_CurrentSpeed = m_Speed;
        }
    }

    //public Vector3 GetPositionAtFollowDistance()
    //{
    //    return transform.position + (transform.forward * (m_FollowDistance + m_Collider.size.z * 0.5f));
    //}

    public bool IsBlockedByCar() => Physics.Raycast(GetForwardColliderPosition(), transform.forward, m_FollowDistance + m_LinecastHitAdjustOffset * 2f, m_LayerMaskVehicle);
    //{
    //    //Ray ray = new Ray(GetForwardColliderPosition(), transform.forward);
    //    //return Physics.Raycast(ray, m_FollowDistance, m_LayerMaskVehicle);

    //    //Vector3 linecastStart = transform.position;
    //    //Vector3 linecastEnd = linecastStart + (transform.forward * (m_FollowDistance + m_Collider.size.z * 0.5f)) + (transform.forward * m_LinecastHitAdjustOffset * 2f); 
    //    //return Physics.Linecast(linecastStart, linecastEnd, m_LayerMaskVehicle);
    //}

    //public bool IsBlockedAtIntersection()
    //{

    //}

    public IntersectionManager TryFindIntersection()
    {
        Ray ray = new Ray(GetForwardColliderPosition(), transform.forward);
        RaycastHit hit;
        //IntersectionManager intersectionManager = null;
        if (Physics.Raycast(ray, out hit, m_LinecastHitAdjustOffset * 2f, m_LayerMaskIntersection))
        {
            /*intersectionManager =*/ return hit.collider.GetComponent<IntersectionManager>();
            
        }

        //return intersectionManager;
        return null;



        //Vector3 lineCastStart = transform.position;
        //Vector3 lineCastEnd = lineCastStart + transform.forward * m_LinecastHitAdjustOffset * 2f + (transform.forward * m_Collider.size.z / 2f);

        //RaycastHit hit;
        //if (Physics.Linecast(lineCastStart, lineCastEnd, out hit, m_LayerMaskIntersection))
        //{
        //    m_WaitingAtIntersection = hit.collider.GetComponent<IntersectionManager>();
        //    return true;
        //}

        //return false;
    }


    //private bool WaitAtIntersection()
    //{
    //    //if (m_WaitingAtIntersection == null)
    //    //{
    //    //   TryFindIntersection();
    //    //}

    //    if (m_WaitingAtIntersection != null)
    //    {
    //        if (m_WaitingAtIntersection.m_TrafficFlowDirection != m_WorldTravelDirection) return true;


    //        WorldTravelDirection otherTrafficDirection = m_WorldTravelDirection == WorldTravelDirection.X ? WorldTravelDirection.Z : WorldTravelDirection.X;
    //        if (m_WaitingAtIntersection.m_TrafficFlowDirection == m_WorldTravelDirection &&
    //            m_WaitingAtIntersection.NumVehiclsInIntersection(otherTrafficDirection) > 0)
    //        {
    //            return true;
    //        }

    //    }

    //    return false;
    //}

    public bool m_LogMe = false;
    public void Log(int num)
    {
        if (m_LogMe)
        {
            Debug.Log(num);
        }
    }

    public bool WaitAtIntersection()
    {
        if (m_WaitingAtIntersection == null) return false;
        return !m_WaitingAtIntersection.CanEnterIntersection(this);
    }

    private void Move()
    {
        if (m_IsStopped)
        {
            m_IsStopped = IsBlockedByCar() || WaitAtIntersection();
        }
        else
        {

            UpdateCurrentSpeed();
            float moveDelta = Time.deltaTime * m_CurrentSpeed;
            Vector3 positionOffset = transform.forward * moveDelta;

            // look for Vehicle

            float vehicleFollowtraceLength = m_FollowDistance + moveDelta;
            Ray ray = new Ray(GetForwardColliderPosition(), transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, vehicleFollowtraceLength, m_LayerMaskVehicle))
            {
                float currentDistance = hit.distance;//  Vector3.Distance(GetForwardColliderPosition(), hit.point);
                float seperation = m_FollowDistance - currentDistance;
                Vector3 newPosition = transform.position + (-transform.forward * seperation);
                //transform.position = newPosition;

                positionOffset = -transform.forward * seperation;

                
                ResetCurrentSpeed();
                m_IsStopped = true;

                //return;
            }

            if (Physics.Raycast(ray, out hit, moveDelta, m_LayerMaskIntersection))
            {
                IntersectionManager intersectionManager = hit.collider.GetComponent<IntersectionManager>();
                if (intersectionManager != null)
                {
                    if (intersectionManager.CanEnterIntersection(this))
                    {
                        m_WaitingAtIntersection = null;
                        intersectionManager.VehicleEnteredIntersection(this);
                        //positionOffset = transform.forward * moveDelta;
                        //transform.position += transform.forward * moveDelta;
                        //return;
                    }
                    else
                    {
                        float currentDistance = hit.distance;
                        //moveDelta = m_LinecastHitAdjustOffset
                        positionOffset = -transform.forward * (m_LinecastHitAdjustOffset - currentDistance);
                        //transform.position = transform.position + (-transform.forward * (m_LinecastHitAdjustOffset - currentDistance));//  moveDelta * hit.distance * moveDelta;
                        m_WaitingAtIntersection = intersectionManager;
                        m_IsStopped = true;
                        ResetCurrentSpeed();
                        //return;
                    }
                }  
            }

            //transform.position += transform.forward * moveDelta;
            transform.position += positionOffset;
        }   
    }


    public void ResetCurrentSpeed()
    {
        m_CurrentSpeed = 0f;
        m_AccelerateTime = 0f;
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

    public Vector3 GetForwardColliderPosition()
    {
        Vector3 worldColliderCenter = transform.TransformPoint(m_Collider.center);
        return worldColliderCenter + transform.forward * m_Collider.size.z * 0.5f;
    }

    private void OnDestroy()
    {
        EventManager.RaiseOnVehicleDestroyed(this);
        //OnVehicleDestroyed?.Invoke(this);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.white;
        //Vector3 colliderCenter = transform.TransformPoint(m_Collider.center);
        //Vector3 colliderFront = colliderCenter + transform.forward * m_Collider.size.z * 0.5f;
        //Gizmos.DrawSphere(colliderFront, 0.25f);

        //Ray r = new Ray(colliderFront, transform.forward * 10f);
        //Gizmos.DrawRay(r);

        Gizmos.color = Color.green;
        Vector3 maxFollowPosition = GetForwardColliderPosition() + transform.forward * m_FollowDistance;
        Gizmos.DrawSphere(maxFollowPosition, 0.25f);


        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(GetForwardColliderPosition(), 0.25f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(GetForwardColliderPosition(), maxFollowPosition);

        //Vector3 lineCastStart = transform.position + (transform.forward * (m_FollowDistance + m_Collider.size.z / 2f));// + (m_Collider.bounds.center * m_Collider.size.z / 2f);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(lineCastStart, 0.25f);

        if (hitLocation != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitLocation, 0.5f);
        }
    }
}
