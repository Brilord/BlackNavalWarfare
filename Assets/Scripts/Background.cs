using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform cameraTransform;     // Reference to the camera
    private Vector2 minBounds;            // Minimum bounds of the background
    private Vector2 maxBounds;            // Maximum bounds of the background
    private float camHalfHeight;          // Half height of the camera
    private float camHalfWidth;           // Half width of the camera
    public AudioClip[] backgroundMusic;   // Array of background music clips
    private AudioSource audioSource;      // Reference to the AudioSource
    private int currentClipIndex = 0;     // Track the current clip index

    void Start()
    {
        // Calculate the bounds of the background (assuming it's a sprite)
        SpriteRenderer backgroundRenderer = GetComponent<SpriteRenderer>();
        Bounds backgroundBounds = backgroundRenderer.bounds;
        
        // Set the minimum and maximum bounds of the background
        minBounds = backgroundBounds.min;
        maxBounds = backgroundBounds.max;

        // Calculate half of the camera's height and width based on its orthographic size
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect; // Aspect ratio adjustment

        // Get the AudioSource component and set up the background music
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false; // Disable looping for individual clips
        audioSource.playOnAwake = false; // Don't play automatically on start

        PlayNextClip(); // Start playing the first clip
    }

    void LateUpdate()
    {
        // Ensure the camera doesn't move outside the background
        Vector3 newPosition = cameraTransform.position;

        // Clamp the camera's X position between the min and max bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);

        // Clamp the camera's Y position between the min and max bounds
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);

        // Set the camera's position to the clamped values
        cameraTransform.position = newPosition;

        // Check if the current clip has finished playing
        if (!audioSource.isPlaying)
        {
            PlayNextClip(); // Play the next clip
        }
    }

    void PlayNextClip()
    {
        if (backgroundMusic.Length == 0) return; // No clips available

        // Play the current clip in the array
        audioSource.clip = backgroundMusic[currentClipIndex];
        audioSource.Play();

        // Move to the next clip index, looping back to the start if necessary
        currentClipIndex = (currentClipIndex + 1) % backgroundMusic.Length;
    }
}
