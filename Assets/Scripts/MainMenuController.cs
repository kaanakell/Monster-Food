using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject creditsPanel;      // The credits panel
    public RectTransform creditsImage;   // The RectTransform of the Credits image
    public float animationTime = 0.5f;   // Duration of the animation
    private Vector2 offScreenPosition;   // Position of the credits image off-screen
    private Vector2 onScreenPosition;    // Position of the credits image on-screen
    private bool isCreditsVisible = false;  // Track if the credits panel is visible

    private void Start()
    {
        // Store the on-screen position (center) of the panel
        onScreenPosition = creditsImage.anchoredPosition;

        // Calculate the off-screen position (to the left)
        offScreenPosition = new Vector2(-Screen.width, creditsImage.anchoredPosition.y);

        // Initially position the credits image off-screen
        creditsImage.anchoredPosition = offScreenPosition;
    }

    // Method to handle the Start Game button
    public void StartGame()
    {
        // Load the game scene (replace "GameScene" with the actual scene name of your game)
        SceneManager.LoadScene("GameScene");
        creditsPanel.SetActive(false);
    }

    // Method to toggle the credits panel animation
    public void ToggleCredits()
    {
        if (!isCreditsVisible)
        {
            ShowCredits();
        }
        else
        {
            HideCredits();
        }
    }

    // Method to animate the credits panel in (from left)
    public void ShowCredits()
    {
        // Move the credits image from off-screen to the center
        LeanTween.move(creditsImage, onScreenPosition, animationTime).setEase(LeanTweenType.easeInOutQuad);
        isCreditsVisible = true;
    }

    // Method to animate the credits panel out (to left)
    public void HideCredits()
    {
        // Move the credits image off-screen to the left
        LeanTween.move(creditsImage, offScreenPosition, animationTime).setEase(LeanTweenType.easeInOutQuad);
        isCreditsVisible = false;
    }

    // Method to handle the Exit Game button
    public void ExitGame()
    {
        // Quit the game
        Debug.Log("Game is exiting...");  // This will work in the editor
        Application.Quit();  // This will work in the built game
    }
}
