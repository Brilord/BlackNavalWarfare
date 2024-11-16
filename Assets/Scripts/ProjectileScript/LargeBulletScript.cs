using UnityEngine;

public class LargeBulletScript : MonoBehaviour
{
    [SerializeField]
    private int largeBulletDamage = 10;      // Increased damage dealt by the large bullet

    [SerializeField]
    private float bulletSpeed = 10f;         // Speed at which the large bullet travels

    [SerializeField]
    private float maxRange = 20f;            // Maximum range of the large bullet

    [SerializeField]
    private AudioClip spawnSound;            // Sound effect for when the bullet is spawned

    private AudioSource audioSource;         // AudioSource component for playing the sound

    private Vector2 startPosition;           // The position where the large bullet was instantiated

    void Start()
    {
        // Store the initial position of the large bullet
        startPosition = transform.position;

        // Get or add an AudioSource component to play the spawn sound
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the audio clip to the spawn sound and play it
        audioSource.clip = spawnSound;
        audioSource.playOnAwake = false;
        audioSource.Play();
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
        // Check if the large bullet hit an enemy and apply damage
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(largeBulletDamage);
        }

        // Apply damage to other enemy types
        EnemyBaseScript enemyBase = collision.gameObject.GetComponent<EnemyBaseScript>();
        if (enemyBase != null)
        {
            enemyBase.TakeDamage(largeBulletDamage);
        }

        EnemyGunboatScript gunboat = collision.gameObject.GetComponent<EnemyGunboatScript>();
        if (gunboat != null)
        {
            gunboat.TakeDamage(largeBulletDamage);
        }

        EnemyCruiserScript cruiser = collision.gameObject.GetComponent<EnemyCruiserScript>();
        if (cruiser != null)
        {
            cruiser.TakeDamage(largeBulletDamage);
        }

        EnemyAntiAirShipScript antiAirShip = collision.gameObject.GetComponent<EnemyAntiAirShipScript>();
        if (antiAirShip != null)
        {
            antiAirShip.TakeDamage(largeBulletDamage);
        }

        EnemyBattleShipScript battleShip = collision.gameObject.GetComponent<EnemyBattleShipScript>();
        if (battleShip != null)
        {
            battleShip.TakeDamage(largeBulletDamage);
        }

        // Destroy the large bullet instance after it hits something
        Destroy(gameObject);
    }

    // Public method to get the large bullet's damage value
    public int GetBulletDamage()
    {
        return largeBulletDamage;
    }

    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }
}
