using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject missile;
    private Rigidbody bossRb;
    private GameObject player;
    private bool isOnGround = true;
    private float bossJumpForce = 30;
    private float bossSlamStrength = 15;
    // Start is called before the first frame update
    void Start()
    {
        bossRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        isOnGround = true;
        InvokeRepeating("BossMove", 3, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && !isOnGround)
        {
            isOnGround = true;
            Slam();
        }
    }

    private void BossMove()
    {
        int move = Random.Range(0, 3);
        switch (move)
        {
            case 0:
                SpawnEnemy();
                break;
            case 1:
                SpawnMissile();
                break;
            case 3:
                Jump();
                break;
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
    }

    private Vector3 GenerateSpawnPosition()
    {
        Vector3 offset = (player.transform.position - transform.position).normalized * 4f;
        return transform.position + offset;
    }

    private void SpawnMissile()
    {
        GameObject currmissile = Instantiate(missile, GenerateSpawnPosition(), missile.transform.rotation);
        currmissile.GetComponent<Missile>().enemy = player.gameObject;
    }

    private void Jump()
    {
        bossRb.AddForce(Vector3.up*bossJumpForce, ForceMode.Impulse);
        isOnGround = false;
    }

    private void Slam()
    {
        var enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy.gameObject != gameObject)
            {
                enemy.gameObject.GetComponent<Rigidbody>().AddExplosionForce(bossSlamStrength, transform.position, 100f, 0f, ForceMode.Impulse);
            }
        }
        player.GetComponent<Rigidbody>().AddExplosionForce(bossSlamStrength, transform.position, 10f, 0f, ForceMode.Impulse);
    }
}
