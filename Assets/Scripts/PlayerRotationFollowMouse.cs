using Unity.VisualScripting;
using UnityEngine;

public class PlayerRotationFollowMouse : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        // Reference to the Cinemachine virtual camera
        cam = Camera.main;  // Assuming the main camera is using Cinemachine
    }

    void Update()
    {
        // Get the mouse position in world space using the main Cinemachine camera
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0;  // Ensure z-axis is 0 for 2D space
        
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the direction from the player to the mouse
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;

        transform.up = direction;

        // Calculate the angle in degrees to rotate towards the mouse
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the player object on the Z axis (2D space)
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
