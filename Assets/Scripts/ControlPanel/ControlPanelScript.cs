using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControlPanelScript : MonoBehaviour
{
    public Button lightGunboatButton;
    public Button smallAntiAirShipButton;
    public Button battleshipButton; // New button for battleship
    public Button cruiserButton; // New button for cruiser
    public Button quitButton;
    public Button resourceStorageUpgradeButton;
    public Button resourceGenerationIncreaseButton;
    public BaseScript baseScript;
    public BaseScript enemyBaseScript;
    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI baseHealthText;
    public TextMeshProUGUI enemyBaseHealthText;
    public Button healthRegenUpgradeButton; // New button for health regeneration
    public Button healthCapacityUpgradeButton; // New button for health capacity
    // Initial upgrade costs
    private int resourceStorageUpgradeCost = 50;
    private int resourceGenerationUpgradeCost = 30;

     // Initial upgrade costs
    private int healthCapacityUpgradeCost = 60; // Initial cost for health capacity upgrade
    private int healthRegenUpgradeCost = 40;    // Initial cost for health regeneration upgrade

    // Multiplier to increase upgrade costs each time
    private float upgradeCostMultiplier = 1.2f;

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

        if (battleshipButton != null)
        {
            battleshipButton.onClick.AddListener(OnBattleshipButtonClick);
        }

        if (cruiserButton != null)
        {
            cruiserButton.onClick.AddListener(OnCruiserButtonClick);
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

        if (healthRegenUpgradeButton != null)
        {
            healthRegenUpgradeButton.onClick.AddListener(OnHealthRegenUpgradeButtonClick);
        }

        if (healthCapacityUpgradeButton != null)
        {
            healthCapacityUpgradeButton.onClick.AddListener(OnHealthCapacityUpgradeButtonClick);
        }

        if (baseScript != null)
        {
            baseScript.OnResourceChanged += UpdateResourceText;
            baseScript.OnHealthChanged += UpdateBaseHealthText;
        }

        if (enemyBaseScript != null)
        {
            enemyBaseScript.OnHealthChanged += UpdateEnemyBaseHealthText;
        }

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

        if (Input.GetKeyDown(KeyCode.Alpha3)) // Press "3" key for battleship
        {
            OnBattleshipButtonClick();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) // Press "4" key for cruiser
        {
            OnCruiserButtonClick();
        }

        // Key bindings for upgrades
        if (Input.GetKeyDown(KeyCode.K)) // Press "K" key for resource generation increase
        {
            OnResourceGenerationIncreaseButtonClick();
        }

        if (Input.GetKeyDown(KeyCode.L)) // Press "L" key for resource storage upgrade
        {
            OnResourceStorageUpgradeButtonClick();
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

    void OnBattleshipButtonClick()
    {
        if (baseScript != null)
        {
            baseScript.SpawnSpecificUnit(2); // Assuming 3 is the index for battleship
        }
    }

    void OnCruiserButtonClick()
    {
        if (baseScript != null)
        {
            baseScript.SpawnSpecificUnit(3); // Assuming 4 is the index for cruiser
        }
    }

    void OnResourceStorageUpgradeButtonClick()
    {
        if (baseScript != null && baseScript.CanAfford(resourceStorageUpgradeCost))
        {
            baseScript.UpgradeResourceStorage(200, resourceStorageUpgradeCost);
            
            // Increase the cost for the next upgrade
            resourceStorageUpgradeCost = Mathf.CeilToInt(resourceStorageUpgradeCost * upgradeCostMultiplier);
        }
    }

    void OnResourceGenerationIncreaseButtonClick()
    {
        if (baseScript != null && baseScript.CanAfford(resourceGenerationUpgradeCost))
        {
            baseScript.UpgradeResourceGeneration(5, resourceGenerationUpgradeCost);
            
            // Increase the cost for the next upgrade
            resourceGenerationUpgradeCost = Mathf.CeilToInt(resourceGenerationUpgradeCost * upgradeCostMultiplier);
        }
    }
    void OnHealthRegenUpgradeButtonClick()
    {
        if (baseScript != null && baseScript.CanAfford(healthRegenUpgradeCost))
        {
            baseScript.UpgradeHealthRegen(2, healthRegenUpgradeCost);
            healthRegenUpgradeCost = Mathf.CeilToInt(healthRegenUpgradeCost * upgradeCostMultiplier);
        }
    }

    void OnHealthCapacityUpgradeButtonClick()
    {
        if (baseScript != null && baseScript.CanAfford(healthCapacityUpgradeCost))
        {
            baseScript.UpgradeHealthCapacity(50, healthCapacityUpgradeCost);
            healthCapacityUpgradeCost = Mathf.CeilToInt(healthCapacityUpgradeCost * upgradeCostMultiplier);
        }
    }

    void OnQuitButtonClick()
    {
        SceneManager.LoadScene("StageSelector");
    }
}
