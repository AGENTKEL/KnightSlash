using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public int waveCount = 0;

    private float timer = 0f;
    
    public static EnemyWaveSpawner Instance;
    
    public PlayerHealth playerHealth;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (playerHealth.isDead) return;
        timer += Time.deltaTime;
        if (timer >= timeBetweenWaves && waveCount < 177)
        {
            SpawnWave();
            timer = 0f;
        }
    }

    void SpawnWave()
    {
        waveCount++;
        Debug.Log($"Wave {waveCount}");

        // Скелеты на всех волнах
        int skeletonCount = Mathf.FloorToInt(5 + waveCount * 1.5f);

        // Гоблины на всех волнах, чуть меньше чем скелеты
        int goblinCount = Mathf.FloorToInt(3 + waveCount * 1.2f);

        // Боссы только на каждой 10-й волне
        int bossCount = (waveCount % 10 == 0) ? waveCount / 10 : 0;

        SpawnEnemies("Skeleton", skeletonCount);
        SpawnEnemies("Goblin", goblinCount);

        if (bossCount > 0)
            SpawnEnemies("Boss", bossCount);
    }

    void SpawnEnemies(string type, int count)
    {
        bool isBoss = type == "Boss";

        for (int i = 0; i < count; i++)
        {
            // Пропускаем проверку лимита для боссов
            if (!isBoss && EnemyPoolManager.Instance.GetTotalActiveEnemies() >= 100)
                break;

            GameObject enemy = EnemyPoolManager.Instance.GetFromPool(type);
            if (enemy != null)
            {
                Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
                enemy.transform.position = point.position;
                enemy.transform.rotation = Quaternion.identity;
                enemy.SetActive(true);
            }
        }
    }
}
