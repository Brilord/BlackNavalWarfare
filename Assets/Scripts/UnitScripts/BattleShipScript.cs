using UnityEngine;

public class BattleShipScript : MonoBehaviour
{
    public int health = 1000;
    public GameObject gunBulletPrefab;
    public GameObject missilePrefab;
    public Transform gun1FirePoint;
    public Transform gun2FirePoint;
    public Transform missileFirePoint;

    public float gunFireRate = 0.5f;
    public float missileFireRate = 5f;
    public float gunBulletSpeed = 15f;
    public float missileSpeed = 8f;
    private float gunFireTimer = 0f;
    private float missileFireTimer = 0f;

    public float moveSpeed = 1f;
    private float currentSpeed;
    private bool movingRight = true;
    private Vector3 originalScale;
    public float detectionRange = 5f;
    private GameObject targetEnemy = null;
    public float stopDuration = 1f;
    private float stopTimer = 0f;

    void Start()
    {
        originalScale = transform.localScale;
        currentSpeed = moveSpeed;
    }
    

    void Update()
    {
        DetectEnemy();

        if (targetEnemy != null)
        {
            // Gradually reduce speed to zero over stopDuration
            stopTimer += Time.deltaTime;
            currentSpeed = Mathf.Lerp(moveSpeed, 0f, stopTimer / stopDuration);

            // Handle gun shooting
            gunFireTimer += Time.deltaTime;
            if (gunFireTimer >= gunFireRate)
            {
                FireGun(gun1FirePoint);
                FireGun(gun2FirePoint);
                gunFireTimer = 0f;
            }

            // Handle missile shooting
            missileFireTimer += Time.deltaTime;
            if (missileFireTimer >= missileFireRate)
            {
                FireMissile();
                missileFireTimer = 0f;
            }
        }
        else
        {
            // Reset stop timer and speed when no enemy is in range
            stopTimer = 0f;
            currentSpeed = moveSpeed;
        }

        // Move the battleship with the current speed
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

    void FireGun(Transform firePoint)
    {
        if (gunBulletPrefab != null)
        {
            GameObject bullet = Instantiate(gunBulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * gunBulletSpeed;
            }
        }
        else
        {
            Debug.LogWarning("Gun bullet prefab is not assigned or has been destroyed!");
        }
    }

    void FireMissile()
    {
        if (missilePrefab != null)
        {
            GameObject missile = Instantiate(missilePrefab, missileFirePoint.position, missileFirePoint.rotation);
            Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = missileFirePoint.right * missileSpeed;
            }
        }
        else
        {
            Debug.LogWarning("Missile prefab is not assigned or has been destroyed!");
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
