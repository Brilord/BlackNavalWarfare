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

        // Initialize health text
        UpdateHealthText();
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
        // Check if the collision is with an enemy bullet, missile, or large bullet
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
