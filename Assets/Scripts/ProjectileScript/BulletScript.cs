using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private int bulletDamage = 10;      // Damage dealt by the bullet

    [SerializeField]
    private float bulletSpeed = 10f;    // Speed at which the bullet travels

    private Rigidbody2D rb;

    void Start()
    {
        // Try to get the Rigidbody2D component attached to the bullet instance
        rb = GetComponent<Rigidbody2D>();

        // Check if the Rigidbody2D component exists
        if (rb != null)
        {
            // Apply velocity to the bullet to make it move
            rb.linearVelocity = transform.right * bulletSpeed;
        }
        else
        {
            Debug.LogError("No Rigidbody2D found on the bullet instance!");
        }
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

        // Destroy the bullet instance after it hits something
        Destroy(gameObject);
    }

    // Public method to get the bullet's damage value
    public int GetBulletDamage()
    {
        return bulletDamage;
    }
}
