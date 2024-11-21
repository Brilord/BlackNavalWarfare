using UnityEngine;
using UnityEngine.UI;

public class GunboatScript : MonoBehaviour
{
    public int health = 40;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    private float fireTimer = 0f;
    public float bulletSpeed = 10f;
    public float moveSpeed = 1f;
    private float currentSpeed;
    private bool movingRight = true;
    private Vector3 originalScale;
    public float detectionRange = 5f;
    private GameObject targetEnemy = null;
    public float stopDuration = 1f;
    private float stopTimer = 0f;

    // Reference to the health text
    public Text healthText;

    void Start()
{
    originalScale = transform.localScale;
    currentSpeed = moveSpeed;


    // Adjust the BoxCollider2D size with slight random fluctuation
    BoxCollider2D collider = GetComponent<BoxCollider2D>();
    if (collider != null)
    {
        // Store the original size
        Vector2 originalSize = collider.size;

        // Add a small random fluctuation to the size
        float randomXAdjustment = Random.Range(-1f, 1f); // ±0.01 range
        float randomYAdjustment = Random.Range(-1f, 1f); // ±0.01 range
        collider.size = new Vector2(
            originalSize.x + randomXAdjustment,
            originalSize.y + randomYAdjustment
        );

        Debug.Log($"Collider size adjusted: X {collider.size.x}, Y {collider.size.y}");
    }
    else
    {
        Debug.LogWarning("No BoxCollider2D attached to the object!");
    }
}


    void Update()
    {
        DetectEnemy();

        if (targetEnemy != null)
        {
            stopTimer += Time.deltaTime;
            currentSpeed = Mathf.Lerp(moveSpeed, 0f, stopTimer / stopDuration);

            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
        else
        {
            stopTimer = 0f;
            currentSpeed = moveSpeed;
        }

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
        UpdateHealthText();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

void OnTriggerEnter2D(Collider2D collision)
{
    // Define layer indices for different enemy projectiles
    int enemyBulletLayer = LayerMask.NameToLayer("EnemyBullet");
    int enemyMissileLayer = LayerMask.NameToLayer("EnemyMissile");
    int enemyLargeBulletLayer = LayerMask.NameToLayer("EnemyLargeBullet");

    int damage = 0;

    // Check if the collision is with an enemy bullet, missile, or large bullet based on layers
    if (collision.gameObject.layer == enemyBulletLayer)
    {
        EnemyBulletScript bullet = collision.GetComponent<EnemyBulletScript>();
        if (bullet != null)
        {
            damage = bullet.GetBulletDamage();
        }
    }
    else if (collision.gameObject.layer == enemyMissileLayer)
    {
        EnemyMissileScript missile = collision.GetComponent<EnemyMissileScript>();
        if (missile != null)
        {
            damage = missile.GetMissileDamage();
        }
    }
    else if (collision.gameObject.layer == enemyLargeBulletLayer)
    {
        EnemyLargeBulletScript largeBullet = collision.GetComponent<EnemyLargeBulletScript>();
        if (largeBullet != null)
        {
            damage = largeBullet.GetBulletDamage();
        }
    }

    // Apply damage if valid and destroy the object
    if (damage > 0)
    {
        TakeDamage(damage);
        Destroy(collision.gameObject);
        Debug.Log("Took damage from enemy projectile with damage: " + damage);
    }
}

    void DetectEnemy()
    {
        if (targetEnemy != null)
        {
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

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = health.ToString();
        }
        else
        {
            Debug.LogWarning("Health Text is not assigned!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
