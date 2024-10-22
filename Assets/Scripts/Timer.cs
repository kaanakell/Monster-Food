using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider timeSlider; // Reference to the Slider in the Canvas
    public float totalTime = 30f; // Total time for the timer (seconds)
    private float timeRemaining;

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
        }
        else
        {
            // Time's up! Handle the end of the timer here.
            Debug.Log("Time's up!");
        }
    }
}
