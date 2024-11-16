using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private int bulletDamage = 1;      // Damage dealt by the bullet

    [SerializeField]
    private float bulletSpeed = 10f;    // Speed at which the bullet travels

    [SerializeField]
    private float maxRange = 20f;       // Maximum range of the bullet

    [SerializeField]
    private AudioClip bulletSpawnSound; // Sound to play when the bullet is spawned

    private Vector2 startPosition;      // The position where the bullet was instantiated

    void Start()
    {
        // Store the initial position of the bullet
        startPosition = transform.position;

        // Play the bullet spawn sound
        if (bulletSpawnSound != null)
        {
            AudioSource.PlayClipAtPoint(bulletSpawnSound, transform.position);
        }
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
        // Check if the bullet hit an enemy ship
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            // Apply damage to the enemy
            enemy.TakeDamage(bulletDamage);
        }
        EnemyBaseScript enemyBase = collision.gameObject.GetComponent<EnemyBaseScript>();
        if (enemyBase != null)
        {
            // Apply damage to the enemy base
            enemyBase.TakeDamage(bulletDamage);
        }

        // Check if the bullet hit an enemy gunboat
        EnemyGunboatScript gunboat = collision.gameObject.GetComponent<EnemyGunboatScript>();
        if (gunboat != null)
        {
            // Apply damage to the enemy gunboat
            gunboat.TakeDamage(bulletDamage);
        }

        // Check if the bullet hit an enemy cruiser
        EnemyCruiserScript cruiser = collision.gameObject.GetComponent<EnemyCruiserScript>();
        if (cruiser != null)
        {
            // Apply damage to the enemy cruiser
            cruiser.TakeDamage(bulletDamage);
        }

        // Check if the bullet hit an enemy anti-air ship
        EnemyAntiAirShipScript antiAirShip = collision.gameObject.GetComponent<EnemyAntiAirShipScript>();
        if (antiAirShip != null)
        {
            // Apply damage to the enemy anti-air ship
            antiAirShip.TakeDamage(bulletDamage);
        }
        EnemyBattleShipScript battleShip = collision.gameObject.GetComponent<EnemyBattleShipScript>();
        if (battleShip != null)
        {
            // Apply damage to the battleship
            battleShip.TakeDamage(bulletDamage);
        }

        // Destroy the bullet instance after it hits something
        Destroy(gameObject);
    }

    // Public method to get the bullet's damage value
    public int GetBulletDamage()
    {
        return bulletDamage;
    }

    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }
}
