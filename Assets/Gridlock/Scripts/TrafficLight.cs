using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    [SerializeField] public WorldTravelDirection m_TrafficFlowDirection;
    [SerializeField] private GameObject m_RedLight;
    [SerializeField] private GameObject m_GreenLight;

    public void SetGreenLight()
    {
        m_GreenLight.SetActive(true);
        m_RedLight.SetActive(false);
    }

    public void SetRedLight()
    {
        m_RedLight.SetActive(true);
        m_GreenLight.SetActive(false);
    }
}
