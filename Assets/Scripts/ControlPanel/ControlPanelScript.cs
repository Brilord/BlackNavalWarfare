using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlPanelScript : MonoBehaviour
{
    // Reference to the LightGunboatButton
    public Button lightGunboatButton;

    // Reference to the SmallAntiAirShipButton
    public Button smallAntiAirShipButton;

    // Reference to the QuitButton
    public Button quitButton;

    // Reference to the ResourceStorageUpgradeButton
    public Button resourceStorageUpgradeButton;

    // Reference to the ResourceGenerationIncreaseButton
    public Button resourceGenerationIncreaseButton;

    // Reference to the BaseScript (attach your BaseScript component here)
    public BaseScript baseScript;

    // Start is called before the first frame update
    void Start()
    {
        // Assign listeners to the buttons
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
    }

    // Update is called once per frame to detect key presses
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnLightGunboatButtonClick();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnSmallAntiAirShipButtonClick();
        }
        
        // Additional keyboard shortcuts (optional)
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnResourceStorageUpgradeButtonClick();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnResourceGenerationIncreaseButtonClick();
        }
    }

    void OnLightGunboatButtonClick()
    {
        Debug.Log("Light Gunboat triggered");

        if (baseScript != null)
        {
            baseScript.SpawnSpecificUnit(0); // Assuming 0 is the index for the light gunboat
        }
    }

    void OnSmallAntiAirShipButtonClick()
    {
        Debug.Log("Small Anti-Air Ship triggered");

        if (baseScript != null)
        {
            baseScript.SpawnSpecificUnit(1); // Assuming 1 is the index for the small anti-air ship
        }
    }

void OnResourceStorageUpgradeButtonClick()
{
    Debug.Log("Resource Storage Upgrade triggered");
    int upgradeCost = 50;  // Define the cost for upgrading storage
    if (baseScript != null && baseScript.CanAfford(upgradeCost))
    {
        baseScript.UpgradeResourceStorage(200, upgradeCost); // Upgrade by 200 units with a cost
    }
    else
    {
        Debug.LogWarning("Not enough resources for storage upgrade.");
    }
}

void OnResourceGenerationIncreaseButtonClick()
{
    Debug.Log("Resource Generation Increase triggered");
    int upgradeCost = 30;  // Define the cost for upgrading generation rate
    if (baseScript != null && baseScript.CanAfford(upgradeCost))
    {
        baseScript.UpgradeResourceGeneration(5, upgradeCost); // Increase by 5 units per second with a cost
    }
    else
    {
        Debug.LogWarning("Not enough resources for generation rate upgrade.");
    }
}



    // This method is called when QuitButton is clicked
    void OnQuitButtonClick()
    {
        Debug.Log("Quit button clicked");
        SceneManager.LoadScene("StageSelector"); // Replace with your actual scene name
    }
}
