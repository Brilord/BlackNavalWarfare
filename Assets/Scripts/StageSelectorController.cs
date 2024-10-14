using NUnit.Framework.Internal.Commands;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectorController : MonoBehaviour
{

    public Button backButton;
    public Button stage1SelectButton;

    // Start is called before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backButton.onClick.AddListener(BackToMainMenu);
        stage1SelectButton.onClick.AddListener(Stage1SelectButton);
    }

    // Update is called once per frame
    void Update()
    {
        // Optional: Handle per-frame updates if needed
    }

    // Navigation function to load a scene by name
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Navigation function to load a scene by index
    public void Stage1SelectButton()
    {
        SceneManager.LoadScene("BattleScene");
    }
    

}
