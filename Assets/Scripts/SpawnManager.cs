using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType {Push, Missile, Slam};
public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public GameObject[] powerupPrefabs;
    [SerializeField] private int enemyCount; 
    private float spawnRange = 9f;
    private int waveNumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if (enemyCount == 0)
        {
            waveNumber++;
            if (waveNumber%3 == 0)
            {
                SpawnEnemyWave(waveNumber/3);
                SpawnBoss();
            }
            else
            {
                SpawnEnemyWave(waveNumber);
            }
        }
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        GameObject enemyPrefab;
        for(int i = 0; i<enemiesToSpawn; i++)
        {
            enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
        GameObject powerupPrefab = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
    }
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0.5f, spawnPosZ);

        return randomPos;
    }
    private void SpawnBoss()
    {
        Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
    } 
}
