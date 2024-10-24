using UnityEngine;
using UnityEngine.UI;  // Required to work with UI components

public class SliderController : MonoBehaviour
{
    public Slider slider;      // Reference to the slider
    public float maxValue = 100f;  // Maximum value of the slider
    public float currentValue = 100f;  // Current value of the slider

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the slider with the maximum value
        if (slider != null)
        {
            slider.maxValue = maxValue;
            slider.value = currentValue;
        }
        else
        {
            Debug.LogError("Slider not assigned. Please assign the slider in the Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // You can add logic here to control the slider value, like reducing it over time
        // Example: Simulating health decreasing over time (this is optional)
        if (currentValue > 0)
        {
            currentValue -= Time.deltaTime * 5f;  // Decrease the slider value over time
            UpdateSliderValue(currentValue);
        }
    }

    // Method to update the slider's value (call this method when you want to change the value)
    public void UpdateSliderValue(float newValue)
    {
        if (slider != null)
        {
            currentValue = Mathf.Clamp(newValue, 0, maxValue); // Ensure value stays within bounds
            slider.value = currentValue;  // Update the slider's displayed value
        }
    }

    // Method to reset the slider to its maximum value
    public void ResetSlider()
    {
        UpdateSliderValue(maxValue);
    }
}
