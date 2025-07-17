using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolManager : MonoBehaviour
{
    public static ProjectilePoolManager Instance;

    public GameObject projectilePrefab;
    public GameObject projectilePrefabGun;
    public GameObject bombProjectilePrefab;
    public int initialPoolSize = 25;

    private List<GameObject> normalPool = new List<GameObject>();
    private List<GameObject> gunPool = new List<GameObject>();
    private List<GameObject> bombPool = new List<GameObject>();

    public Transform pooledParent;

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            if (pooledParent != null)
                obj.transform.parent = pooledParent;
            normalPool.Add(obj);
        }

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefabGun);
            obj.SetActive(false);
            if (pooledParent != null)
                obj.transform.parent = pooledParent;
            gunPool.Add(obj);
        }
    }

    public GameObject GetNormalProjectile()
    {
        foreach (GameObject obj in normalPool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        GameObject newObj = Instantiate(projectilePrefab);
        newObj.SetActive(false);
        if (pooledParent != null)
            newObj.transform.parent = pooledParent;
        normalPool.Add(newObj);
        return newObj;
    }

    public GameObject GetGunProjectile()
    {
        foreach (GameObject obj in gunPool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        GameObject newObj = Instantiate(projectilePrefabGun);
        newObj.SetActive(false);
        if (pooledParent != null)
            newObj.transform.parent = pooledParent;
        gunPool.Add(newObj);
        return newObj;
    }
    
    public GameObject GetBombProjectile()
    {
        foreach (GameObject obj in bombPool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        GameObject newObj = Instantiate(bombProjectilePrefab);
        newObj.SetActive(false);
        if (pooledParent != null)
            newObj.transform.parent = pooledParent;
        bombPool.Add(newObj);
        return newObj;
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
    }
}
