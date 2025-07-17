using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int coins = 0;
    private int allCoins = 0;
    
    public bool hasGun => YG2.saves.hasGun;
    public bool hasSword => YG2.saves.hasSword;
    public bool hasBomb => YG2.saves.hasBomb;
    public bool hasAxe => YG2.saves.hasAxe;

    public int GetCoinCount() => coins;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount = 1)
    {
        coins += amount;
        Debug.Log("Coins: " + coins);
    }

    public void SaveCoins()
    {
        YG2.saves.coins += GetCoinCount();
        YG2.SaveProgress();
        allCoins = YG2.saves.coins;
        coins = 0;
    }
    
    public bool SpendCoins(int amount)
    {
        if (YG2.saves.coins >= amount)
        {
            YG2.saves.coins -= amount;
            YG2.SaveProgress();
            return true;
        }
        return false;
    }
}
