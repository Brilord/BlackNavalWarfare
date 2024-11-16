using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Include SceneManagement for scene loading
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
    public GameObject gameOverPanel;
    public int healthRegenRate = 5; // Amount of health to regenerate per second
    public TextMeshProUGUI resourceText;
    public GameObject retryButton;
    public GameObject quitButton;

    public event Action OnResourceChanged;
    public event Action OnHealthChanged;

    private void Start()
    {
        UpdateResourceText();
        StartCoroutine(GenerateResources());
        StartCoroutine(RegenerateHealth());

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Hide the panel initially
        }
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     int damage = 0;

    //     if (collision.CompareTag("EnemyBullet"))
    //     {
    //         damage = collision.GetComponent<EnemyBulletScript>().GetBulletDamage();
    //     }
    //     else if (collision.CompareTag("EnemyMissile"))
    //     {
    //         damage = collision.GetComponent<EnemyMissileScript>().GetMissileDamage();
    //     }
    //     else if (collision.CompareTag("EnemyLargeBullet"))
    //     {
    //         damage = collision.GetComponent<EnemyLargeBulletScript>().GetBulletDamage();
    //     }

    //     if (damage > 0)
    //     {
    //         TakeDamage(damage);
    //         Destroy(collision.gameObject);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D collision)
{
    // Define layer indices for different enemy projectiles
    int enemyBulletLayer = LayerMask.NameToLayer("EnemyBullet");
    int enemyMissileLayer = LayerMask.NameToLayer("EnemyMissile");
    int enemyLargeBulletLayer = LayerMask.NameToLayer("EnemyLargeBullet");

    int damage = 0;

    // Check for each layer and apply damage accordingly
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

    // Apply damage if valid
    if (damage > 0)
    {
        TakeDamage(damage);
        Destroy(collision.gameObject);
        Debug.Log("Took damage from enemy projectile with damage: " + damage);
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
            TriggerResourceChanged();
        }
    }

    public void UpgradeHealthCapacity(int additionalCapacity, int upgradeCost)
    {
        if (CanAfford(upgradeCost))
        {
            resources -= upgradeCost;
            maxHealth += additionalCapacity;
            baseHealth = Mathf.Min(baseHealth, maxHealth); // Ensure current health doesn't exceed the new max
            TriggerResourceChanged();
            TriggerHealthChanged();
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
            TriggerHealthChanged(); // Trigger health update when damaged
        }
    }

    private void BaseDestroyed()
    {
        Debug.Log("Base destroyed!");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Reveal the hidden panel
        }
        Destroy(gameObject);
    }

    private void TriggerResourceChanged()
    {
        OnResourceChanged?.Invoke();
        UpdateResourceText();
    }

    private void TriggerHealthChanged()
    {
        OnHealthChanged?.Invoke();
    }

    // Methods for Retry and Quit buttons
    public void RetryGame()
    {
        SceneManager.LoadScene("GameScene"); // Replace "GameScene" with your actual game scene name
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your actual main menu scene name
    }
}
