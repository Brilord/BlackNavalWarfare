using UnityEngine;

public class SmallAntiAirShip : MonoBehaviour
{
    public int health = 40;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    private float fireTimer = 0f;

    public float moveSpeed = 2f;
    private float currentSpeed;
    private bool movingRight = true;

    public float detectionRange = 5f; // Range within which to detect enemies
    private GameObject targetEnemy = null;
    public float stopDuration = 1f;   // Duration to come to a stop
    private float stopTimer = 0f;     // Timer to track stopping duration

    void Start()
    {
        currentSpeed = moveSpeed;     // Initialize current speed to moveSpeed
    }

    void Update()
    {
        DetectEnemy();

        if (targetEnemy != null)
        {
            // Gradually reduce speed to zero over stopDuration (1 second)
            stopTimer += Time.deltaTime;
            currentSpeed = Mathf.Lerp(moveSpeed, 0f, stopTimer / stopDuration);

            // Handle shooting
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
        else
        {
            // Reset stop timer and speed if no enemy is detected
            stopTimer = 0f;
            currentSpeed = moveSpeed;
        }

        // Move the ship with the current speed
        Move(currentSpeed);

        if (health <= 0)
        {
            Destroy(gameObject);  // Destroy the ship if health reaches zero
        }
    }

    void Move(float speed)
    {
        // Move the ship in the current direction
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        // Switch direction after reaching a certain boundary
        if (transform.position.x >= 10f)
        {
            movingRight = false;
        }
        else if (transform.position.x <= -10f)
        {
            movingRight = true;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right; // Set velocity based on direction without speed multiplier
            }
        }
        else
        {
            Debug.LogWarning("Bullet prefab is not assigned or has been destroyed!");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void DetectEnemy()
    {
        if (targetEnemy != null && targetEnemy == null)
        {
            targetEnemy = null;
            return;
        }

        // Find the nearest enemy within range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                targetEnemy = hit.gameObject;
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a sphere in the editor to visualize the detection range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with an enemy bullet, missile, or large bullet
        if (collision.CompareTag("EnemyBullet"))
        {
            int bulletDamage = collision.GetComponent<EnemyBulletScript>().GetBulletDamage();
            TakeDamage(bulletDamage);
            Destroy(collision.gameObject);
        }
        
    }
}
