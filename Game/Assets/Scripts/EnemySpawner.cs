using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject prefabGoblin;
    [SerializeField] private GameObject prefabEye;
    [SerializeField] private GameObject prefabSkeleton;
    
    public float _spawnInterval; // Time interval between enemy spawns.
    private float _nextSpawnTime;
    private GameObject _spawnedEnemy;
    private readonly int maxActiveEnemies = 3; // Set the maximum number of active enemies
    
    // List to keep track of spawned enemies
    private List<GameObject> _spawnedEnemies = new List<GameObject>();

    private void Update()
    {
        if (playerController != null && Time.time >= _nextSpawnTime)
        {
            CalculateNextSpawnTime();
            if (CountActiveEnemies() < maxActiveEnemies)
            {
                SpawnEnemy();
            }
        }
    }
    
    private void SpawnEnemy()
    {
        float randomXSpawn;
        float randomSide;
        Vector3 spawnPosition;
        Vector3 playerPosition = playerController.playerTransform.position;
        int randomSpawnTicket = Random.Range(0, 3); // Generates a random number between depending on how many cases

        switch (randomSpawnTicket)
        {
            case 0:
                // Spawn Goblin
                // in the range 10 to 12
                randomXSpawn = playerPosition.x + Random.Range(10f, 12f);

                // Create a spawn position with the calculated X and fixed Y position
                spawnPosition = new Vector3(randomXSpawn, -1.78f, 0f);
                // Instantiate the enemy prefab at the calculated spawn position
                _spawnedEnemy = Instantiate(prefabGoblin, spawnPosition, Quaternion.identity);
            
                break;
            case 1:
                // Spawn Flying_eye
                // Calculate the new X spawn position within the desired range
                randomSide = Random.Range(-1f, 1f);
                // in the range 10 to 12
                randomXSpawn = playerPosition.x + Random.Range(10f, 12f);

                // Create a spawn position with the calculated X and fixed Y position
                spawnPosition = new Vector3(randomXSpawn, -1.7f, 0f);
                // Instantiate the enemy prefab at the calculated spawn position
                _spawnedEnemy = Instantiate(prefabEye, spawnPosition, Quaternion.identity);
                break;
            case 2:
                // Spawn Skeleton
                // in the range 10 to 12
                randomXSpawn = playerPosition.x + Random.Range(10f, 12f);

                // Create a spawn position with the calculated X and fixed Y position
                spawnPosition = new Vector3(randomXSpawn, -1.93f, 0f);
                // Instantiate the enemy prefab at the calculated spawn position
                _spawnedEnemy = Instantiate(prefabSkeleton, spawnPosition, Quaternion.identity);
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
                }
            }
        }

        // Disable the EnemySpawner script itself
        enabled = false;
    }
    
    private int CountActiveEnemies()
    {
        int activeEnemies = 0;
        foreach (var enemy in _spawnedEnemies)
        {
            if (enemy != null)
            {
                if (enemy.TryGetComponent<Goblin>(out Goblin goblin) && !goblin.isDead)
                {
                    activeEnemies++;
                }
                else if (enemy.TryGetComponent<FlyingEye>(out FlyingEye flyingEye) && !flyingEye.isDead)
                {
                    activeEnemies++;
                }
            }
        }
        return activeEnemies;
    }


}

