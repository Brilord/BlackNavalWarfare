using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField]
    private int bulletDamage = 10;       // Damage dealt by the enemy bullet

    [SerializeField]
    private float bulletSpeed = 10f;     // Speed at which the enemy bullet travels

    [SerializeField]
    private float maxRange = 20f;        // Maximum range of the enemy bullet

    private Vector2 startPosition;       // The position where the enemy bullet was instantiated

    void Start()
    {
        // Store the initial position of the bullet
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the bullet backward based on its speed and direction
        transform.Translate(Vector2.left * bulletSpeed * Time.deltaTime);

        // Check if the bullet has exceeded its maximum range
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet hit something that can take damage (e.g., a player)
        PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            // Apply damage to the player
            player.TakeDamage(bulletDamage);
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
