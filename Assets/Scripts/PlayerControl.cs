using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerControl : MonoBehaviour
{
    public float movSpeed;
    float speedX, speedY;
    public InventoryManager inventory;
    public VisualEffect vfxRenderer;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movSpeed;
        rb.velocity = new Vector2(speedX, speedY);

        // Visual Effect Position
        vfxRenderer.SetVector3("ColliderPos", transform.position);

        if (Input.GetKeyDown(KeyCode.E))
        {
            //use the item
            if (inventory.selectedItem != null)
            {
                inventory.selectedItem.Use(this);
            }
        }
    }
}
