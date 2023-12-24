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


    public float m_TimeWaitingAtIntersection { get; private set; }
    public Vector3 m_LocationWhenEnterendLastIntersection { get; private set; }

    
    public float m_CurrentSpeed { get; private set; }
    private float m_LerpAccelerateTime = 0f;


    public WorldTravelDirection m_WorldTravelDirection { get; private set; }

    public bool m_Scored = false;

    private IntersectionManager m_WaitingAtIntersection;

    public bool IsStopped => (m_Speed <= 0f);

    private void Awake()
    {
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
    }

    private void Move()
    {
        if (WaitAtIntersection()) return;

        UpdateCurrentSpeed();

        float moveDelta = Time.deltaTime * m_CurrentSpeed;
        Vector3 positionOffset = transform.forward * moveDelta;
        Ray ray = new Ray(GetForwardColliderPosition(), transform.forward);

        // look for Vehicle
        float vehicleFollowtraceLength = m_FollowDistance + moveDelta;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, vehicleFollowtraceLength, m_LayerMaskVehicle))
        {
            float seperation = m_FollowDistance - hit.distance;
            positionOffset = -transform.forward * seperation;
            ResetCurrentSpeed();
        }

        // Look for intersection
        if (Physics.Raycast(ray, out hit, moveDelta, m_LayerMaskIntersection))
        {
            IntersectionManager intersectionManager = hit.collider.GetComponent<IntersectionManager>();
            if (intersectionManager != null)
            {
                if (intersectionManager.CanEnterIntersection(this))
                {
                    m_WaitingAtIntersection = null;
                    intersectionManager.VehicleEnteredIntersection(this);
                }
                else
                {
                    positionOffset = -transform.forward * (m_LinecastHitAdjustOffset - hit.distance);
                    m_WaitingAtIntersection = intersectionManager;
                    ResetCurrentSpeed();
                }
            }
        }

        transform.position += positionOffset;
    }

    public bool WaitAtIntersection()
    {
        if (m_WaitingAtIntersection == null) return false;
        return !m_WaitingAtIntersection.CanEnterIntersection(this);
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

    public void ResetCurrentSpeed()
    {
        m_CurrentSpeed = 0f;
        m_AccelerateTime = 0f;
    }   

    public Vector3 GetForwardColliderPosition()
    {
        Vector3 worldColliderCenter = transform.TransformPoint(m_Collider.center);
        return worldColliderCenter + transform.forward * m_Collider.size.z * 0.5f;
    }

    private void OnDestroy()
    {
        EventManager.RaiseOnVehicleDestroyed(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 maxFollowPosition = GetForwardColliderPosition() + transform.forward * m_FollowDistance;
        Gizmos.DrawSphere(maxFollowPosition, 0.25f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(GetForwardColliderPosition(), 0.25f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(GetForwardColliderPosition(), maxFollowPosition);
    }
}
