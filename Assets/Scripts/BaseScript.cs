using UnityEngine;

public class BaseScript : MonoBehaviour
{
    // Array to hold different types of unit prefabs
    public GameObject[] unitPrefabs;

    // Spawn point for the units
    public Transform spawnPoint;

    // Time between spawns
    public float spawnInterval = 5.0f;

    // Timer to track time between spawns
    private float spawnTimer;

    // Index to track the selected unit type (default to the first unit)
    private int selectedUnitIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = spawnInterval; // Initialize the spawn timer
    }

    // Update is called once per frame
    void Update()
    {
        // Handle unit selection with user input
        HandleUnitSelection();

        // Count down the timer
        spawnTimer -= Time.deltaTime;

        // When the timer reaches zero, spawn a new unit
        if (spawnTimer <= 0)
        {
            SpawnUnit();
            spawnTimer = spawnInterval; // Reset the timer
        }
    }

    // Function to handle user input for selecting units
    void HandleUnitSelection()
    {
        // Example input using number keys 1, 2, 3, etc. for unit selection
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            selectedUnitIndex = 0; // Select the first unit
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            selectedUnitIndex = 1; // Select the second unit
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            selectedUnitIndex = 2; // Select the third unit
        }
        // Add more conditions if you have more units
    }

    // Function to spawn the selected unit
    void SpawnUnit()
    {
        if (selectedUnitIndex >= 0 && selectedUnitIndex < unitPrefabs.Length)
        {
            // Instantiate the selected unitPrefab at the spawnPoint's position and rotation
            Instantiate(unitPrefabs[selectedUnitIndex], spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Invalid unit index selected.");
        }
    }
}
