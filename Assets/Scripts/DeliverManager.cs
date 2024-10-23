using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeliverManager : BaseInventory
{
    public int[] ids;
    private int currentId;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip submitSound;
    [SerializeField] private AudioClip newOrderSound;

    [SerializeField] private Button submitButton;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DataArrayObject dataArrayObject;

    // Game Over UI handling
    public GameObject gameOverUI;
    private Image gameOverImage;
    public float fadeDuration = 2f;    // Duration for the fade-in effect
    public Timer timer;                // Reference to the Timer script to add time
    public PlayerHealth playerHealth;  // Reference to the PlayerHealth script to add health
    public float healthGain = 20f;     // Amount of health to gain on a correct delivery
    public float timeGain = 10f;       // Amount of time to gain on a correct delivery

    private void Awake()
    {
        submitButton.onClick.AddListener(OnSubmitButtonClicked);
        SetCurrentId();
        VisualizeSetCurrentId();

        // Initialize Game Over UI and fade-in logic
        if (gameOverUI != null)
        {
            gameOverImage = gameOverUI.GetComponent<Image>();
            gameOverImage.color = new Color(gameOverImage.color.r, gameOverImage.color.g, gameOverImage.color.b, 0f); // Set initial alpha to 0
            gameOverUI.SetActive(false);  // Hide the panel at the start
        }
    }

    private void OnSubmitButtonClicked()
    {
        // Play submit button sound
        audioSource.PlayOneShot(submitSound);
        Submit();
    }

    public void Submit()
    {
        var slotItem = items[0].GetItem();
        if (slotItem.Id == currentId)
        {
            // Ids match, give health and time boost
            Debug.Log("Ids match: " + slotItem.Id + " == " + currentId);

            // Gain health and time
            playerHealth.currentHealth = Mathf.Min(playerHealth.maxHealth, playerHealth.currentHealth + healthGain); // Clamp health to max
            playerHealth.healthBar.SetHealth(playerHealth.currentHealth);  // Update health bar

            timer.timeSlider.value = Mathf.Min(timer.timeSlider.maxValue, timer.timeSlider.value + timeGain); // Clamp time to max value

            // Reset the current ID and remove the submitted item
            SetCurrentId();
            VisualizeSetCurrentId();
            Remove(items[0].GetItem());
        }
        else
        {
            // Ids don't match, trigger Game Over
            Debug.Log("Ids don't match: " + slotItem.Id + " != " + currentId);
            StartCoroutine(TriggerGameOver());
        }
    }

    IEnumerator TriggerGameOver()
    {
        // Activate the GameOver UI and start the fade-in
        DisplayGameOverUI();

        // Wait for fade-in to finish before stopping the time
        yield return new WaitForSecondsRealtime(fadeDuration);

        // Stop the game
        Time.timeScale = 0;
    }

    void DisplayGameOverUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);  // Show the panel
            StartCoroutine(FadeInGameOverUI());  // Start fading in
        }
        else
        {
            Debug.LogWarning("GameOverUI not found in the scene!");
        }
    }

    IEnumerator FadeInGameOverUI()
    {
        float elapsedTime = 0f;
        Color originalColor = gameOverImage.color;

        // Gradually increase the alpha over the specified duration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;  // Continue even when Time.timeScale = 0
            float newAlpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            gameOverImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha); // Set new alpha
            yield return null;
        }
    }

    public bool AddSubmitItem(ItemClass item, int quantity)
    {
        SlotClass slots = Contains(item);
        if (slots != null && slots.GetItem().isStackable)
        {
            slots.AddQuantity(quantity);
        }
        else
        {
            items[0].AddItem(item, quantity);
        }
        RefreshUI();
        return true;
    }

    public void SetCurrentId()
    {
        var random = UnityEngine.Random.Range(0, ids.Length);
        currentId = random;
        // Play new order sound
        audioSource.PlayOneShot(newOrderSound);
    }

    public void VisualizeSetCurrentId()
    {
        spriteRenderer.sprite = dataArrayObject.dataArray[currentId].itemIcon;
    }
}
