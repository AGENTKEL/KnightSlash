using UnityEngine;
using YG;

public class WeaponModel : MonoBehaviour
{
    public GameObject crossModel;
    public GameObject gunModel;
    public GameObject swordModel;
    public GameObject axeModel;

    void Start()
    {
        UpdateWeaponModel();
    }

    public void UpdateWeaponModel()
    {
        // Отключаем все модели
        crossModel.SetActive(false);
        gunModel.SetActive(false);
        swordModel.SetActive(false);
        axeModel.SetActive(false);

        // Включаем активное оружие из сохранений
        if (YG2.saves.equipedGun)
        {
            gunModel.SetActive(true);
        }
        else if (YG2.saves.equipedSword)
        {
            swordModel.SetActive(true);
        }
        else if (YG2.saves.equipedAxe)
        {
            axeModel.SetActive(true);
        }
        else if (!YG2.saves.equipedAxe && !YG2.saves.equipedSword && !YG2.saves.equipedGun)
        {
            crossModel.SetActive(true);
        }
    }
}
