using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider timeSlider;          // Reference to the Slider in the Canvas
    public float totalTime = 30f;      // Total time for the timer (seconds)
    private float timeRemaining;

    public GameObject gameOverUI;      // Reference to the GameOver UI panel
    private Image gameOverImage;       // Image component of the GameOver UI panel
    public float fadeDuration = 2f;    // Duration for the fade-in effect

    private bool isGameOver = false;   // To prevent multiple game over triggers

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clockSound;

    void Start()
    {
        // Initialize the timer
        timeRemaining = totalTime;

        // Set the slider’s max value to match the total time
        if (timeSlider != null)
        {
            timeSlider.maxValue = totalTime;
            timeSlider.value = totalTime; // Start at full value
        }

        // Get the Image component of the GameOverUI panel
        if (gameOverUI != null)
        {
            gameOverImage = gameOverUI.GetComponent<Image>();
            gameOverImage.color = new Color(gameOverImage.color.r, gameOverImage.color.g, gameOverImage.color.b, 0f); // Set initial alpha to 0
            gameOverUI.SetActive(false);  // Hide the panel at the start
        }
    }

    void Update()
    {
        // Reduce the timeRemaining value every frame
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Countdown by deltaTime (seconds)
            if (timeRemaining < 0)
            {
                timeRemaining = 0; // Clamp time to 0
            }

            // Update the slider to reflect the remaining time
            if (timeSlider != null)
            {
                timeSlider.value = timeRemaining;
            }
            audioSource.PlayOneShot(clockSound);
        }
        else if (!isGameOver)
        {
            // Time's up! Handle the end of the timer here.
            Debug.Log("Time's up!");
            StartCoroutine(HandleGameOver());
        }
    }

    IEnumerator HandleGameOver()
    {
        isGameOver = true;

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
}
