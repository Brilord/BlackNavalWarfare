using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float leftBoundary = -19f;  // Left limit of movement
    public float rightBoundary = 10f;  // Right limit of movement
    public float bottomBoundary = -5f; // Bottom limit of movement
    public float topBoundary = 5f;     // Top limit of movement

    // Update is called once per frame
    void Update()
    {
        // Get input from the horizontal and vertical axes
        float moveDirectionX = Input.GetAxis("Horizontal");  // Left/Right
        float moveDirectionY = Input.GetAxis("Vertical");    // Up/Down

        // Calculate new position based on input
        Vector3 newPosition = transform.position + new Vector3(moveDirectionX, moveDirectionY, 0) * moveSpeed * Time.deltaTime;

        // Clamp the new position within the boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, leftBoundary, rightBoundary);
        newPosition.y = Mathf.Clamp(newPosition.y, bottomBoundary, topBoundary);

        // Apply the clamped position to the object
        transform.position = newPosition;
    }
}
