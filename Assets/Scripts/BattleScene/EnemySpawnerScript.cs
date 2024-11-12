using UnityEngine;
using System.Collections;
using TMPro;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject[] unitPrefabs;        // Array of unit prefabs
    public float spawnInterval = 5f;
    private float resources;

    private EnemyBaseScript enemyBaseScript; // Reference to the base script

    void Start()
    {
        enemyBaseScript = GetComponent<EnemyBaseScript>();
        if (enemyBaseScript == null)
        {
            Debug.LogError("EnemyBaseScript not found on this GameObject.");
            return;
        }

        resources = enemyBaseScript.resources;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemyBaseScript.baseHP > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            int unitIndex = SelectRandomUnitToSpawn();
            if (unitIndex != -1)
            {
                SpawnSpecificUnit(unitIndex);
            }
        }
    }

    private int SelectRandomUnitToSpawn()
    {
        // Get list of affordable units
        var affordableUnits = new System.Collections.Generic.List<int>();
        for (int i = 0; i < unitPrefabs.Length; i++)
        {
            if (CanAfford(GetUnitCost(unitPrefabs[i])))
            {
                affordableUnits.Add(i);
            }
        }

        if (affordableUnits.Count > 0)
        {
            return affordableUnits[Random.Range(0, affordableUnits.Count)];
        }

        return -1;
    }

    private void SpawnSpecificUnit(int unitIndex)
    {
        if (unitIndex >= 0 && unitIndex < unitPrefabs.Length)
        {
            GameObject unitPrefab = unitPrefabs[unitIndex];
            int unitCost = GetUnitCost(unitPrefab);

            if (CanAfford(unitCost))
            {
                resources -= unitCost;
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = -3.4f;
                Debug.Log($"Spawning {unitPrefab.name} at {spawnPosition}. Cost: {unitCost} resources. Remaining: {resources}");

                Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    private int GetUnitCost(GameObject unitPrefab)
    {
        var unitScript = unitPrefab.GetComponent<EnemyGunboatScript>();
        return unitScript != null ? unitScript.cost : 0;
    }

    private bool CanAfford(int cost)
    {
        return resources >= cost;
    }

    public void SetResources(float newResources)
    {
        resources = newResources;
    }
}
