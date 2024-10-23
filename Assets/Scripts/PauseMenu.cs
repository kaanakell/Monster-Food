using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
    }

    void Update()
    {
        // Pause the game when "Escape" is pressed (optional)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        Time.timeScale = 1f; // Resume time
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        Time.timeScale = 0f; // Pause time
        isPaused = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Make sure the game is unpaused when restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current level
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Make sure the game is unpaused when going to the menu
        SceneManager.LoadScene("MainMenu"); // Load the main menu (use your main menu scene name)
    }
}
