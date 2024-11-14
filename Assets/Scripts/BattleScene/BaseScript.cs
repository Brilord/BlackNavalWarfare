using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class BaseScript : MonoBehaviour
{
    public GameObject[] unitPrefabs;
    public int baseHealth = 16000;
    public int maxHealth = 16000; // Set max health for display purposes
    public int resources = 100;
    public int maxResourceCapacity = 800;
    public int resourceGenerationRate = 20;

     public int healthRegenRate = 5; // Amount of health to regenerate per second
    public TextMeshProUGUI resourceText;

    // Define events to notify listeners when resources or health change
    public event Action OnResourceChanged;
    public event Action OnHealthChanged;

    // Define events to notify listeners when resources or health change
    

    private void Start()
    {
        UpdateResourceText();
        StartCoroutine(GenerateResources());
        StartCoroutine(RegenerateHealth());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            // Assuming the bullet has a script with a damage property
            int damage = collision.GetComponent<EnemyBulletScript>().GetBulletDamage();

            TakeDamage(damage);

            // Destroy the bullet after collision
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator GenerateResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (resources < maxResourceCapacity)
            {
                resources = Mathf.Min(resources + resourceGenerationRate, maxResourceCapacity);
                Debug.Log("Added " + resourceGenerationRate + " resources. Current resources: " + resources);
                TriggerResourceChanged();
            }
            else
            {
                Debug.Log("Resource capacity reached. Generation paused.");
            }
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (baseHealth < maxHealth)
            {
                baseHealth = Mathf.Min(baseHealth + healthRegenRate, maxHealth);
                Debug.Log("Regenerated " + healthRegenRate + " health. Current health: " + baseHealth);
                TriggerHealthChanged();
            }
        }
    }

    private void UpdateResourceText()
    {
        if (resourceText != null)
        {
            resourceText.text = $"Resources: {resources}/{maxResourceCapacity}";
        }
    }

    public bool CanAfford(int cost)
    {
        return resources >= cost;
    }

    public void UpgradeResourceStorage(int additionalCapacity, int upgradeCost)
    {
        if (CanAfford(upgradeCost))
        {
            resources -= upgradeCost;
            maxResourceCapacity += additionalCapacity;
            Debug.Log("Resource storage upgraded! New capacity: " + maxResourceCapacity);
            TriggerResourceChanged();
        }
        else
        {
            Debug.LogWarning("Not enough resources to upgrade storage.");
        }
    }

    public void UpgradeResourceGeneration(int additionalRate, int upgradeCost)
    {
        if (CanAfford(upgradeCost))
        {
            resources -= upgradeCost;
            resourceGenerationRate += additionalRate;
            Debug.Log("Resource generation rate upgraded! New rate: " + resourceGenerationRate + " per second.");
            TriggerResourceChanged();
        }
        else
        {
            Debug.LogWarning("Not enough resources to upgrade generation rate.");
        }
    }

    public void UpgradeHealthRegen(int additionalRegenRate, int upgradeCost)
    {
        if (CanAfford(upgradeCost))
        {
            resources -= upgradeCost;
            healthRegenRate += additionalRegenRate;
            Debug.Log("Health regeneration rate upgraded! New rate: " + healthRegenRate + " per second.");
            TriggerResourceChanged();
        }
        else
        {
            Debug.LogWarning("Not enough resources to upgrade health regeneration.");
        }
    }

    public void UpgradeHealthCapacity(int additionalCapacity, int upgradeCost)
    {
        if (CanAfford(upgradeCost))
        {
            resources -= upgradeCost;
            maxHealth += additionalCapacity;
            baseHealth = Mathf.Min(baseHealth, maxHealth); // Ensure current health doesn't exceed the new max
            Debug.Log("Health capacity upgraded! New max health: " + maxHealth);
            TriggerResourceChanged();
            TriggerHealthChanged();
        }
        else
        {
            Debug.LogWarning("Not enough resources to upgrade health capacity.");
        }
    }

    public void SpawnSpecificUnit(int unitIndex)
    {
        if (unitIndex >= 0 && unitIndex < unitPrefabs.Length)
        {
            int unitCost = GetUnitCost(unitIndex);

            if (CanAfford(unitCost))
            {
                resources -= unitCost;
                TriggerResourceChanged();

                Vector3 spawnPosition = transform.position;
                spawnPosition.y = -3.4f;
                Debug.Log($"{unitPrefabs[unitIndex].name} spawned. Cost: {unitCost} resources. Remaining: {resources}");

                // Use Quaternion.identity to ensure units spawn with default rotation
                Instantiate(unitPrefabs[unitIndex], spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"Not enough resources to spawn {unitPrefabs[unitIndex].name}. Required: {unitCost}, Available: {resources}");
            }
        }
        else
        {
            Debug.LogWarning("Invalid unit index selected.");
        }
    }

    private int GetUnitCost(int unitIndex)
    {
        return unitIndex switch
        {
            0 => 60,
            1 => 80,
            2 => 400,
            3 => 100,
            _ => 0,
        };
    }

    public void AddResources(int amount)
    {
        resources = Mathf.Min(resources + amount, maxResourceCapacity);
        TriggerResourceChanged();
        Debug.Log($"{amount} resources added. Total: {resources}");
    }

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
            TriggerHealthChanged(); // Trigger health update when damaged
        }
    }

    private void BaseDestroyed()
    {
        Debug.Log("Base destroyed!");
    }
    

    private void TriggerResourceChanged()
    {
        // Invoke the event to notify listeners of the resource change
        OnResourceChanged?.Invoke();
        UpdateResourceText();
    }

    private void TriggerHealthChanged()
    {
        // Invoke the event to notify listeners of the health change
        OnHealthChanged?.Invoke();
    }
}
