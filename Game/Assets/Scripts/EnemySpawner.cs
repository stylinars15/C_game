using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject enemyPrefab;

    private float fixedYSpawn = -1.78f; // The fixed Y position for spawning.
    private float spawnInterval = 500f; // Time interval between enemy spawns.
    private float nextSpawnTime = 0f;

    // List to keep track of spawned enemies
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Update()
    {
        // Check if playerController is not null before using it
        if (playerController != null)
        {
            // Check if it's time to spawn a new enemy.
            if (Time.time >= nextSpawnTime)
            {
                SpawnEnemy();
                CalculateNextSpawnTime();
            }
        }
    }

	private void SpawnEnemy()
	{
    	Vector3 playerPosition = playerController.playerTransform.position;

    	// Calculate the new X spawn position within the desired range
    	float randomXSpawn;
		float randomSide = Random.Range(-1f, 1f);

		if (randomSide < 0)
		{
    	// If randomSide is negative, spawn on the left side in the range -12 to -10
    	randomXSpawn = playerPosition.x + Random.Range(-12f, -10f);
		}
		else
		{
    	// If randomSide is non-negative, spawn on the right side in the range 10 to 12
    	randomXSpawn = playerPosition.x + Random.Range(10f, 12f);
		}

    	// Create a spawn position with the calculated X and fixed Y position
    	Vector3 spawnPosition = new Vector3(randomXSpawn, fixedYSpawn, 0f);

    	// Instantiate the enemy prefab at the calculated spawn position
    	GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

    	// Add the spawned enemy to the list
    	spawnedEnemies.Add(spawnedEnemy);
	}


    private void CalculateNextSpawnTime()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    
    public void DisableSpawnedEnemies()
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                // Disable the PolygonCollider2D (if it exists)
                PolygonCollider2D polygonCollider = enemy.GetComponent<PolygonCollider2D>();
                if (polygonCollider != null)
                {
                    polygonCollider.enabled = false;
                }

                // Disable the BoxCollider2D (if it exists)
                BoxCollider2D boxCollider = enemy.GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    boxCollider.enabled = false;
                }
            	Goblin goblinScript = enemy.GetComponent<Goblin>();
            	if (goblinScript != null)
            	{
                	goblinScript.enabled = false; // Disable the script
            	}
            }
        }

        // Disable the EnemySpawner script itself
        enabled = false;
    }
}

