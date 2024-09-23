using UnityEngine;

public class MainMenuBackground : MonoBehaviour
{
    public AudioClip backgroundMusic; // Reference to the music file
    private AudioSource audioSource;  // Reference to the AudioSource component

    // Start is called before the first frame update
    void Start()
    {
        // Create an AudioSource component dynamically and attach it to the same GameObject
        audioSource = gameObject.AddComponent<AudioSource>();

        // Assign the background music to the AudioSource
        audioSource.clip = backgroundMusic;

        // Set the audio to loop
        audioSource.loop = true;

        // Start playing the music
        audioSource.Play();
    }
}
