using UnityEngine;

public class EnemyLargeBulletScript : MonoBehaviour
{
    [SerializeField]
    private int bulletDamage = 10;       // Increased damage dealt by the large enemy bullet

    [SerializeField]
    private float bulletSpeed = 10f;    // Speed at which the large enemy bullet travels

    [SerializeField]
    private float maxRange = 20f;       // Maximum range of the large enemy bullet

    [SerializeField]
    private AudioClip spawnSound;       // Sound effect for when the bullet is spawned

    private AudioSource audioSource;    // AudioSource component for playing the sound
    private Vector2 startPosition;      // The position where the large enemy bullet was instantiated

    void Start()
    {
        // Store the initial position of the bullet
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
        // Check if the bullet hit something that can take damage
        PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            player.TakeDamage(bulletDamage);
        }

        // Apply damage to other friendly units
        GunboatScript gunboat = collision.gameObject.GetComponent<GunboatScript>();
        if (gunboat != null)
        {
            gunboat.TakeDamage(bulletDamage);
        }

        BaseScript baseScript = collision.gameObject.GetComponent<BaseScript>();
        if (baseScript != null)
        {
            baseScript.TakeDamage(bulletDamage);
        }

        CruiserScript cruiser = collision.gameObject.GetComponent<CruiserScript>();
        if (cruiser != null)
        {
            cruiser.TakeDamage(bulletDamage);
        }

        SmallAntiAirShip smallAntiAirShip = collision.gameObject.GetComponent<SmallAntiAirShip>();
        if (smallAntiAirShip != null)
        {
            smallAntiAirShip.TakeDamage(bulletDamage);
        }

        BattleShipScript battleShip = collision.gameObject.GetComponent<BattleShipScript>();
        if (battleShip != null)
        {
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
