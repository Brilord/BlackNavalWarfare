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
    public float resourceGenerationRate = 10f;
    public float upgradeInterval = 30f;

    private BoxCollider2D baseCollider;
    public TextMeshProUGUI healthText;
    public GameObject destructionPopup;
    public GameObject retryButton;
    public GameObject quitButton;
    public GameObject[] unitPrefabs; // Array of unit prefabs
    public GameObject[] spawnablePrefabs; // Array of generic spawnable prefabs
    public float spawnInterval = 0.2f;
    public float resources = 100f;

    void Start()
    {
        // Initialize UI elements, start coroutines, etc.
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
        StartCoroutine(GenerateResources());
        StartCoroutine(UpgradeOverTime());
        StartCoroutine(SpawnPrefabsAtInterval());
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
        var affordableUnits = new System.Collections.Generic.List<int>();
        for (int i = 0; i < unitPrefabs.Length; i++)
        {
            if (CanAfford(GetUnitCost(unitPrefabs[i])))
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
            GameObject unitPrefab = unitPrefabs[unitIndex];
            int unitCost = GetUnitCost(unitPrefab);
            
            if (CanAfford(unitCost))
            {
                resources -= unitCost;
                TriggerResourceChanged();

                Vector3 spawnPosition = transform.position;
                spawnPosition.y = -3.4f;
                Debug.Log($"Spawning {unitPrefab.name} at {spawnPosition}. Cost: {unitCost} resources. Remaining: {resources}");

                Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"Not enough resources to spawn {unitPrefab.name}. Required: {unitCost}, Available: {resources}");
            }
        }
        else
        {
            Debug.LogWarning("Invalid unit index selected. Check unitPrefabs array.");
        }
    }

    private int GetUnitCost(GameObject unitPrefab)
    {
        var unitScript = unitPrefab.GetComponent<EnemyGunboatScript>();
        return unitScript != null ? unitScript.cost : 0;
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
            yield return new WaitForSeconds(1f);
            resources += resourceGenerationRate;
            TriggerResourceChanged();
        }
    }

    private IEnumerator UpgradeOverTime()
    {
        while (baseHP > 0)
        {
            yield return new WaitForSeconds(upgradeInterval);

            healthRegenRate += 2f;
            resourceGenerationRate += 5f;

            Debug.Log("Base upgraded! New Health Regen Rate: " + healthRegenRate + ", New Resource Generation Rate: " + resourceGenerationRate);
        }
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("StageSelector");
    }

    public void TakeDamage(float damageAmount)
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

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = baseHP.ToString("F0") + "/" + maxHP.ToString("F0");
        }
    }

    public bool IsEnemy()
    {
        return true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            BulletScript bullet = collision.GetComponent<BulletScript>();
            if (bullet != null)
            {
                int bulletDamage = bullet.GetBulletDamage();
                TakeDamage(bulletDamage);
                Destroy(collision.gameObject);
                Debug.Log("Took damage from Bullet with damage: " + bulletDamage);
            }
        }
        else if (collision.CompareTag("Missile"))
        {
            MissileScript missile = collision.GetComponent<MissileScript>();
            if (missile != null)
            {
                int missileDamage = missile.GetMissileDamage();
                TakeDamage(missileDamage);
                Destroy(collision.gameObject);
                Debug.Log("Took damage from Missile with damage: " + missileDamage);
            }
        }
        else if (collision.CompareTag("LargeBullet"))
        {
            LargeBulletScript largeBullet = collision.GetComponent<LargeBulletScript>();
            if (largeBullet != null)
            {
                int largeBulletDamage = largeBullet.GetBulletDamage();
                TakeDamage(largeBulletDamage);
                Destroy(collision.gameObject);
                Debug.Log("Took damage from LargeBullet with damage: " + largeBulletDamage);
            }
        }
        
        // Destroy the projectile after collision
    }

    // New coroutine to spawn generic prefabs at intervals
    private IEnumerator SpawnPrefabsAtInterval()
    {
        while (baseHP > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            foreach (var prefab in spawnablePrefabs)
            {
                SpawnPrefab(prefab);
            }
        }
    }

    private void SpawnPrefab(GameObject prefab)
{
    Vector3 spawnPosition = transform.position;
    spawnPosition.y = -3.4f; // Fixed Y position at -3.4f

    Instantiate(prefab, spawnPosition, Quaternion.identity);
    Debug.Log($"Spawned {prefab.name} at {spawnPosition}");
}

}
