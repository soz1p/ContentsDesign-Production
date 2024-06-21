using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiCompassSpawn : MonoBehaviour
{
    public GameObject compassPrefab;
    public float spawnRadius = 10;
    void Start()
    {
        SpawnCompass();
    }

    void SpawnCompass()
    {
        if (compassPrefab != null)
        {
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(compassPrefab, randomPosition, Quaternion.identity);
            Debug.Log("Compass spawned at: " + randomPosition);
        }
        else
        {
            Debug.LogError("Compass prefab is not assigned in the CompassSpawner script.");
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position;
        randomDirection.y = 7; // y 값을 평면으로 유지
        return randomDirection;
    }
}
