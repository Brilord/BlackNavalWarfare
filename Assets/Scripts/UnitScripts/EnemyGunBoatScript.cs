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

    private Vector3 originalScale;            // Store the original scale of the object

    public int cost = 50;                     // Cost of the enemy gunboat (if relevant for your game)

    void Start()
    {
        // Store the initial scale of the object
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Handle shooting
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }

        // Check if the enemy gunboat's health reaches zero
        if (health <= 0)
        {
            Destroy(gameObject);  // Destroy the enemy gunboat if health is 0
        }

        // Move the enemy gunboat left and right
        Move();
    }

    void Move()
    {
        // Move the enemy gunboat in the current direction
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }

        // Switch direction after reaching a certain boundary (e.g., screen width)
        if (transform.position.x >= 10f && movingRight) // Adjust the boundary values as needed
        {
            movingRight = false;
            Flip(); // Flip the gunboat to face the opposite direction
        }
        else if (transform.position.x <= -10f && !movingRight) // Adjust the boundary values as needed
        {
            movingRight = true;
            Flip(); // Flip the gunboat to face the opposite direction
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
        // Null check to prevent instantiating a destroyed or null bulletPrefab
        if (bulletPrefab != null)
        {
            // Instantiate a bullet at the fire point
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            // Add force to the bullet to move it forward
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

    // Call this function when the enemy gunboat takes damage
    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
