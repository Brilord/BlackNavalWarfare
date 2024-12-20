using UnityEngine;

public class EnemyBattleShipScript : MonoBehaviour
{
    public int health = 1000;
    public GameObject gunBulletPrefab;
    public GameObject missilePrefab;
    public Transform gun1FirePoint;
    public Transform gun2FirePoint;
    public Transform missileFirePoint;
    public int cost = 400;                     // Cost of the enemy gunboat (if relevant for your game)

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
    private GameObject targetPlayer = null;
    public float stopDuration = 1f;
    private float stopTimer = 0f;

    void Start()
    {
        originalScale = transform.localScale;
        currentSpeed = moveSpeed;
    }
    
    void Update()
    {
        DetectPlayer();

        if (targetPlayer != null)
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
            // Reset stop timer and speed when no player is in range
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
    // Always move to the left
    Vector2 movement = Vector2.left * speed * Time.deltaTime;
    transform.Translate(movement);

    // Update rotation to face the direction of movement
    if (movement.x > 0)
    {
        // Face right
        transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }
    else if (movement.x < 0)
    {
        // Face left
        transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
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



    void DetectPlayer()
    {
        if (targetPlayer != null)
        {
            if (targetPlayer == null)
            {
                targetPlayer = null;
            }
            return;
        }

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
