using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource source;
    private GameObject SoundManagerObj;

    // AudioSource for background music
    private AudioSource backgroundMusicSource;

    // Dictionary to store preloaded audio clips
    private Dictionary<string, AudioClip> preloadedClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        // Main audio source for sound effects
        source = GetComponent<AudioSource>();
        
        // Add a separate audio source for background music
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.loop = true; // Loop background music

        SoundManagerObj = GameObject.Find("SoundManager");
        DontDestroyOnLoad(SoundManagerObj);

        // Optionally preload audio files here
        StartCoroutine(PreloadAudioClips());
    }

    // Coroutine to preload audio asynchronously
    private IEnumerator PreloadAudioClips()
    {
        // List of audio files to preload from BackgroundMusic folder (inside Resources)
        string[] audioFiles = { "Audio/BackgroundMusic/LevelBackground", "Audio/BackgroundMusic/GameOver", "Audio/BackgroundMusic/LevelComplete1", "Audio/BackgroundMusic/MenuBackground"}; // Update with your music file names

        foreach (string audioFileName in audioFiles)
        {
            ResourceRequest loadRequest = Resources.LoadAsync<AudioClip>(audioFileName);

            // Wait until the audio clip is loaded
            yield return loadRequest;

            // Add the preloaded clip to the dictionary if loading was successful
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
}
