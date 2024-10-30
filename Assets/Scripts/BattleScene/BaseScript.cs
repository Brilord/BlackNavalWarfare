using UnityEngine;
using TMPro;
using System.Collections;

public class BaseScript : MonoBehaviour
{
    // Array to hold different types of unit prefabs
    public GameObject[] unitPrefabs;

    // Default health of the base
    public int baseHealth = 16000;

    // Resource quantity and capacity
    public int resources = 100;  // Starting resource quantity
    public int maxResourceCapacity = 800;  // Max storage capacity for resources
    public int resourceGenerationRate = 20;  // Resources generated per second
    public TextMeshProUGUI resourceText;  // Text component for resource display

    private void Start()
    {
        UpdateResourceText();  // Initialize the resource display at the start
        StartCoroutine(GenerateResources());  // Start resource generation coroutine
    }

    // Coroutine to generate resources
    private IEnumerator GenerateResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);  // Wait for 1 second

            if (resources < maxResourceCapacity)
            {
                resources += resourceGenerationRate;

                // Ensure we do not exceed max capacity
                if (resources > maxResourceCapacity)
                {
                    resources = maxResourceCapacity;
                }

                Debug.Log("Added " + resourceGenerationRate + " resources. Current resources: " + resources);
                UpdateResourceText();
            }
            else
            {
                Debug.Log("Resource capacity reached. Generation paused.");
            }
        }
    }

    

   // Function to check if there are enough resources for an upgrade or purchase
public bool CanAfford(int cost)
{
    return resources >= cost;
}

// Updated function to apply resource storage upgrade with cost
public void UpgradeResourceStorage(int additionalCapacity, int upgradeCost)
{
    if (resources >= upgradeCost)
    {
        resources -= upgradeCost;  // Deduct resources
        maxResourceCapacity += additionalCapacity;
        Debug.Log("Resource storage upgraded! New capacity: " + maxResourceCapacity);
        UpdateResourceText();
    }
    else
    {
        Debug.LogWarning("Not enough resources to upgrade storage.");
    }
}

// Updated function to apply resource generation rate upgrade with cost
public void UpgradeResourceGeneration(int additionalRate, int upgradeCost)
{
    if (resources >= upgradeCost)
    {
        resources -= upgradeCost;  // Deduct resources
        resourceGenerationRate += additionalRate;
        Debug.Log("Resource generation rate upgraded! New rate: " + resourceGenerationRate + " per second.");
        UpdateResourceText();
    }
    else
    {
        Debug.LogWarning("Not enough resources to upgrade generation rate.");
    }
}

    // Function to spawn a specific unit when the button is pressed
    public void SpawnSpecificUnit(int unitIndex)
    {
        if (unitIndex >= 0 && unitIndex < unitPrefabs.Length)
        {
            // Set default cost for units
            int unitCost = 10;  // Default cost for units
            
            // Set specific costs based on unit type
            if (unitIndex == 0)  // Light gunboat
            {
                unitCost = 60;
            }
            else if (unitIndex == 1)  // Small Anti-Air Ship
            {
                unitCost = 80;
            }

            // Check if there are enough resources to spawn the selected unit
            if (resources >= unitCost)
            {
                resources -= unitCost;  // Deduct resources based on unit cost
                UpdateResourceText();  // Update resource display

                // Get the base position (the object's position)
                Vector3 spawnPosition = transform.position;

                // Adjust spawn position based on unit type
                if (unitIndex == 0)  // Light gunboat
                {
                    spawnPosition.y = -3.4f;
                    Debug.Log("Light Gunboat spawned. Cost: " + unitCost + " resources. Remaining: " + resources);
                }
                else if (unitIndex == 1)  // Small Anti-Air Ship
                {
                    spawnPosition.y = -3.4f;
                    Debug.Log("Small Anti-Air Ship spawned. Cost: " + unitCost + " resources. Remaining: " + resources);
                }

                // Instantiate the selected unitPrefab at the modified spawn position and default rotation
                Instantiate(unitPrefabs[unitIndex], spawnPosition, transform.rotation);
            }
            else
            {
                Debug.LogWarning("Not enough resources to spawn this unit. Required: " + unitCost + ", Available: " + resources);
            }
        }
        else
        {
            Debug.LogWarning("Invalid unit index selected.");
        }
    }

    // Function to add resources with capacity check
    public void AddResources(int amount)
    {
        if (resources + amount <= maxResourceCapacity)
        {
            resources += amount;
            UpdateResourceText();
            Debug.Log(amount + " resources added. Total: " + resources);
        }
        else
        {
            resources = maxResourceCapacity;
            UpdateResourceText();
            Debug.Log("Resources reached maximum capacity of " + maxResourceCapacity);
        }
    }

    // Function to deal damage to the base
    public void TakeDamage(int damage)
    {
        baseHealth -= damage;
        if (baseHealth <= 0)
        {
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

    // Function to update resource text
    private void UpdateResourceText()
    {
        if (resourceText != null)
        {
            resourceText.text = "Resources: " + resources + "/" + maxResourceCapacity;
        }
    }
}
