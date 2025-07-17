using System.Collections.Generic;
using UnityEngine;

public class CoinPoolManager : MonoBehaviour
{
    public static CoinPoolManager Instance;

    public GameObject coinPrefab;
    public int initialSize = 30;
    public Transform coinParent;
    public AudioSource audioSource;

    private List<GameObject> coinPool = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.SetActive(false);
            if (coinParent != null)
                coin.transform.parent = coinParent;

            coinPool.Add(coin);
        }
    }

    public GameObject GetCoin()
    {
        foreach (GameObject coin in coinPool)
        {
            if (!coin.activeInHierarchy)
                return coin;
        }
        
        GameObject newCoin = Instantiate(coinPrefab);
        newCoin.SetActive(false);
        if (coinParent != null)
            newCoin.transform.parent = coinParent;

        coinPool.Add(newCoin);
        return newCoin;
    }

    public void ReturnCoin(GameObject coin)
    {
        coin.SetActive(false);
        audioSource.Play();
    }
}
