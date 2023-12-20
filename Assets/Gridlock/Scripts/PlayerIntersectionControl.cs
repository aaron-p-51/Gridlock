using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntersectionControl : MonoBehaviour
{
    [SerializeField] private IntersectionManager m_IntersectionManager;

    public void SwitchTrafficFlow()
    {
        m_IntersectionManager.TrySwitchTrafficFlow();
    }
    
}
