using UnityEngine;

public class EnemyMissileScript : MonoBehaviour
{
    public float speed = 3f;                    // Speed of the missile
    public float rotationSpeed = 400f;           // Rotation speed for homing behavior
    public float lifetime = 5f;                  // How long before the missile self-destructs
    public int impactDamage = 20;                // Fixed damage dealt on impact
    public float soarTime = 0.2f;                // Time in seconds to soar upwards before homing
    public float stoppingDistance = 0.5f;        // Distance at which the missile stops rotating toward the target

    [SerializeField]
    private AudioClip missileSpawnSound;         // Sound to play when the missile is spawned

    private float lifetimeTimer;
    private float soarTimer;
    private Transform target;                    // Variable to store the closest target
    private bool isHoming;                       // Flag to check if missile is homing towards target

    void Start()
    {
        lifetimeTimer = lifetime;
        soarTimer = soarTime;
        isHoming = false;                        // Start with soaring behavior
        FaceUpwards();                           // Ensure the missile initially faces upwards

        // Play the missile spawn sound
        if (missileSpawnSound != null)
        {
            AudioSource.PlayClipAtPoint(missileSpawnSound, transform.position);
        }
    }

    void Update()
    {
        if (soarTimer > 0)
        {
            SoarUpwards();                       // Missile soars up at the beginning
            soarTimer -= Time.deltaTime;
        }
        else if (!isHoming)
        {
            // Lock onto the closest target only once after soaring
            isHoming = true;
            FindClosestTarget();
        }
        
        if (isHoming && target != null)
        {
            MoveMissile();
        }
        
        UpdateLifetime();
    }

    void FaceUpwards()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90); // 90 degrees to face upwards
    }

    void SoarUpwards()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void FindClosestTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Friendly"); // Targets player objects
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestPlayer = player;
            }
        }

        if (closestPlayer != null)
        {
            target = closestPlayer.transform;
        }
    }

    void MoveMissile()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            
            // Smoothly rotate the missile towards the target
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            // Move missile forward in the direction it is facing
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    void UpdateLifetime()
    {
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0)
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
            player.TakeDamage(impactDamage);
        }

        // Check if the bullet hit the gunboat
        GunboatScript gunboat = collision.gameObject.GetComponent<GunboatScript>();
        if (gunboat != null)
        {
            // Apply damage to the gunboat
            gunboat.TakeDamage(impactDamage);
        }

        // Check if the bullet hit the friendly base
        BaseScript baseScript = collision.gameObject.GetComponent<BaseScript>();
        if (baseScript != null)
        {
            // Apply damage to the base
            baseScript.TakeDamage(impactDamage);
        }

        // Check if the bullet hit the cruiser
        CruiserScript cruiser = collision.gameObject.GetComponent<CruiserScript>();
        if (cruiser != null)
        {
            // Apply damage to the cruiser
            cruiser.TakeDamage(impactDamage);
        }

        // Check if the bullet hit the small anti-air ship
        SmallAntiAirShip smallAntiAirShip = collision.gameObject.GetComponent<SmallAntiAirShip>();
        if (smallAntiAirShip != null)
        {
            // Apply damage to the small anti-air ship
            smallAntiAirShip.TakeDamage(impactDamage);
        }

        // Check if the bullet hit the battleship
        BattleShipScript battleShip = collision.gameObject.GetComponent<BattleShipScript>();
        if (battleShip != null)
        {
            // Apply damage to the battleship
            battleShip.TakeDamage(impactDamage);
        }

        // Destroy the bullet instance after it hits something
        Destroy(gameObject);
    }

    public int GetMissileDamage()
    {
        return impactDamage;
    }
    
}
