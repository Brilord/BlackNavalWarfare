using UnityEngine;

public class EnemyCruiserScript : MonoBehaviour
{
    public int health = 100;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float fireTimer = 0f;

    public float moveSpeed = 1.5f;
    private float currentSpeed;

    public float detectionRange = 7f;
    public float missileRange = 5f;
    private GameObject targetPlayer = null;
    public float stopDuration = 1.5f;
    private float stopTimer = 0f;

    public float missileFireInterval = 5f;
    private float missileFireTimer = 0f;

    void Start()
    {
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        DetectPlayer();

        if (targetPlayer != null)
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
            float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.transform.position);
            if (missileFireTimer >= missileFireInterval && distanceToPlayer >= missileRange)
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
        // Always move left
        transform.Translate(Vector2.left * speed * Time.deltaTime);
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
                    rb.linearVelocity = firePoint.right * bulletScript.GetBulletSpeed();
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
                rb.linearVelocity = firePoint.right * 10f;
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

    void DetectPlayer()
    {
        if (targetPlayer == null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Friendly"))
                {
                    targetPlayer = hit.gameObject;
                    break;
                }
            }
        }
    }

    
//     void OnTriggerEnter2D(Collider2D collision)
// {
//     if (collision.CompareTag("Bullet"))
//     {
//         BulletScript bullet = collision.GetComponent<BulletScript>();
//         if (bullet != null)
//         {
//             int bulletDamage = bullet.GetBulletDamage();
//             TakeDamage(bulletDamage);
//             Debug.Log("Took damage from Bullet with damage: " + bulletDamage);
//         }
//     }
//     else if (collision.CompareTag("Missile"))
//     {
//         MissileScript missile = collision.GetComponent<MissileScript>();
//         if (missile != null)
//         {
//             int missileDamage = missile.GetMissileDamage();
//             TakeDamage(missileDamage);
//             Debug.Log("Took damage from Missile with damage: " + missileDamage);
//         }
//     }
//     else if (collision.CompareTag("LargeBullet"))
//     {
//         LargeBulletScript largeBullet = collision.GetComponent<LargeBulletScript>();
//         if (largeBullet != null)
//         {
//             int largeBulletDamage = largeBullet.GetBulletDamage();
//             TakeDamage(largeBulletDamage);
//             Debug.Log("Took damage from LargeBullet with damage: " + largeBulletDamage);
//         }
//     }
    
//     // Destroy the projectile after collision
//     //Destroy(collision.gameObject);
// }
void OnTriggerEnter2D(Collider2D collision)
{
    int bulletLayer = LayerMask.NameToLayer("Bullet");
    int missileLayer = LayerMask.NameToLayer("Missile");
    int largeBulletLayer = LayerMask.NameToLayer("LargeBullet");

    if (collision.gameObject.layer == bulletLayer)
    {
        BulletScript bullet = collision.GetComponent<BulletScript>();
        if (bullet != null)
        {
            int bulletDamage = bullet.GetBulletDamage();
            TakeDamage(bulletDamage);
            Debug.Log("Took damage from Bullet with damage: " + bulletDamage);
        }
    }
    else if (collision.gameObject.layer == missileLayer)
    {
        MissileScript missile = collision.GetComponent<MissileScript>();
        if (missile != null)
        {
            int missileDamage = missile.GetMissileDamage();
            TakeDamage(missileDamage);
            Debug.Log("Took damage from Missile with damage: " + missileDamage);
        }
    }
    else if (collision.gameObject.layer == largeBulletLayer)
    {
        LargeBulletScript largeBullet = collision.GetComponent<LargeBulletScript>();
        if (largeBullet != null)
        {
            int largeBulletDamage = largeBullet.GetBulletDamage();
            TakeDamage(largeBulletDamage);
            Debug.Log("Took damage from LargeBullet with damage: " + largeBulletDamage);
        }
    }
}


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
