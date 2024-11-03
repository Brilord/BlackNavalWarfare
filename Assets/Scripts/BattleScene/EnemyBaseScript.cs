using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyBaseScript : MonoBehaviour
{
    // Base HP
    public float baseHP = 20000f;
    public float maxHP = 20000f;
    
    // Health regeneration variables
    public float healthRegenRate = 5f; // Amount of health regenerated per second
    public float regenInterval = 1f; // Time interval (in seconds) for regeneration

    // Reference to the BoxCollider2D component
    private BoxCollider2D baseCollider;

    // Reference to the UI Text element to display health
    public TextMeshProUGUI healthText;

    // Reference to the popup panel that will display the destruction message
    public GameObject destructionPopup;
    public GameObject retryButton;
    public GameObject quitButton;
    public GameObject[] unitPrefabs; // Array of different unit prefabs
public int[] unitCosts; // Array of costs for each unit
public float spawnInterval = 5f; // Time interval between spawns
public float resources = 100f; // Total resources available for spawning units


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
            baseCollider.isTrigger = true;
        }

        // Ensure the healthText is assigned in the Inspector
        if (healthText == null)
        {
            Debug.LogError("Health Text UI not assigned. Please assign it in the Inspector.");
        }

        // Ensure the destructionPopup is assigned in the Inspector
        if (destructionPopup == null)
        {
            Debug.LogError("Destruction Popup UI not assigned. Please assign it in the Inspector.");
        }
        else
        {
            destructionPopup.SetActive(false); // Hide it initially
        }
        if (retryButton != null)
        {
            retryButton.GetComponent<Button>().onClick.AddListener(RestartGame);
        }

        if (quitButton != null)
        {
            quitButton.GetComponent<Button>().onClick.AddListener(QuitToMainMenu);
        }

        // Start the health regeneration coroutine
        StartCoroutine(RegenerateHealth());
        StartCoroutine(SpawnEnemies());

    }

    // Update is called once per frame
    void Update()
    {
        if (baseHP <= 0)
        {
            DestroyBase();
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("BattleField"); // Reloads the current scene
    }
    private IEnumerator SpawnEnemies()
{
    while (baseHP > 0)
    {
        yield return new WaitForSeconds(spawnInterval);

        int unitIndex = SelectUnitToSpawn(); // Select a unit based on AI logic
        SpawnSpecificUnit(unitIndex);
    }
}
private int SelectUnitToSpawn()
{
    // Simple AI logic: Randomly select a unit that the AI can afford
    for (int i = 0; i < unitPrefabs.Length; i++)
    {
        if (CanAfford(unitCosts[i]))
        {
            return i;
        }
    }

    return -1; // Return -1 if no units can be afforded
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
            Debug.Log($"{unitPrefabs[unitIndex].name} spawned. Cost: {unitCost} resources. Remaining: {resources}");

            Instantiate(unitPrefabs[unitIndex], spawnPosition, transform.rotation);
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


private bool CanAfford(int cost)
{
    return resources >= cost;
}

private void TriggerResourceChanged()
{
    Debug.Log("Resources updated: " + resources);
}




    // Method to quit to the main menu (Quit)
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("StageSelector"); // Replace "MainMenu" with the name of your main menu scene
    }

    // Method to apply damage to the base
    public void ApplyDamage(float damageAmount)
    {
        baseHP -= damageAmount;
        UpdateHealthText();

        if (baseHP <= 0)
        {
            DestroyBase();
        }
    }

    // Coroutine to regenerate health over time
    private IEnumerator RegenerateHealth()
    {
        while (baseHP > 0 && baseHP < maxHP)
        {
            yield return new WaitForSeconds(regenInterval);
            baseHP = Mathf.Min(baseHP + healthRegenRate, maxHP); // Regenerate health up to maxHP
            UpdateHealthText();
            Debug.Log("Health regenerated. Current HP: " + baseHP);
        }
    }

    // Handle the destruction of the base
    private void DestroyBase()
    {
        Debug.Log("Base destroyed! Final HP: " + baseHP + "/" + maxHP);
        
        // Show the destruction popup
        if (destructionPopup != null)
        {
            destructionPopup.SetActive(true); // Make the popup visible
        }
        
        Destroy(gameObject); // Destroy the base GameObject
    }

    // Detect trigger events with the bullet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BulletScript bullet = collision.GetComponent<BulletScript>();
        
        if (bullet != null)
        {
            ApplyDamage(bullet.GetBulletDamage());
            Destroy(collision.gameObject);
        }
    }

    // Update the health text on the screen
    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = baseHP.ToString("F0") + "/" + maxHP.ToString("F0");
        }
    }
}
