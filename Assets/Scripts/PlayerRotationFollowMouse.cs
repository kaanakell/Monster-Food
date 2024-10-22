using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationFollowMouse : MonoBehaviour
{
    void Update()
    {
        // Get the mouse position in the world
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Calculate the direction from the light to the mouse position
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        // Calculate the angle to rotate the light towards the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation of the light
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
