using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlPanelScript : MonoBehaviour
{
    // Reference to the LightGunboatButton
    public Button lightGunboatButton;

    // Reference to the SmallAntiAirShipButton
    public Button smallAntiAirShipButton;

    // // Reference to the OptionButton
    // public Button optionButton;

    // Reference to the QuitButton
    public Button quitButton;

    // Reference to the OptionsPanel (another UI panel for settings/options)
    // public GameObject optionsPanel;

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

        // if (optionButton != null)
        // {
        //     optionButton.onClick.AddListener(OnOptionButtonClick);
        // }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        // // Ensure the options panel is hidden initially
        // if (optionsPanel != null)
        // {
        //     optionsPanel.SetActive(false);
        // }
    }

    // Update is called once per frame to detect key presses
    void Update()
    {
        // Check if the '1' key is pressed to spawn Light Gunboat
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnLightGunboatButtonClick();
        }

        // Check if the '2' key is pressed to spawn Small Anti-Air Ship
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnSmallAntiAirShipButtonClick();
        }
    }

    // This method is called when LightGunboatButton is clicked or '1' key is pressed
    void OnLightGunboatButtonClick()
    {
        Debug.Log("Light Gunboat triggered");

        // Trigger the spawn of the light gunboat through the BaseScript
        if (baseScript != null)
        {
            baseScript.SpawnSpecificUnit(0); // Assuming 0 is the index for the light gunboat
        }
    }

    // This method is called when SmallAntiAirShipButton is clicked or '2' key is pressed
    void OnSmallAntiAirShipButtonClick()
    {
        Debug.Log("Small Anti-Air Ship triggered");

        // Trigger the spawn of the small anti-air ship through the BaseScript
        if (baseScript != null)
        {
            baseScript.SpawnSpecificUnit(1); // Assuming 1 is the index for the small anti-air ship
        }
    }

    // This method is called when OptionButton is clicked
    // void OnOptionButtonClick()
    // {
    //     Debug.Log("Option button clicked");

    //     // Toggle the visibility of the options panel
    //     if (optionsPanel != null)
    //     {
    //         optionsPanel.SetActive(!optionsPanel.activeSelf);
    //     }
    // }

    // This method is called when QuitButton is clicked
    void OnQuitButtonClick()
    {
        Debug.Log("Quit button clicked");

        // Navigate to the stage selector scene
        SceneManager.LoadScene("StageSelector"); // Replace with your actual scene name
    }
}
