using UnityEngine;

public class LargeBulletScript : MonoBehaviour
{
    [SerializeField]
    private int bulletDamage = 40;      // Increased damage dealt by the large bullet

    [SerializeField]
    private float bulletSpeed = 10f;    // Speed at which the large bullet travels

    [SerializeField]
    private float maxRange = 20f;       // Maximum range of the large bullet

    private Vector2 startPosition;      // The position where the large bullet was instantiated

    void Start()
    {
        // Store the initial position of the large bullet
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the large bullet forward based on its speed and direction
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);

        // Check if the large bullet has exceeded its maximum range
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the large bullet hit an enemy ship
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            // Apply damage to the enemy
            enemy.TakeDamage(bulletDamage);
            Debug.Log("Large Bullet hit an EnemyShip and dealt " + bulletDamage + " damage.");
        }

        // Check if the large bullet hit an enemy gunboat
        EnemyGunboatScript gunboat = collision.gameObject.GetComponent<EnemyGunboatScript>();
        if (gunboat != null)
        {
            // Apply damage to the enemy gunboat
            gunboat.TakeDamage(bulletDamage);
            Debug.Log("Large Bullet hit an EnemyGunboat and dealt " + bulletDamage + " damage.");
        }

        // Check if the large bullet hit an enemy cruiser
        EnemyCruiserScript cruiser = collision.gameObject.GetComponent<EnemyCruiserScript>();
        if (cruiser != null)
        {
            // Apply damage to the enemy cruiser
            cruiser.TakeDamage(bulletDamage);
            Debug.Log("Large Bullet hit an EnemyCruiser and dealt " + bulletDamage + " damage.");
        }

        // Check if the large bullet hit an enemy anti-air ship
        EnemyAntiAirShipScript antiAirShip = collision.gameObject.GetComponent<EnemyAntiAirShipScript>();
        if (antiAirShip != null)
        {
            // Apply damage to the enemy anti-air ship
            antiAirShip.TakeDamage(bulletDamage);
            Debug.Log("Large Bullet hit an EnemyAntiAirShip and dealt " + bulletDamage + " damage.");
        }

        // Destroy the large bullet instance after it hits something
        Destroy(gameObject);
    }

    // Public method to get the large bullet's damage value
    public int GetBulletDamage()
    {
        return bulletDamage;
    }

    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }
}
