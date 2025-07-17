using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;

    [System.Serializable]
    public class EnemyPool
    {
        public string type;
        public GameObject prefab;
        public int initialSize;
        public List<GameObject> pool = new List<GameObject>();
    }

    public List<EnemyPool> enemyPools;
    public Transform pooledParent;

    void Awake()
    {
        Instance = this;

        foreach (var enemyPool in enemyPools)
        {
            for (int i = 0; i < enemyPool.initialSize; i++)
            {
                GameObject obj = Instantiate(enemyPool.prefab);
                obj.SetActive(false);
                if (pooledParent != null)
                    obj.transform.parent = pooledParent;
                enemyPool.pool.Add(obj);
            }
        }
    }

    public GameObject GetFromPool(string type)
    {
        EnemyPool pool = enemyPools.Find(p => p.type == type);
        if (pool == null) return null;

        foreach (var enemy in pool.pool)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        GameObject newEnemy = Instantiate(pool.prefab);
        newEnemy.SetActive(false);
        if (pooledParent != null)
            newEnemy.transform.parent = pooledParent;
        pool.pool.Add(newEnemy);
        return newEnemy;
    }
    
    public int GetTotalActiveEnemies()
    {
        int total = 0;
        foreach (var pool in enemyPools)
        {
            foreach (var enemy in pool.pool)
            {
                if (enemy.activeInHierarchy)
                    total++;
            }
        }
        return total;
    }
}
