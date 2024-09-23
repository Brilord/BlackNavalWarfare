using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletLifetime = 5f;  // How long the bullet exists before being destroyed
    public int bulletDamage = 10;      // Damage dealt by the bullet
    public float bulletSpeed = 10f;    // Speed at which the bullet travels

    private Rigidbody2D rb;

    void Start()
    {
        // Try to get the Rigidbody2D component attached to the bullet
        rb = GetComponent<Rigidbody2D>();

        // Check if the Rigidbody2D component exists
        if (rb != null)
        {
            // Apply velocity to the bullet to make it move
            rb.linearVelocity = transform.right * bulletSpeed;
        }
        else
        {
            Debug.LogError("No Rigidbody2D found on the bullet!");
        }

        // Destroy the bullet after a certain time to prevent it from existing indefinitely
        Destroy(gameObject, bulletLifetime);
    }

    // This method is called when the bullet collides with another object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet hit something that can take damage (e.g., an enemy)
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            // Apply damage to the enemy
            enemy.TakeDamage(bulletDamage);
        }

        // Destroy the bullet after it hits something
        Destroy(gameObject);
    }
}
