using UnityEngine;

public class EnemyAntiAirShipScript : MonoBehaviour
{
    public int health = 50;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float fireTimer = 0f;

    public float moveSpeed = 2f;
    private float currentSpeed;

    public float detectionRange = 7f; // Detection range for friendly units
    private GameObject targetFriendly = null;
    public float stopDuration = 1f;   // Duration to stop when a target is detected
    private float stopTimer = 0f;     // Timer to control stop duration

    void Start()
    {
        currentSpeed = moveSpeed;     // Initialize current speed
    }

    

    void Update()
    {
        DetectFriendly();

        if (targetFriendly != null)
        {
            // Gradually slow down to stop over the defined stop duration
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
            // Reset stop timer and speed if no friendly unit is detected
            stopTimer = 0f;
            currentSpeed = moveSpeed;
        }

        // Move the enemy ship to the left with the current speed
        Move(currentSpeed);

        if (health <= 0)
        {
            Destroy(gameObject); // Destroy the ship if health reaches zero
        }
    }

    void Move(float speed)
    {
        // Always move to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime);
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

    void DetectFriendly()
    {
        if (targetFriendly != null && targetFriendly == null)
        {
            targetFriendly = null;
            return;
        }

        // Find the nearest friendly unit within range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Friendly"))
            {
                targetFriendly = hit.gameObject;
                break;
            }
        }
    }
     void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Bullet"))
    {
        BulletScript bullet = collision.GetComponent<BulletScript>();
        if (bullet != null)
        {
            int bulletDamage = bullet.GetBulletDamage();
            TakeDamage(bulletDamage);
            Debug.Log("Took damage from Bullet with damage: " + bulletDamage);
        }
    }
    else if (collision.CompareTag("Missile"))
    {
        MissileScript missile = collision.GetComponent<MissileScript>();
        if (missile != null)
        {
            int missileDamage = missile.GetMissileDamage();
            TakeDamage(missileDamage);
            Debug.Log("Took damage from Missile with damage: " + missileDamage);
        }
    }
    else if (collision.CompareTag("LargeBullet"))
    {
        LargeBulletScript largeBullet = collision.GetComponent<LargeBulletScript>();
        if (largeBullet != null)
        {
            int largeBulletDamage = largeBullet.GetBulletDamage();
            TakeDamage(largeBulletDamage);
            Debug.Log("Took damage from LargeBullet with damage: " + largeBulletDamage);
        }
    }
    
    // Destroy the projectile after collision
    //Destroy(collision.gameObject);
}


    void OnDrawGizmosSelected()
    {
        // Draw a detection range sphere in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
