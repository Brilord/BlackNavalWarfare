using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public Transform backgroundTransform; // Reference to the background

    private float leftBoundary;  // Left limit of movement
    private float rightBoundary; // Right limit of movement
    private float bottomBoundary; // Bottom limit of movement
    private float topBoundary;    // Top limit of movement
    private float camHalfHeight;  // Half the height of the camera
    private float camHalfWidth;   // Half the width of the camera

    private float initialX = -13.63f; // Hardcoded initial X position of the camera
    private float initialY = -2.03f;  // Hardcoded initial Y position of the camera

    void Start()
    {
        // Calculate half of the camera's height and width
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

        // Get the background bounds
        SpriteRenderer backgroundRenderer = backgroundTransform.GetComponent<SpriteRenderer>();
        Bounds backgroundBounds = backgroundRenderer.bounds;

        // Set the boundaries based on background size
        leftBoundary = backgroundBounds.min.x + camHalfWidth;
        rightBoundary = backgroundBounds.max.x - camHalfWidth;
        bottomBoundary = backgroundBounds.min.y + camHalfHeight;
        topBoundary = backgroundBounds.max.y - camHalfHeight;

        // Directly set initial position to specified values, clamped within boundaries
        SetPosition(initialX, initialY);
    }

    // Method to set the camera position to a specific point
    public void SetPosition(float x, float y)
    {
        // Clamp the position within the boundaries
        float clampedX = Mathf.Clamp(x, leftBoundary, rightBoundary);
        float clampedY = Mathf.Clamp(y, bottomBoundary, topBoundary);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    void Update()
    {
        // Get input from the horizontal and vertical axes
        float moveDirectionX = Input.GetAxis("Horizontal");  // Left/Right
        float moveDirectionY = Input.GetAxis("Vertical");    // Up/Down

        // Calculate new position based on input
        Vector3 newPosition = transform.position + new Vector3(moveDirectionX, moveDirectionY, 0) * moveSpeed * Time.deltaTime;

        // Clamp the new position within the dynamic boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, leftBoundary, rightBoundary);
        newPosition.y = Mathf.Clamp(newPosition.y, bottomBoundary, topBoundary);

        // Apply the clamped position to the object
        transform.position = newPosition;
    }
}
