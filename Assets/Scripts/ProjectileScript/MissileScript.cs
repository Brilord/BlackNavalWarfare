using UnityEngine;

public class MissileScript : MonoBehaviour
{
    public float speed = 10f;                    // Speed of the missile
    public float rotationSpeed = 200f;           // Rotation speed for homing behavior
    public float lifetime = 5f;                  // How long before the missile self-destructs
    public int impactDamage = 100;               // Fixed damage dealt on impact
    public float soarTime = 0.2f;                // Time in seconds to soar upwards before homing
    public float stoppingDistance = 0.5f;        // Distance at which the missile stops rotating toward the target
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.transform;
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
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(impactDamage); // Deal fixed impact damage to the enemy
        }

        // Destroy the missile after impact
        Destroy(gameObject);
    }

    public float GetMissileDamage()
    {
        return impactDamage;
    }
}
