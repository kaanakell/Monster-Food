using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public Material fovMaterial;
    public Transform player;

    void Update()
    {
        // Set the player's position in the shader
        Vector2 playerPos = new Vector2(player.position.x, player.position.y);
        fovMaterial.SetVector("_PlayerPosition", playerPos);
    }
}
