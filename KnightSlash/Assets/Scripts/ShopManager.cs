using UnityEngine;
using YG;

public class ShopManager : MonoBehaviour
{
    public int gunPrice = 50;
    public int swordPrice = 30;
    public int bombPrice = 40;
    public int axePrice = 35;

    public GameObject gunButton;
    public GameObject swordButton;
    public GameObject bombButton;
    public GameObject axeButton;

    [SerializeField] private MenuUI menuUI;
    
    void Start()
    {
        CheckPurchasedItems();
    }

    public void BuyGun()
    {
        if (!YG2.saves.hasGun && GameManager.instance.SpendCoins(gunPrice))
        {
            YG2.saves.hasGun = true;
            YG2.SaveProgress();
            gunButton.SetActive(false);
            menuUI.UpdateCoinUI();
            Debug.Log("Gun purchased!");
        }
        else
        {
            Debug.Log("Not enough coins or already purchased Gun.");
        }
    }

    public void BuySword()
    {
        if (!YG2.saves.hasSword && GameManager.instance.SpendCoins(swordPrice))
        {
            YG2.saves.hasSword = true;
            YG2.SaveProgress();
            swordButton.SetActive(false);
            menuUI.UpdateCoinUI();
            Debug.Log("Sword purchased!");
        }
        else
        {
            Debug.Log("Not enough coins or already purchased Sword.");
        }
    }

    public void BuyBomb()
    {
        if (!YG2.saves.hasBomb && GameManager.instance.SpendCoins(bombPrice))
        {
            YG2.saves.hasBomb = true;
            YG2.SaveProgress();
            bombButton.SetActive(false);
            menuUI.UpdateCoinUI();
            Debug.Log("Bomb purchased!");
        }
        else
        {
            Debug.Log("Not enough coins or already purchased Bomb.");
        }
    }

    public void BuyAxe()
    {
        if (!YG2.saves.hasAxe && GameManager.instance.SpendCoins(axePrice))
        {
            YG2.saves.hasAxe = true;
            YG2.SaveProgress();
            axeButton.SetActive(false);
            menuUI.UpdateCoinUI();
            Debug.Log("Axe purchased!");
        }
        else
        {
            Debug.Log("Not enough coins or already purchased Axe.");
        }
    }
    
    public void CheckPurchasedItems()
    {
        if (YG2.saves.hasGun)
            gunButton.SetActive(false);

        if (YG2.saves.hasSword)
            swordButton.SetActive(false);

        if (YG2.saves.hasBomb)
            bombButton.SetActive(false);

        if (YG2.saves.hasAxe)
            axeButton.SetActive(false);
    }
}
