using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private int bulletDamage = 10;      // Damage dealt by the bullet

    [SerializeField]
    private float bulletSpeed = 10f;    // Speed at which the bullet travels

    void Update()
    {
        // Move the bullet forward based on its speed and direction
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
    }

    // This method is called when the bullet collides with another object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet hit something that can take damage (e.g., an enemy)
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            // Apply damage to the enemy
            enemy.TakeDamage(bulletDamage);
        }

        // Destroy the bullet instance after it hits something
        Destroy(gameObject);
    }

    // Public method to get the bullet's damage value
    public int GetBulletDamage()
    {
        return bulletDamage;
    }
}
