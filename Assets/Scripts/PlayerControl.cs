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
    public GameObject inventoryPanel;
    public GameObject craftingPanel;
    public GameObject servePanel;
    public Animator animator;

    private bool isPanelOpen = false;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Cursor.visible = false;
        if(inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movSpeed;
        rb.velocity = new Vector2(speedX, speedY);

        animator.SetFloat("Speed", rb.velocity.magnitude);

        // Visual Effect Position
        vfxRenderer.SetVector3("ColliderPos", transform.position);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //use the item
            if (inventory.selectedItem != null)
            {
                inventory.selectedItem.Use(this);
            }
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePanel();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // This will be handled by the InteractiveObject script
            // We keep the existing item use logic here
            if (inventory.selectedItem != null)
            {
                inventory.selectedItem.Use(this);
            }
        }
    }

    void TogglePanel()
    {
        if(inventoryPanel != null)
        {
            isPanelOpen = !isPanelOpen;
            inventoryPanel.SetActive(isPanelOpen);
        }
    }
}
