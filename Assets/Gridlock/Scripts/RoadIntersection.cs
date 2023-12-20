using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadIntersection : MonoBehaviour
{
    [SerializeField] private IntersectionManager m_IntersectionManager;
    [SerializeField] private BoxCollider[] m_TrafficFlowColliders_0;
    [SerializeField] private BoxCollider[] m_TrafficFlowColliders_1;


    bool m_TrafficFlowActive_0;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void SwitchTrafficFlowColliders()
    {
        m_IntersectionManager.TrySwitchStoplights();

        //m_TrafficFlowActive_0 = !m_TrafficFlowActive_0;
        //Debug.Log($"Traffic Flow Now:{m_TrafficFlowActive_0}");

        //foreach(BoxCollider collider in m_TrafficFlowColliders_0)
        //{
        //    collider.enabled = m_TrafficFlowActive_0;
        //}

        //foreach (BoxCollider collider in m_TrafficFlowColliders_1)
        //{
        //    collider.enabled = !m_TrafficFlowActive_0;
        //}

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_TrafficFlowActive_0)
        {
            foreach (BoxCollider collider in m_TrafficFlowColliders_0)
            {
                Vector3 center = collider.transform.position;
                Vector3 size = collider.transform.TransformVector(collider.bounds.size);
                Gizmos.DrawWireCube(center, size);
            }
        }
        else
        {
            foreach (BoxCollider collider in m_TrafficFlowColliders_1)
            {
                Vector3 center = collider.transform.position;
                Vector3 size = collider.transform.TransformVector(collider.bounds.size);
                Gizmos.DrawWireCube(center, size);
            }
        }
        

    }
}
