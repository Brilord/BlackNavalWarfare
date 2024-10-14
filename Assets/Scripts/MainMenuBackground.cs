using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using UnityEngine.UI; // Required for UI components

public class MainMenuBackground : MonoBehaviour
{
    public AudioClip backgroundMusic; // Reference to the music file
    private AudioSource audioSource;  // Reference to the AudioSource component

    // UI buttons
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        // Create an AudioSource component dynamically and attach it to the same GameObject
        audioSource = gameObject.AddComponent<AudioSource>();

        // Assign the background music to the AudioSource
        audioSource.clip = backgroundMusic;

        // Set the audio to loop
        audioSource.loop = true;

        // Start playing the music
        audioSource.Play();

        // Set up button listeners
        playButton.onClick.AddListener(OnPlayButtonClicked);
        optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    // Navigate to the stage selector
    public void OnPlayButtonClicked()
{
    SceneManager.LoadScene("StageSelector"); // Replace with your actual stage selector scene name
}

public void OnOptionsButtonClicked()
{
    SceneManager.LoadScene("Options"); // Replace with your actual options scene name
}

public void OnQuitButtonClicked()
{
    SaveGameData(); // Call your custom save logic here (if needed)
    Application.Quit();
}


    // Example save function (you can customize this)
    private void SaveGameData()
    {
        // Implement your saving logic here
        Debug.Log("Game data saved.");
    }
}
