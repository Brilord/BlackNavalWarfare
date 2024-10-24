using UnityEngine;

public class BaseScript : MonoBehaviour
{
    // Array to hold different types of unit prefabs
    public GameObject[] unitPrefabs;

    // Default health of the base
    public int baseHealth = 16000;

    // Function to spawn a specific unit when the button is pressed
    public void SpawnSpecificUnit(int unitIndex)
    {
        // Check if the unitIndex is valid
        if (unitIndex >= 0 && unitIndex < unitPrefabs.Length)
        {
            // Get the base position (the object's position)
            Vector3 spawnPosition = transform.position;

            // Check if the selected unit is a ship type and adjust the spawn position accordingly
            if (unitIndex == 0) // Assuming 0 is the index for the light gunboat
            {
                // Set the y-position to -3.4 for the ship
                spawnPosition.y = -3.4f;
                Debug.Log("Light Gunboat spawned.");
            }
            else if (unitIndex == 1) // Assuming 1 is the index for the small anti-air ship
            {
                // Adjust the spawn position for the Small Anti-Air Ship if necessary
                spawnPosition.y = -3.4f; // Example adjustment for the Small Anti-Air Ship
                Debug.Log("Small Anti-Air Ship spawned.");
            }

            // Instantiate the selected unitPrefab at the modified spawn position and default rotation
            Instantiate(unitPrefabs[unitIndex], spawnPosition, transform.rotation);
        }
        else
        {
            Debug.LogWarning("Invalid unit index selected.");
        }
    }

    // Function to deal damage to the base
    public void TakeDamage(int damage)
    {
        baseHealth -= damage;
        if (baseHealth <= 0)
        {
            // Call function when base health reaches zero or below
            BaseDestroyed();
        }
        else
        {
            Debug.Log("Base health: " + baseHealth);
        }
    }

    // Function to handle base destruction
    private void BaseDestroyed()
    {
        Debug.Log("Base destroyed!");
        // Implement logic for base destruction (e.g., game over, play destruction animation, etc.)
    }
}
