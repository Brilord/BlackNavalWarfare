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
    public TextMeshProUGUI statusText; // New status text field
    public GameObject destructionPopup;
    public GameObject retryButton;
    public GameObject quitButton;
    public GameObject[] unitPrefabs;
    public GameObject[] spawnablePrefabs;
    public float spawnInterval = 0.2f;
    public float resources = 100f;

    void Start()
    {
        if (healthText == null)
        {
            Debug.LogError("Health Text UI not assigned. Please assign it in the Inspector.");
        }
        if (statusText == null)
        {
            Debug.LogError("Status Text UI not assigned. Please assign it in the Inspector.");
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
            return affordableUnits[Random.Range(0, affordableUnits.Count)];
        }

        return -1;
    }

    private void SpawnSpecificUnit(int unitIndex)
{
    if (unitIndex >= 0 && unitIndex < unitPrefabs.Length)
    {
        GameObject unitPrefab = unitPrefabs[unitIndex];
        int unitCost = GetUnitCost(unitPrefab);

        // Debug statement to check the available resources before spawning
        Debug.Log($"Attempting to spawn {unitPrefab.name}. Cost: {unitCost}, Available resources: {resources}");

        if (CanAfford(unitCost))
        {
            resources -= unitCost; // Deduct resources
            TriggerResourceChanged();

            // Debug statement to confirm resource deduction
            Debug.Log($"Spawned {unitPrefab.name}. Resources after deduction: {resources}");

            Vector3 spawnPosition = transform.position;
            spawnPosition.y = -3.4f;
            UpdateStatusText($"Spawning {unitPrefab.name}. Cost: {unitCost} resources. Remaining: {resources}");

            Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            // Debug statement if resources are insufficient
            Debug.Log($"Insufficient resources to spawn {unitPrefab.name}. Required: {unitCost}, Available: {resources}");
            UpdateStatusText($"Not enough resources to spawn {unitPrefab.name}. Required: {unitCost}, Available: {resources}");
        }
    }
}


    private int GetUnitCost(GameObject unitPrefab)
{
    // Check for each possible script and retrieve the cost
    if (unitPrefab.TryGetComponent(out EnemyGunboatScript gunboatScript))
    {
        return gunboatScript.cost;
    }
    else if (unitPrefab.TryGetComponent(out EnemyAntiAirShipScript antiAirScript))
    {
        return antiAirScript.cost;
    }
    else if (unitPrefab.TryGetComponent(out EnemyBattleShipScript battleShipScript))
    {
        return battleShipScript.cost;
    }
    else if (unitPrefab.TryGetComponent(out EnemyCruiserScript cruiserScript))
    {
        return cruiserScript.cost;
    }
    
    // Return 0 if no matching script is found
    return 0;
}

    private bool CanAfford(int cost)
    {
        return resources >= cost;
    }

    private void TriggerResourceChanged()
    {
        UpdateStatusText("Resources updated: " + resources);
    }

    private IEnumerator GenerateResources()
{
    while (baseHP > 0)
    {
        yield return new WaitForSeconds(1f);
        float adjustedResourceGeneration = resourceGenerationRate * Mathf.Sqrt(resources / 100); // Adjust based on current resources
        resources += adjustedResourceGeneration;
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
            maxHP += 500f;
            baseHP = Mathf.Min(baseHP + 500f, maxHP);

            UpdateHealthText();
            UpdateStatusText($"Base upgraded! Max HP: {maxHP}, Regen Rate: {healthRegenRate}, Resource Rate: {resourceGenerationRate}");
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
        UpdateStatusText($"Took {damageAmount} damage. Current HP: {baseHP}/{maxHP}");

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
            UpdateStatusText($"Health regenerated. Current HP: {baseHP}");
        }
    }

    private void DestroyBase()
    {
        UpdateStatusText("Base destroyed!");

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

    private void UpdateStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        int bulletLayer = LayerMask.NameToLayer("Bullet");
        int missileLayer = LayerMask.NameToLayer("Missile");
        int largeBulletLayer = LayerMask.NameToLayer("LargeBullet");

        if (collision.gameObject.layer == bulletLayer)
        {
            BulletScript bullet = collision.GetComponent<BulletScript>();
            if (bullet != null)
            {
                int bulletDamage = bullet.GetBulletDamage();
                TakeDamage(bulletDamage);
                UpdateStatusText($"Took damage from Bullet with damage: {bulletDamage}");
            }
        }
        else if (collision.gameObject.layer == missileLayer)
        {
            MissileScript missile = collision.GetComponent<MissileScript>();
            if (missile != null)
            {
                int missileDamage = missile.GetMissileDamage();
                TakeDamage(missileDamage);
                UpdateStatusText($"Took damage from Missile with damage: {missileDamage}");
            }
        }
        else if (collision.gameObject.layer == largeBulletLayer)
        {
            LargeBulletScript largeBullet = collision.GetComponent<LargeBulletScript>();
            if (largeBullet != null)
            {
                int largeBulletDamage = largeBullet.GetBulletDamage();
                TakeDamage(largeBulletDamage);
                UpdateStatusText($"Took damage from Large Bullet with damage: {largeBulletDamage}");
            }
        }
    }

    private IEnumerator SpawnPrefabsAtInterval()
{
    while (baseHP > 0)
    {
        float healthPercentage = baseHP / maxHP;
        float adjustedSpawnInterval = Mathf.Lerp(0.3f, spawnInterval, Mathf.Pow(healthPercentage, 1.5f));

        yield return new WaitForSeconds(adjustedSpawnInterval);

        if (spawnablePrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnablePrefabs.Length);
            SpawnPrefab(spawnablePrefabs[randomIndex]);
        }
    }
}


    private void SpawnPrefab(GameObject prefab)
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y = -3.4f;

        Instantiate(prefab, spawnPosition, Quaternion.identity);
        UpdateStatusText($"Spawned {prefab.name}");
    }
}
