using UnityEngine;

public class EnemyAntiAirShipScript : MonoBehaviour
{
    public int health = 50;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float fireTimer = 0f;
    public float bulletSpeed = 12f;

    public float moveSpeed = 2f;
    private float currentSpeed;
    private bool movingRight = true;

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

        // Move the enemy ship with the current speed
        Move(currentSpeed);

        if (health <= 0)
        {
            Destroy(gameObject); // Destroy the ship if health reaches zero
        }
    }

    void Move(float speed)
    {
        // Move in the current direction
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
                rb.linearVelocity = firePoint.right * bulletSpeed;
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

    void OnDrawGizmosSelected()
    {
        // Draw a detection range sphere in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
