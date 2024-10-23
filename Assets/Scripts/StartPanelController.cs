using UnityEngine;
using UnityEngine.UI;

public class StartPanelController : MonoBehaviour
{
    public RectTransform panelTransform; // Reference to the panel's RectTransform
    public Vector2 offScreenPosition = new Vector2(-500, -500); // Start off screen (adjust to fit your needs)
    public Vector2 onScreenPosition = new Vector2(0, 0); // The position on the screen (adjust to fit your UI layout)
    public float animationDuration = 0.5f; // Duration for ease in/out
    public float waitTime = 2f; // Time to stay on screen before disappearing

    private void Start()
    {
        ShowPanel();
    }

    public void ShowPanel()
    {
        // Start by placing the panel off-screen
        panelTransform.anchoredPosition = offScreenPosition;

        // Move panel on screen (ease in)
        LeanTween.move(panelTransform, onScreenPosition, animationDuration)
            .setEase(LeanTweenType.easeOutExpo) // Customize the easing type
            .setOnComplete(() => StartCoroutine(HidePanelAfterDelay()));
    }

    private System.Collections.IEnumerator HidePanelAfterDelay()
    {
        // Wait for the specified time before hiding the panel
        yield return new WaitForSeconds(waitTime);

        // Move the panel off-screen (ease out)
        LeanTween.move(panelTransform, offScreenPosition, animationDuration)
            .setEase(LeanTweenType.easeInExpo); // Customize the easing type
    }
}

