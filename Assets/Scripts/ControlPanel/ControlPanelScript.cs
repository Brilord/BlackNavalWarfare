using UnityEngine;
using UnityEngine.UI;

public class ControlPanelScript : MonoBehaviour
{
    // Reference to the LightGunboatButton
    public Button lightGunboatButton;

    // Reference to the SmallAntiAirShipButton
    public Button smallAntiAirShipButton;

    // Reference to the BaseScript
    public BaseScript baseScript; // Attach your BaseScript component here

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
    }

    // This method is called when LightGunboatButton is clicked
    void OnLightGunboatButtonClick()
    {
        Debug.Log("Light Gunboat Button was clicked");

        // Trigger the spawn of the light gunboat through the BaseScript
        if (baseScript != null)
        {
            baseScript.SpawnSpecificUnit(0); // Assuming 0 is the index for the light gunboat
        }
    }

    // This method is called when SmallAntiAirShipButton is clicked
    void OnSmallAntiAirShipButtonClick()
    {
        Debug.Log("Small Anti-Air Ship Button was clicked");

        // Trigger the spawn of the small anti-air ship through the BaseScript
        if (baseScript != null)
        {
            baseScript.SpawnSpecificUnit(1); // Assuming 1 is the index for the small anti-air ship
        }
    }
}
