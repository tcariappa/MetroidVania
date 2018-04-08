using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSpawner : MonoBehaviour
{

    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private GameObject spawnPrefab;
    [SerializeField]
    private int numberOfSpawns;
    [SerializeField]
    private float timeBetweenSpawn;

    private void Start()
    {
        StartCoroutine(spawnStart());
    }
    IEnumerator spawnStart()
    {
        for (int i = 0; i < numberOfSpawns; i++)
        {
            Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }
}
