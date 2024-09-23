using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health = 50;  // Set the initial health of the enemy
    public GameObject deathEffect;  // Reference to a death effect prefab (optional)

    // Update is called once per frame
    void Update()
    {
        // Check if the enemy's health reaches zero
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to take damage when hit by a bullet or other sources
    public void TakeDamage(int damage)
    {
        health -= damage;  // Reduce the health by the damage amount
    }

    // Method to handle the enemy's death
    void Die()
    {
        // Optional: Create a death effect when the enemy dies
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }
}
