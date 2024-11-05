using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int health = 100;  // Set the initial health of the player
    public GameObject deathEffect;  // Reference to a death effect prefab (optional)

    // Update is called once per frame
    void Update()
    {
        // Check if the player's health reaches zero
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to take damage when hit by enemy attacks or hazards
    public void TakeDamage(int damage)
    {
        health -= damage;  // Reduce the health by the damage amount
    }

    // Optional method to heal the player
    public void Heal(int healAmount)
    {
        health += healAmount;  // Increase health by the heal amount
    }

    // Method to handle the player's death
    void Die()
    {
        // Optional: Create a death effect when the player dies
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }


        // Destroy the player object or disable player controls
        Destroy(gameObject);
    }
}
