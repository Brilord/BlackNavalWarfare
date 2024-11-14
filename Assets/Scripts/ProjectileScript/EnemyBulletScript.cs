using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField]
    private int bulletDamage = 1;       // Damage dealt by the enemy bullet

    [SerializeField]
    private float bulletSpeed = 10f;    // Speed at which the enemy bullet travels

    [SerializeField]
    private float maxRange = 20f;       // Maximum range of the enemy bullet

    [SerializeField]
    private AudioClip bulletSpawnSound; // Sound to play when the bullet is spawned

    private Vector2 startPosition;      // The position where the enemy bullet was instantiated

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

        // Check if the bullet hit the gunboat
        GunboatScript gunboat = collision.gameObject.GetComponent<GunboatScript>();
        if (gunboat != null)
        {
            // Apply damage to the gunboat
            gunboat.TakeDamage(bulletDamage);
        }

        // Check if the bullet hit the friendly base
        BaseScript baseScript = collision.gameObject.GetComponent<BaseScript>();
        if (baseScript != null)
        {
            // Apply damage to the base
            baseScript.TakeDamage(bulletDamage);
        }

        // Check if the bullet hit the cruiser
        CruiserScript cruiser = collision.gameObject.GetComponent<CruiserScript>();
        if (cruiser != null)
        {
            // Apply damage to the cruiser
            cruiser.TakeDamage(bulletDamage);
        }

        // Check if the bullet hit the small anti-air ship
        SmallAntiAirShip smallAntiAirShip = collision.gameObject.GetComponent<SmallAntiAirShip>();
        if (smallAntiAirShip != null)
        {
            // Apply damage to the small anti-air ship
            smallAntiAirShip.TakeDamage(bulletDamage);
        }

        // Check if the bullet hit the battleship
        BattleShipScript battleShip = collision.gameObject.GetComponent<BattleShipScript>();
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
}
