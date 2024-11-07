using UnityEngine;

public class EnemyGunboatScript : MonoBehaviour
{
    public int health = 40;                  // Health of the enemy gunboat
    public GameObject bulletPrefab;           // The bullet prefab to shoot
    public Transform firePoint;               // The point where bullets will spawn
    public float fireRate = 0.2f;             // Fire rate for the enemy machine gun
    private float fireTimer = 0f;             // Timer for firing bullets
    public float bulletSpeed = 10f;           // Speed of the bullet

    public float moveSpeed = 1f;              // Speed for moving left and right
    private bool movingRight = true;          // Direction of movement
    private float currentSpeed;               // Current movement speed

    private Vector3 originalScale;            // Store the original scale of the object

    public int cost = 50;                     // Cost of the enemy gunboat (if relevant for your game)

    public float detectionRange = 5f;         // Range within which to detect friendly units
    private GameObject targetFriendly = null; // The detected friendly unit within range
    public float stopDuration = 1f;           // Duration to come to a stop when target detected
    private float stopTimer = 0f;             // Timer to track stopping duration

    void Start()
    {
        // Store the initial scale of the object
        originalScale = transform.localScale;
        currentSpeed = moveSpeed;             // Initialize the current speed to moveSpeed
    }

    void Update()
    {
        // Detect any friendly unit within the detection range
        DetectFriendly();

        // Handle shooting if a target is within detection range
        if (targetFriendly != null)
        {
            // Gradually slow down to stop over the defined stop duration
            stopTimer += Time.deltaTime;
            currentSpeed = Mathf.Lerp(moveSpeed, 0f, stopTimer / stopDuration);

            // Shoot at the detected target
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
        else
        {
            // Reset the stop timer and speed if no target is detected
            stopTimer = 0f;
            currentSpeed = moveSpeed;
        }

        // Check if the gunboat's health reaches zero
        if (health <= 0)
        {
            Destroy(gameObject);  // Destroy the gunboat if health reaches zero
        }

        // Move the gunboat with the current speed
        Move();
    }

    void Move()
    {
        // Move in the current direction
        if (movingRight)
        {
            transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
        }

        // Switch direction after reaching certain boundaries
        if (transform.position.x >= 10f && movingRight)
        {
            movingRight = false;
            Flip();
        }
        else if (transform.position.x <= -10f && !movingRight)
        {
            movingRight = true;
            Flip();
        }
    }

    void Flip()
    {
        // Flip the gunboat by changing its scale on the X axis
        Vector3 newScale = originalScale;
        newScale.x *= -1; // Invert the X scale to flip the sprite
        transform.localScale = newScale;
    }

    void Shoot()
    {
        // Check if bulletPrefab is assigned
        if (bulletPrefab != null)
        {
            // Instantiate a bullet at the fire point
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Add velocity to the bullet to make it move
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * bulletSpeed;
            }
        }
        else
        {
            Debug.LogWarning("Bullet prefab is not assigned or has been destroyed!");
        }
    }

    // Call this function when the gunboat takes damage
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void DetectFriendly()
    {
        // Clear the target if it's null
        if (targetFriendly != null && targetFriendly == null)
        {
            targetFriendly = null;
            return;
        }

        // Find the nearest friendly unit within range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Friendly"))
            {
                targetFriendly = hit.gameObject;
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a sphere in the editor to visualize the detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
