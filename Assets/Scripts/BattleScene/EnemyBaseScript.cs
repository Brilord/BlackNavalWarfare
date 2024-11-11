using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyBaseScript : MonoBehaviour
{
    public float baseHP = 20000f;
    public float maxHP = 20000f;
    public float healthRegenRate = 5f;
    public float regenInterval = 1f;
    public float resourceGenerationRate = 10f; // New field for resource generation rate
    public float upgradeInterval = 30f; // Interval for upgrading base stats

    private BoxCollider2D baseCollider;
    public TextMeshProUGUI healthText;
    public GameObject destructionPopup;
    public GameObject retryButton;
    public GameObject quitButton;
    public GameObject[] unitPrefabs;
    public int[] unitCosts;
    public float spawnInterval = 5f;
    public float resources = 100f;

    void Start()
    {
        baseCollider = GetComponent<BoxCollider2D>();
        if (baseCollider == null)
        {
            Debug.Log("Collider2D not found, adding BoxCollider2D to the enemy base.");
            baseCollider = gameObject.AddComponent<BoxCollider2D>();
            baseCollider.isTrigger = true;
        }

        if (healthText == null)
        {
            Debug.LogError("Health Text UI not assigned. Please assign it in the Inspector.");
        }

        if (destructionPopup == null)
        {
            Debug.LogError("Destruction Popup UI not assigned. Please assign it in the Inspector.");
        }
        else
        {
            destructionPopup.SetActive(false);
        }

        if (retryButton != null)
        {
            retryButton.GetComponent<Button>().onClick.AddListener(RestartGame);
        }

        if (quitButton != null)
        {
            quitButton.GetComponent<Button>().onClick.AddListener(QuitToMainMenu);
        }

        StartCoroutine(RegenerateHealth());
        StartCoroutine(SpawnEnemies());
        StartCoroutine(GenerateResources()); // Start resource generation
        StartCoroutine(UpgradeOverTime());   // Start upgrades over time
    }

    void Update()
    {
        if (baseHP <= 0)
        {
            DestroyBase();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("BattleField");
    }

    private IEnumerator SpawnEnemies()
    {
        while (baseHP > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            int unitIndex = SelectRandomUnitToSpawn();
            if (unitIndex != -1)
            {
                SpawnSpecificUnit(unitIndex);
            }
        }
    }

    private int SelectRandomUnitToSpawn()
    {
        // Get list of affordable units
        System.Collections.Generic.List<int> affordableUnits = new System.Collections.Generic.List<int>();
        for (int i = 0; i < unitPrefabs.Length; i++)
        {
            if (CanAfford(unitCosts[i]))
            {
                affordableUnits.Add(i);
            }
        }

        if (affordableUnits.Count > 0)
        {
            // Select a random affordable unit
            return affordableUnits[Random.Range(0, affordableUnits.Count)];
        }
        
        return -1; // No units can be afforded
    }

    private void SpawnSpecificUnit(int unitIndex)
    {
        if (unitIndex >= 0 && unitIndex < unitPrefabs.Length)
        {
            int unitCost = unitCosts[unitIndex];
            if (CanAfford(unitCost))
            {
                resources -= unitCost;
                TriggerResourceChanged();

                Vector3 spawnPosition = transform.position;
                spawnPosition.y = -3.4f;
                Debug.Log($"Spawning {unitPrefabs[unitIndex].name} at {spawnPosition}. Cost: {unitCost} resources. Remaining: {resources}");

                Instantiate(unitPrefabs[unitIndex], spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"Not enough resources to spawn {unitPrefabs[unitIndex].name}. Required: {unitCost}, Available: {resources}");
            }
        }
        else
        {
            Debug.LogWarning("Invalid unit index selected. Check unitPrefabs array.");
        }
    }

    private bool CanAfford(int cost)
    {
        return resources >= cost;
    }

    private void TriggerResourceChanged()
    {
        Debug.Log("Resources updated: " + resources);
    }

    private IEnumerator GenerateResources()
    {
        while (baseHP > 0)
        {
            yield return new WaitForSeconds(1f); // Generate resources every second
            resources += resourceGenerationRate;
            TriggerResourceChanged();
        }
    }

    private IEnumerator UpgradeOverTime()
    {
        while (baseHP > 0)
        {
            yield return new WaitForSeconds(upgradeInterval);

            // Increase health regeneration rate and resource generation rate as upgrades
            healthRegenRate += 2f;          // Increase health regen
            resourceGenerationRate += 5f;    // Increase resource generation

            Debug.Log("Base upgraded! New Health Regen Rate: " + healthRegenRate + ", New Resource Generation Rate: " + resourceGenerationRate);
        }
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("StageSelector");
    }

    public void ApplyDamage(float damageAmount)
    {
        baseHP -= damageAmount;
        UpdateHealthText();

        if (baseHP <= 0)
        {
            DestroyBase();
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (baseHP > 0 && baseHP < maxHP)
        {
            yield return new WaitForSeconds(regenInterval);
            baseHP = Mathf.Min(baseHP + healthRegenRate, maxHP);
            UpdateHealthText();
            Debug.Log("Health regenerated. Current HP: " + baseHP);
        }
    }

    private void DestroyBase()
    {
        Debug.Log("Base destroyed! Final HP: " + baseHP + "/" + maxHP);

        if (destructionPopup != null)
        {
            destructionPopup.SetActive(true);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    // Check if the collided object is tagged as "Bullet"
    if (collision.CompareTag("Bullet"))
    {
        BulletScript bullet = collision.GetComponent<BulletScript>();
        if (bullet != null)
        {
            ApplyDamage(bullet.GetBulletDamage());
            Destroy(collision.gameObject);
            return;
        }
    }

    // Check if the collided object is a missile
    MissileScript missile = collision.GetComponent<MissileScript>();
    if (missile != null)
    {
        Debug.Log("Missile collision with enemy base");
        ApplyDamage(missile.GetMissileDamage());
        Destroy(collision.gameObject);
        return;
    }
}

    

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = baseHP.ToString("F0") + "/" + maxHP.ToString("F0");
        }
    }

    // New method for gunboat to check if this is an enemy
    public bool IsEnemy()
    {
        return true; // This object is an enemy base
    }
}
