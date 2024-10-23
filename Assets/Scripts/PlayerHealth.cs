using UnityEngine;
using System.Collections;
using UnityEngine.UI;  // Include this for working with UI elements like Image

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float damageFromEnemy = 10f;
    public float damageInterval = 0.5f; // Time between damage applications when in contact
    public HealthBar healthBar;
    public float gameOverDelay = 2f;
    public float damageOverTime;
    public float fadeDuration = 2f; // Time to fully fade in

    private Animator animator;
    private bool isDead = false;
    private float nextDamageTime = 0f;

    // Reference to the GameOver UI panel and its Image component
    public GameObject gameOverUI;
    private Image gameOverImage;

    public Button mainMenuButton;  // Add reference to the Main Menu button
    public Button restartButton;   // Add reference to the Restart button
    public float buttonMoveDelay = 2f;  // Delay before moving buttons
    public float buttonMoveDuration = 1f;  // Duration of button animation
    private Vector3 offScreenPosition; // Initial position of the buttons
    private Vector3 onScreenPosition;  // Final position of the buttons

    // Audio-related fields
    public AudioSource audioSource;
    public AudioClip hitSound; // Add the hit sound

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
        InvokeRepeating("DamageOverTime", 0, 1f);

        // Get the Image component of the GameOverUI panel
        if (gameOverUI != null)
        {
            gameOverImage = gameOverUI.GetComponent<Image>();
            gameOverImage.color = new Color(gameOverImage.color.r, gameOverImage.color.g, gameOverImage.color.b, 0f); // Set alpha to 0
            gameOverUI.SetActive(false);  // Initially hide the panel
        }

        // Initialize the AudioSource if not already set
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        // Check for continuous collision with enemies
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") && Time.time >= nextDamageTime)
            {
                TakeDamage(damageFromEnemy);
                nextDamageTime = Time.time + damageInterval;
                break; // Only take damage from one enemy at a time
            }
        }
    }

    private void DamageOverTime()
    {
        currentHealth -= damageOverTime;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Invoke("Die", 1f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        // Play hit sound
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player died!");
        GetComponent<Collider2D>().enabled = false;

        // Start game over sequence
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(gameOverDelay);

        // Activate the GameOver UI and start the fade-in
        DisplayGameOverUI();

        // Then stop the time after the panel starts fading in
        Time.timeScale = 0;

        yield return new WaitForSeconds(buttonMoveDelay);  // Wait before moving buttons

        // Animate buttons into view
        StartCoroutine(MoveButton(mainMenuButton, onScreenPosition));
        StartCoroutine(MoveButton(restartButton, onScreenPosition));
    }

    IEnumerator MoveButton(Button button, Vector3 targetPosition)
    {
        Vector3 startPosition = button.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < buttonMoveDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;  // Ensure smooth animation even when time is paused
            button.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / buttonMoveDuration);
            yield return null;
        }

        button.transform.position = targetPosition;  // Ensure exact final position
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
            elapsedTime += Time.unscaledDeltaTime;  // Continue during Time.timeScale = 0
            float newAlpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            gameOverImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha); // Set new alpha
            yield return null;
        }
    }
}
