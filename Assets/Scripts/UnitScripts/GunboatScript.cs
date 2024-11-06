using UnityEngine;

public class GunboatScript : MonoBehaviour
{
    public int health = 40;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    private float fireTimer = 0f;
    public float bulletSpeed = 10f;
    public float moveSpeed = 1f;
    private float currentSpeed;      // Current speed for gradual stop
    private bool movingRight = true;
    private Vector3 originalScale;
    public float detectionRange = 5f;
    private GameObject targetEnemy = null;
    public float stopDuration = 1f;  // Duration to come to a stop
    private float stopTimer = 0f;    // Timer to track stopping duration

    void Start()
    {
        originalScale = transform.localScale;
        currentSpeed = moveSpeed;    // Initialize current speed to moveSpeed
    }

    void Update()
    {
        DetectEnemy();

        if (targetEnemy != null)
        {
            // Increment the stop timer to gradually reduce speed
            stopTimer += Time.deltaTime;

            // Gradually reduce speed to zero over stopDuration (1 second)
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
            // Reset the stop timer and speed when no enemy is in range
            stopTimer = 0f;
            currentSpeed = moveSpeed;
        }

        // Move the gunboat with the current speed
        Move(currentSpeed);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Move(float speed)
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (transform.position.x >= 10f && movingRight)
        {
            movingRight = false;
            Flip();
        }
        else if (transform.position.x <= -10f && !movingRight)
        {
            movingRight = true;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 newScale = originalScale;
        newScale.x *= -1;
        transform.localScale = newScale;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            int bulletDamage = collision.GetComponent<EnemyBulletScript>().GetBulletDamage();
            TakeDamage(bulletDamage);
            Destroy(collision.gameObject);
        }
    }

    void DetectEnemy()
    {
        if (targetEnemy != null)
        {
            if (targetEnemy == null)
            {
                targetEnemy = null;
            }
            return;
        }

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
