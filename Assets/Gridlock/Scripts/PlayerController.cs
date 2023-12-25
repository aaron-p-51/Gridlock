using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerUIController;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private PlayerUIController m_PlayerUIController;

    public bool m_PlayerReady;

    private void Awake()
    {
        m_PlayerUIController.OnButtonClicked += HandleOnUIButtonClicked;
    }


    private void HandleOnUIButtonClicked(PlayerUIButtons button)
    {

    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Debug.Log($"Clicked: {hitInfo.collider.gameObject.name}");
                PlayerIntersectionControl playerIntersectionControl = hitInfo.collider.GetComponent<PlayerIntersectionControl>();
                if (playerIntersectionControl != null)
                {
                    playerIntersectionControl.SwitchTrafficFlow();
                }
            }
        }
    }
}
