using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource; // Audio source for the music
    public AudioSource ambienceSource; // Audio source for the ambient sound
    public float fadeDuration = 2f; // Duration of fade (in seconds)

    private Dictionary<AudioClip, float> musicPositions = new Dictionary<AudioClip, float>(); // Store last played position of each music clip
    private AudioClip currentMusicClip;
    private AudioClip currentAmbienceClip;

    // Call this to transition to a new area's music and ambient sound
    public void TransitionArea(AudioClip newMusicClip, AudioClip newAmbienceClip)
    {
        StartCoroutine(FadeOutAndSwitch(newMusicClip, newAmbienceClip));
    }

    private IEnumerator FadeOutAndSwitch(AudioClip newMusicClip, AudioClip newAmbienceClip)
    {
        float elapsedTime = 0f;

        // Fade out the current music and ambient sound
        if (musicSource.isPlaying)
        {
            // Save the current playback position of the current music clip
            if (currentMusicClip != null)
            {
                musicPositions[currentMusicClip] = musicSource.time;
                Debug.Log($"Saving playback time for {currentMusicClip.name}: {musicSource.time}");
            }

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                ambienceSource.volume = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                yield return null;
            }

            musicSource.Stop();
            ambienceSource.Stop();
        }

        // Switch to the new music and ambient sound clips
        currentMusicClip = newMusicClip;
        currentAmbienceClip = newAmbienceClip;

        // Assign new clips
        musicSource.clip = newMusicClip;
        ambienceSource.clip = newAmbienceClip;

        // Resume the music from its last saved position if available
        if (musicPositions.ContainsKey(newMusicClip))
        {
            float savedTime = musicPositions[newMusicClip];
            musicSource.time = savedTime;
            Debug.Log($"Resuming {newMusicClip.name} from position: {musicSource.time}");
        }
        else
        {
            musicSource.time = 0f; // Start from the beginning if no saved position is available
        }

        // Play the new clips
        musicSource.Play();
        ambienceSource.Play();

        // Fade in the new music and ambient sound
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            ambienceSource.volume = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure both audio sources are fully restored to their full volume
        musicSource.volume = 1f;
        ambienceSource.volume = 1f;
    }
}
