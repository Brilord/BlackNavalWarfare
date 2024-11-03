using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private int bulletDamage = 10;      // Damage dealt by the bullet

    [SerializeField]
    private float bulletSpeed = 10f;    // Speed at which the bullet travels

    [SerializeField]
    private float maxRange = 20f;       // Maximum range of the bullet

    private Vector2 startPosition;      // The position where the bullet was instantiated

    void Start()
    {
        // Store the initial position of the bullet
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the bullet forward based on its speed and direction
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);

        // Check if the bullet has exceeded its maximum range
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxRange)
        {
            Destroy(gameObject);
        }
    }

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
