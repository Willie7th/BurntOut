using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource source;
    private GameObject SoundManagerObj;

    // AudioSource for background music
    private AudioSource backgroundMusicSource;
    private AudioSource movementSource;
    private Coroutine fadeOutCoroutine;
    // Dictionary to store preloaded audio clips
    private Dictionary<string, AudioClip> preloadedClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        // Main audio source for sound effects
        source = GetComponent<AudioSource>();

        // Add a separate audio source for background music
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.loop = true; // Loop background music

        movementSource = gameObject.AddComponent<AudioSource>();

        SoundManagerObj = GameObject.Find("SoundManager");
        DontDestroyOnLoad(SoundManagerObj);

        // Optionally preload audio files here
        StartCoroutine(PreloadAudioClips());
    }

    // Coroutine to preload audio asynchronously
    private IEnumerator PreloadAudioClips()
    {
        string[] audioFiles = { "Audio/BackgroundMusic/LevelBackground", "Audio/BackgroundMusic/GameOver", "Audio/BackgroundMusic/LevelComplete1", "Audio/BackgroundMusic/MenuBackground"};

        foreach (string audioFileName in audioFiles)
        {
            ResourceRequest loadRequest = Resources.LoadAsync<AudioClip>(audioFileName);

            yield return loadRequest;

            if (loadRequest.asset != null)
            {
                preloadedClips[audioFileName] = loadRequest.asset as AudioClip;
                Debug.Log($"{audioFileName} preloaded.");
            }
            else
            {
                Debug.LogWarning($"Failed to preload audio: {audioFileName}");
            }
        }
    }

    // Play background music from preloaded clips
    public void PlayBackgroundMusic(string musicName, float volume)
    {
        if (preloadedClips.ContainsKey(musicName))
        {
            backgroundMusicSource.clip = preloadedClips[musicName];
            backgroundMusicSource.volume = volume;
            backgroundMusicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Background music {musicName} not found in preloaded clips.");
        }
    }

    // Stop the background music
    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }

    // Play preloaded sound effect
    public void PlayPreloadedSound(string clipName, float volume)
    {
        if (preloadedClips.ContainsKey(clipName))
        {
            source.PlayOneShot(preloadedClips[clipName], volume);
        }
        else
        {
            Debug.LogWarning($"Audio clip {clipName} not found in preloaded clips.");
        }
    }

    // Play sound directly if not preloaded (for other sounds)
    public void PlaySound(AudioClip _sound, float volume)
    {
        source.PlayOneShot(_sound, volume);
    }

    public void PlayMovementSound(AudioClip _sound, float volume)
    {
        if (fadeOutCoroutine != null) // Stop fade-out if currently happening
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
            movementSource.volume = volume; // Reset volume in case it was reduced during fade-out
        }

        if (!movementSource.isPlaying)
        {
            movementSource.clip = _sound;
            movementSource.volume = volume;
            movementSource.loop = true;  // Loop the movement sound
            movementSource.Play();
        }
    }

    // Fade out movement sound over 2 seconds
    public void StopMovementSound()
    {
        if (movementSource.isPlaying)
        {
            if (fadeOutCoroutine != null)
                StopCoroutine(fadeOutCoroutine); // Ensure there's no conflicting fade-out

            fadeOutCoroutine = StartCoroutine(FadeOutMovementSound(2f));  // 2 seconds fade-out
        }
    }

    // Coroutine to gradually reduce volume and stop the movement sound
    private IEnumerator FadeOutMovementSound(float fadeDuration)
    {
        float startVolume = movementSource.volume;

        while (movementSource.volume > 0)
        {
            movementSource.volume -= startVolume * Time.deltaTime / fadeDuration;  // Gradually reduce volume
            yield return null;  // Wait for the next frame
        }

        movementSource.Stop();  // Stop the sound after fade-out
        movementSource.clip = null;  // Reset the clip
        movementSource.volume = startVolume;  // Reset the volume for next time the sound is played
    }
}
