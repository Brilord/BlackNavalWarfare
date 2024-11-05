using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for UI elements like Button

public class Popup1Script : MonoBehaviour
{
    // Public variables to assign your buttons in the Inspector
    public Button retryButton;
    public Button quitButton;

    void Start()
    {
        // Ensure the buttons are assigned before adding listeners
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(Retry);
        }
        else
        {
            Debug.LogWarning("Retry Button is not assigned in the Inspector.");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(Quit);
        }
        else
        {
            Debug.LogWarning("Quit Button is not assigned in the Inspector.");
        }
    }

    public void Retry()
    {
        // Reload the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        // Load the Stage Selector scene by its name
        SceneManager.LoadScene("StageSelector"); // Ensure "StageSelector" is added to Build Settings
    }
}
