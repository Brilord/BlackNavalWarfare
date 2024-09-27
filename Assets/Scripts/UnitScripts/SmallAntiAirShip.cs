using UnityEngine;

public class SmallAntiAirShip : MonoBehaviour
{
    public int health = 40;
    public GameObject bulletPrefab;  // The bullet prefab to shoot
    public Transform firePoint;      // The point where bullets will spawn
    public float fireRate = 0.2f;    // Fire rate for the machine gun
    private float fireTimer = 0f;    // Timer for firing bullets
    public float bulletSpeed = 10f;  // Speed of the bullet

    public float moveSpeed = 2f;     // Speed for moving left and right
    private bool movingRight = true; // Direction of movement

    void Start()
    {
        // Initialize any starting logic
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

        // Check if the gunboat's health reaches zero
        if (health <= 0)
        {
            Destroy(gameObject);  // Destroy the gunboat if health is 0
        }

        // Move the gunboat left and right
        Move();
    }

    void Move()
    {
        // Move the gunboat in the current direction
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }

        // Switch direction after reaching a certain boundary (e.g., screen width)
        if (transform.position.x >= 10f) // Adjust the boundary values as needed
        {
            movingRight = false;
        }
        else if (transform.position.x <= -10f) // Adjust the boundary values as needed
        {
            movingRight = true;
        }
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

    // Call this function when the gunboat takes damage
    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
