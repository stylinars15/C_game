using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject prefabGoblin;
    [SerializeField] private GameObject prefabEye;

    public float yCoord; // to test placement 
    public float _spawnInterval = 5f; // Time interval between enemy spawns.
    private float _nextSpawnTime;
    private GameObject _spawnedEnemy;
    
    // List to keep track of spawned enemies
    private List<GameObject> _spawnedEnemies = new List<GameObject>();

    private void Update()
    {
        // Check if playerController is not null before using it
        if (playerController != null)
        {
            // Check if it's time to spawn a new enemy.
            if (Time.time >= _nextSpawnTime)
            {
                SpawnEnemy();
                CalculateNextSpawnTime();
            }
        }
    }

    private void SpawnEnemy()
    {
        float randomXSpawn;
        float randomSide;
        Vector3 spawnPosition;
        Vector3 playerPosition = playerController.playerTransform.position;
        int randomSpawnTicket = Random.Range(0, 2); // Generates a random number between depending on how many cases

        switch (randomSpawnTicket)
        {
            case 0:
                // Spawn Goblin
                // Calculate the new X spawn position within the desired range
                
                randomSide = Random.Range(-1f, 1f);

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
                spawnPosition = new Vector3(randomXSpawn, -1.78f, 0f);
                // Instantiate the enemy prefab at the calculated spawn position
                _spawnedEnemy = Instantiate(prefabGoblin, spawnPosition, Quaternion.identity);
            
                break;
            case 1:
                // Spawn Flying_eye
                // Calculate the new X spawn position within the desired range
                randomSide = Random.Range(-1f, 1f);

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
                spawnPosition = new Vector3(randomXSpawn, -1.7f, 0f);
                // Instantiate the enemy prefab at the calculated spawn position
                _spawnedEnemy = Instantiate(prefabEye, spawnPosition, Quaternion.identity);
                break;
        }
        
        // Add the spawned enemy to the list
        _spawnedEnemies.Add(_spawnedEnemy);
    }


	private void CalculateNextSpawnTime()
    {
        _nextSpawnTime = Time.time + _spawnInterval;
    }
	

	public void DisableSpawnedEnemies()
    {
        foreach (var enemy in _spawnedEnemies)
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
                
                if (enemy.GetComponent<Goblin>() is Goblin goblinScript)
                {
                    goblinScript.enabled = false;
                    Debug.Log("Disabled Goblin script on enemy: " + enemy.name);
                }
                // Check if the enemy is a FlyingEye
                else if (enemy.GetComponent<FlyingEye>() is FlyingEye eyeScript)
                {
                    CapsuleCollider2D capsuleCollider = enemy.GetComponent<CapsuleCollider2D>();
                    if (capsuleCollider != null)
                    {
                        capsuleCollider.enabled = false;
                    }
                    eyeScript.enabled = false;
                    Debug.Log("Disabled FlyingEye script on enemy: " + enemy.name);
                }
            }
        }

        // Disable the EnemySpawner script itself
        enabled = false;
    }
}

