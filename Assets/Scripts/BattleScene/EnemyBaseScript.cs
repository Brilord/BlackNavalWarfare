using UnityEngine;
using UnityEngine.UI;  // Required to work with UI elements

public class EnemyBaseScript : MonoBehaviour
{
    // Base HP
    public float baseHP = 20000f;
    public float maxHP = 20000f; // Store max HP for reference

    // Reference to the BoxCollider2D component
    private BoxCollider2D baseCollider;

    // Reference to the UI Text element to display health
    public Text healthText;

    // Start is called before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Try to get the BoxCollider2D component attached to the enemy base
        baseCollider = GetComponent<BoxCollider2D>();

        // If the collider is not found, add one dynamically
        if (baseCollider == null)
        {
            Debug.Log("Collider2D not found, adding BoxCollider2D to the enemy base.");
            baseCollider = gameObject.AddComponent<BoxCollider2D>();
            baseCollider.isTrigger = true;  // Ensure it is set as a trigger
        }

        // Ensure the healthText is assigned in the Inspector
        if (healthText == null)
        {
            Debug.LogError("Health Text UI not assigned. Please assign it in the Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (baseHP <= 0)
        {
            DestroyBase();
        }
    }

    // Method to apply damage to the base
    public void ApplyDamage(float damageAmount)
    {
        baseHP -= damageAmount;
        Debug.Log("Damage Applied: " + damageAmount + ". Current Base HP: " + baseHP + "/" + maxHP);

        // Update the health text
        UpdateHealthText();

        // If health is zero or below, destroy the base
        if (baseHP <= 0)
        {
            DestroyBase();
        }
    }

    // Handle the destruction of the base
    private void DestroyBase()
    {
        Debug.Log("Base destroyed! Final HP: " + baseHP + "/" + maxHP);
        Destroy(gameObject);  // Destroy the base GameObject
    }

    // Detect trigger events with the bullet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is tagged as "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Try to get the BulletScript component from the colliding object
            BulletScript bullet = collision.GetComponent<BulletScript>();
            if (bullet != null)
            {
                // Apply damage to the enemy base from the bullet
                ApplyDamage(bullet.GetBulletDamage());

                // Destroy the bullet after applying damage
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.LogWarning("BulletScript component not found on the colliding object.");
            }
        }
    }

    // Update the health text on the screen
    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            // Display health as "Current HP / Max HP"
            healthText.text = baseHP.ToString("F0") + "/" + maxHP.ToString("F0");
            Debug.Log("Health Text Updated: " + healthText.text);
        }
        else
        {
            Debug.LogWarning("Health Text UI is missing!");
        }
    }
}
