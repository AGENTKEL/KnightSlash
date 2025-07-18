using UnityEngine;
using UnityEngine.UI;
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
    
    public Button gunButtonEquip;
    public Button swordButtonEquip;
    public Button bombButtonEquip;
    public Button axeButtonEquip;

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
            gunButtonEquip.interactable = true;
            EquipOnly("Gun");
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
            swordButtonEquip.interactable = true;
            EquipOnly("Sword");
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
            bombButtonEquip.interactable = true;
            EquipOnly("Bomb");
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
            axeButtonEquip.interactable = true;
            EquipOnly("Axe");
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
        {
            gunButton.SetActive(false);
            gunButtonEquip.interactable = true;
        }

        if (YG2.saves.hasSword)
        {
            swordButton.SetActive(false);
            swordButtonEquip.interactable = true;
        }

        if (YG2.saves.hasBomb)
        {
            bombButton.SetActive(false);
            bombButtonEquip.interactable = true;
        }

        if (YG2.saves.hasAxe)
        {
            axeButton.SetActive(false);
            axeButtonEquip.interactable = true;
        }
    }
    
    public void EquipOnly(string itemName)
    {
        YG2.saves.equipedGun = itemName == "Gun";
        YG2.saves.equipedSword = itemName == "Sword";
        YG2.saves.equipedBomb = itemName == "Bomb";
        YG2.saves.equipedAxe = itemName == "Axe";
        YG2.SaveProgress();
    }

    public void MyRewardAdvShow(string id)
    {
        YG2.RewardedAdvShow(id);
    }
    
    private void OnReward(string id)
    {
        if (id == "coin100")
        {
            YG2.saves.coins += 200;
            YG2.SaveProgress();
            menuUI.UpdateCoinUI();
        }
    }
    
    private void OnEnable()
    {
        YG2.onRewardAdv += OnReward;
        menuUI.UpdateCoinUI();
    }
    
    private void OnDisable()
    {
        YG2.onRewardAdv -= OnReward;
    }
}
