using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;

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
