using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControlPanelScript : MonoBehaviour
{
    public Button lightGunboatButton;
    public Button smallAntiAirShipButton;
    public Button quitButton;
    public Button resourceStorageUpgradeButton;
    public Button resourceGenerationIncreaseButton;
    public BaseScript baseScript;
    public BaseScript enemyBaseScript;
    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI baseHealthText;
    public TextMeshProUGUI enemyBaseHealthText;

    void Start()
    {
        if (lightGunboatButton != null)
        {
            lightGunboatButton.onClick.AddListener(OnLightGunboatButtonClick);
        }

        if (smallAntiAirShipButton != null)
        {
            smallAntiAirShipButton.onClick.AddListener(OnSmallAntiAirShipButtonClick);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        if (resourceStorageUpgradeButton != null)
        {
            resourceStorageUpgradeButton.onClick.AddListener(OnResourceStorageUpgradeButtonClick);
        }

        if (resourceGenerationIncreaseButton != null)
        {
            resourceGenerationIncreaseButton.onClick.AddListener(OnResourceGenerationIncreaseButtonClick);
        }

        // Subscribe to the resource and health changed events
        if (baseScript != null)
        {
            baseScript.OnResourceChanged += UpdateResourceText;
            baseScript.OnHealthChanged += UpdateBaseHealthText;
        }

        if (enemyBaseScript != null)
        {
            enemyBaseScript.OnHealthChanged += UpdateEnemyBaseHealthText;
        }

        // Initialize the resource and health text displays
        UpdateResourceText();
        UpdateBaseHealthText();
        UpdateEnemyBaseHealthText();
    }

    void Update()
    {
        // Check for key presses for unit spawning
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Press "1" key
        {
            OnLightGunboatButtonClick();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Press "2" key
        {
            OnSmallAntiAirShipButtonClick();
        }
    }

    // Ensure to unsubscribe from the event when the object is destroyed
    void OnDestroy()
    {
        if (baseScript != null)
        {
            baseScript.OnResourceChanged -= UpdateResourceText;
            baseScript.OnHealthChanged -= UpdateBaseHealthText;
        }

        if (enemyBaseScript != null)
        {
            enemyBaseScript.OnHealthChanged -= UpdateEnemyBaseHealthText;
        }
    }

    public void UpdateResourceText()
    {
        if (resourceText != null && baseScript != null)
        {
            resourceText.text = "Resource: " + baseScript.resources + " / " + baseScript.maxResourceCapacity;
        }
    }

    public void UpdateBaseHealthText()
    {
        if (baseHealthText != null && baseScript != null)
        {
            baseHealthText.text = "Base Health: " + baseScript.baseHealth + " / " + baseScript.maxHealth;
        }
    }

    public void UpdateEnemyBaseHealthText()
    {
        if (enemyBaseHealthText != null && enemyBaseScript != null)
        {
            enemyBaseHealthText.text = "Enemy Base Health: " + enemyBaseScript.baseHealth + " / " + enemyBaseScript.maxHealth;
        }
    }

    void OnLightGunboatButtonClick()
    {
        if (baseScript != null)
        {
            baseScript.SpawnSpecificUnit(0);
        }
    }

    void OnSmallAntiAirShipButtonClick()
    {
        if (baseScript != null)
        {
            baseScript.SpawnSpecificUnit(1);
        }
    }

    void OnResourceStorageUpgradeButtonClick()
    {
        int upgradeCost = 50;
        if (baseScript != null && baseScript.CanAfford(upgradeCost))
        {
            baseScript.UpgradeResourceStorage(200, upgradeCost);
        }
    }

    void OnResourceGenerationIncreaseButtonClick()
    {
        int upgradeCost = 30;
        if (baseScript != null && baseScript.CanAfford(upgradeCost))
        {
            baseScript.UpgradeResourceGeneration(5, upgradeCost);
        }
    }

    void OnQuitButtonClick()
    {
        SceneManager.LoadScene("StageSelector");
    }
}
