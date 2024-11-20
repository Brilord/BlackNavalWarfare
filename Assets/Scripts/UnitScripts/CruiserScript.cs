using UnityEngine;

public class CruiserScript : MonoBehaviour
{
    public int health = 100;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;           // Missile prefab for shooting missiles
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float fireTimer = 0f;

    public float moveSpeed = 1.5f;
    private float currentSpeed;
    private bool movingRight = true;

    public float detectionRange = 7f;
    public float missileRange = 5f;            // Minimum distance to fire missiles
    private GameObject targetEnemy = null;
    public float stopDuration = 1.5f;
    private float stopTimer = 0f;

    public float missileFireInterval = 5f;     // Interval for firing missiles
    private float missileFireTimer = 0f;

    void Start()
    {
        currentSpeed = moveSpeed;
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

            missileFireTimer += Time.deltaTime;
            float distanceToEnemy = Vector2.Distance(transform.position, targetEnemy.transform.position);
            if (missileFireTimer >= missileFireInterval && distanceToEnemy >= missileRange)
            {
                FireMissile();
                missileFireTimer = 0f;
            }
        }
        else
        {
            stopTimer = 0f;
            currentSpeed = moveSpeed;
            missileFireTimer = 0f;
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
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();

            if (bulletScript != null)
            {
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = firePoint.right * bulletScript.GetBulletSpeed();  // Get bullet speed from BulletScript
                }
            }
        }
        else
        {
            Debug.LogWarning("Bullet prefab is not assigned or has been destroyed!");
        }
    }

    void FireMissile()
    {
        if (missilePrefab != null)
        {
            GameObject missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * 10f;  // Assign a separate speed for missiles
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

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void DetectEnemy()
    {
        if (targetEnemy != null && targetEnemy == null)
        {
            targetEnemy = null;
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


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
