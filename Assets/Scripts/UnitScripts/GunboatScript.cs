using UnityEngine;

public class GunboatScript : MonoBehaviour
{
    public int health = 40;
    public GameObject bulletPrefab;  // The bullet prefab to shoot
    public Transform firePoint;      // The point where bullets will spawn
    public float fireRate = 0.2f;    // Fire rate for the machine gun
    private float fireTimer = 0f;    // Timer for firing bullets
    public float bulletSpeed = 10f;  // Speed of the bullet

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
