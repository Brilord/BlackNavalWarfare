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
