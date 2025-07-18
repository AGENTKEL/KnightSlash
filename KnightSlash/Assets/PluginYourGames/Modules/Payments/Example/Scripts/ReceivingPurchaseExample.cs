using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace YG.Example
{
    public class ReceivingPurchaseExample : MonoBehaviour
    {
        public GameObject purchaseSuccess;
        public GameObject purchaseFail;
        public MenuUI menuUI;

        // Пример Unity событий, на которые можно подписать,
        // например, открытие уведомление о успешности совершения покупки
        public UnityEvent successPurchased;
        public UnityEvent failedPurchased;

        private void OnEnable()
        {
            YG2.onPurchaseSuccess += SuccessPurchased;
            YG2.onPurchaseFailed += FailedPurchased;
        }

        private void OnDisable()
        {
            YG2.onPurchaseSuccess -= SuccessPurchased;
            YG2.onPurchaseFailed -= FailedPurchased;
        }

        private void SuccessPurchased(string id)
        {
            successPurchased?.Invoke();

            purchaseSuccess.SetActive(true);

            // Ваш код для обработки покупки. Например:

            if (id == "coin500")
                YG2.saves.coins += 500;
            else if (id == "coin1000")
                YG2.saves.coins += 1000;
            else if (id == "coin5000")
                YG2.saves.coins += 5000;
            else if (id == "coin10000")
                YG2.saves.coins += 10000;
            else if (id == "coin50000")
                YG2.saves.coins += 50000;
            else if (id == "coin100000")
                YG2.saves.coins += 100000;
            else if (id == "coin500000")
                YG2.saves.coins += 500000;
            else if (id == "coin1000000")
                YG2.saves.coins += 1000000;
            YG2.SaveProgress();
            menuUI.UpdateCoinUI();
        }

        private void FailedPurchased(string id)
        {
            failedPurchased?.Invoke();

            purchaseFail.SetActive(true);
        }
    }
}