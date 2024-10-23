using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public GameObject interactionPanel;
    public AudioSource pickupSound;  // Reference to the AudioSource component for the pickup sound
    private bool playerInRange = false;

    private void Start()
    {
        // Ensure the panel is hidden at start
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }

        // Check if pickup sound source is set, otherwise find the AudioSource attached to this GameObject
        if (pickupSound == null)
        {
            pickupSound = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Check for E key press when player is in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TogglePanel();

            PlayPickupSound();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            // Hide panel when player leaves range
            if (interactionPanel != null)
            {
                interactionPanel.SetActive(false);
            }
        }
    }

    private void PlayPickupSound()
    {
        if (pickupSound != null)
        {
            pickupSound.Play();  // Play the pickup sound
        }
        else
        {
            Debug.LogWarning("Pickup sound AudioSource is not assigned.");
        }
    }

    private void TogglePanel()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(!interactionPanel.activeSelf);
        }
    }
}
