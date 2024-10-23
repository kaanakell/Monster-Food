using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public PlayerCombat playerCombat;
    public Animator animator;
    private string currentState;

    public AudioClip footstepSound; // Footstep sound
    private AudioSource audioSource; // Audio source to play the sound
    private float footstepTimer = 0f; // Timer to control footstep frequency
    public float footstepDelay = 0.5f; // Delay between footsteps

    //Animation States
    const string PLAYER_IDLE = "PlayerIdle";
    const string PLAYER_RUN_RIGHT = "PlayerRunRight";
    const string PLAYER_RUN_LEFT = "PlayerRunLeft";
    const string PLAYER_RUN_UP = "PlayerRunUp";
    const string PLAYER_RUN_DOWN = "PlayerRunDown";

    private bool isIdle;

    private DropItem currentDroppedItem;
    private bool itemInRange = false;

    private bool isPanelOpen = false;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //Cursor.visible = false;
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetFloat("Speed", rb.velocity.magnitude);
        Move();
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

        if (Input.GetKeyDown(KeyCode.Tab))
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

        AddItemToInventory();
    }

    public void Move()
    {
        // Movement
        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movSpeed;

        // Update the velocity of the rigidbody based on the input
        rb.velocity = new Vector2(speedX, speedY);
        animator.SetFloat("Speed", rb.velocity.magnitude);

        // Determine animation state based on movement direction
        if (speedX < 0)
        {
            // Moving left
            ChangeAnimationState(PLAYER_RUN_LEFT);

            //transform.localScale = new Vector2(-1, 1); // Flip the sprite to the left
        }
        else if (speedX > 0)
        {
            // Moving right
            ChangeAnimationState(PLAYER_RUN_RIGHT);
            //transform.localScale = new Vector2(1, 1); // Ensure sprite faces right
        }
        else if (speedY > 0)
        {
            // Moving up
            ChangeAnimationState(PLAYER_RUN_UP);
        }
        else if (speedY < 0)
        {
            // Moving down
            ChangeAnimationState(PLAYER_RUN_DOWN);
        }
        else
        {
            // Idle state (no movement)
            ChangeAnimationState(PLAYER_IDLE);
        }

        // Handle footstep sound
        if (rb.velocity.magnitude > 0 && footstepTimer <= 0f)
        {
            PlayFootstepSound();
            footstepTimer = footstepDelay; // Reset the footstep timer
        }

        // Decrease the footstep timer over time
        if (footstepTimer > 0)
        {
            footstepTimer -= Time.deltaTime;
        }

    }

    void PlayFootstepSound()
    {
        if (footstepSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(footstepSound); // Play the footstep sound
        }
    }

    public void OnAttackFinished()
    {
        currentState = PlayerCombat.PLAYER_ATTACK_DOWN;
        playerCombat.FinishAttack();
        // Determine animation state based on movement direction
        if (speedX < 0)
        {
            // Moving left
            ChangeAnimationState(PLAYER_RUN_LEFT);
            //transform.localScale = new Vector2(-1, 1); // Flip the sprite to the left
        }
        else if (speedX > 0)
        {
            // Moving right
            ChangeAnimationState(PLAYER_RUN_RIGHT);
            //transform.localScale = new Vector2(1, 1); // Ensure sprite faces right
        }
        else if (speedY > 0)
        {
            // Moving up
            ChangeAnimationState(PLAYER_RUN_UP);
        }
        else if (speedY < 0)
        {
            // Moving down
            ChangeAnimationState(PLAYER_RUN_DOWN);
        }
        else
        {
            // Idle state (no movement)
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    public void ChangeAnimationStateAccordingToMovement()
    {
        float speedX = rb.velocity.x;
        float speedY = rb.velocity.y;

        if (speedX < 0)
        {
            ChangeAnimationState(PLAYER_RUN_LEFT);
            //transform.localScale = new Vector2(-1, 1); // Face left
        }
        else if (speedX > 0)
        {
            ChangeAnimationState(PLAYER_RUN_RIGHT);
            //transform.localScale = new Vector2(1, 1); // Face right
        }
        else if (speedY > 0)
        {
            ChangeAnimationState(PLAYER_RUN_UP);
        }
        else if (speedY < 0)
        {
            ChangeAnimationState(PLAYER_RUN_DOWN);
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE); // Idle if not moving
        }
    }


    public void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }

    void TogglePanel()
    {
        if (inventoryPanel != null)
        {
            isPanelOpen = !isPanelOpen;
            inventoryPanel.SetActive(isPanelOpen);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            currentDroppedItem = collision.GetComponent<DropItem>();
            itemInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (itemInRange && collision.CompareTag("Item"))
        {
            currentDroppedItem = null;
            itemInRange = false;
        }
    }

    private void AddItemToInventory()
    {
        // Check for E key press when player is in range
        if (itemInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (currentDroppedItem != null)
            {
                inventory.Add(currentDroppedItem.Item, currentDroppedItem.Quantity);
                Destroy(currentDroppedItem.gameObject);
                itemInRange = false;
            }
        }
    }
}