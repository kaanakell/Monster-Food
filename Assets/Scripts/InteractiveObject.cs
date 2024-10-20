using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public GameObject interactionPanel;
    private bool playerInRange = false;

    private void Start()
    {
        // Ensure the panel is hidden at start
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for E key press when player is in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TogglePanel();
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

    private void TogglePanel()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(!interactionPanel.activeSelf);
        }
    }
}
