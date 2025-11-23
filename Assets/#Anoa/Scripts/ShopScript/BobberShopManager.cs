using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Anoa
{
    public class BobberShopManager : MonoBehaviour
    {
        [Header("Bobber Items")]
        [SerializeField] protected List<BobberData> listBobberData;

        [Header("UI References")]
        [SerializeField] protected Button buttonIronBobberBuy;
        [SerializeField] protected Button buttonGoldBobberBuy;
        [SerializeField] protected Button buttonDiamondBobberBuy;
        [SerializeField] protected Text textIronBobberPrice;
        [SerializeField] protected Text textGoldBobberPrice;
        [SerializeField] protected Text textDiamondBobberPrice;

        [Header("Notification Reference")]
        [SerializeField] protected NotificationManager notificationManager;

        protected void Start()
        {
            InitializeBobberShop();
        }

        protected void InitializeBobberShop()
        {
            // REGISTER DATA KE STATIC LIST
            foreach (BobberData bobber in listBobberData)
            {
                if (!BobberData.listAllBobberData.Contains(bobber))
                {
                    BobberData.listAllBobberData.Add(bobber);
                }
            }

            // SET DEFAULT BOBBER JIKA BELUM ADA
            if (BobberData.currentEquippedBobber == null)
            {
                BobberData defaultBobber = ScriptableObject.CreateInstance<BobberData>();
                defaultBobber.bobberType = BobberType.None;
                defaultBobber.strBobberName = "No Bobber";
                defaultBobber.floatSpeedReduction = 0f;
                defaultBobber.boolIsOwned = true;
                defaultBobber.boolIsEquipped = true;
                BobberData.currentEquippedBobber = defaultBobber;
            }

            UpdateBobberButtons();
        }

        protected void UpdateBobberButtons()
        {
            UpdateSingleBobberButton(0, buttonIronBobberBuy, textIronBobberPrice);
            UpdateSingleBobberButton(1, buttonGoldBobberBuy, textGoldBobberPrice);
            UpdateSingleBobberButton(2, buttonDiamondBobberBuy, textDiamondBobberPrice);
        }

        protected void UpdateSingleBobberButton(int bobberIndex, Button button, Text priceText)
        {
            BobberData bobber = listBobberData[bobberIndex];

            if (bobber.boolIsEquipped)
            {
                button.GetComponentInChildren<Text>().text = "EQUIPPED";
                button.interactable = false;
                if (priceText != null) priceText.text = "OWNED";
            }
            else if (bobber.boolIsOwned)
            {
                button.GetComponentInChildren<Text>().text = "EQUIP";
                button.interactable = true;
                if (priceText != null) priceText.text = "OWNED";
            }
            else
            {
                button.GetComponentInChildren<Text>().text = "BUY";
                button.interactable = true;
                if (priceText != null) priceText.text = "Price: " + bobber.intPrice;
            }
        }

        public void BuyIronBobber() { TryBuyBobber(0); }
        public void BuyGoldBobber() { TryBuyBobber(1); }
        public void BuyDiamondBobber() { TryBuyBobber(2); }

        protected void TryBuyBobber(int bobberIndex)
        {
            BobberData bobber = listBobberData[bobberIndex];
            int playerCoins = CoinManager.GetStaticCoins();

            if (!bobber.boolIsOwned)
            {
                if (playerCoins >= bobber.intPrice)
                {
                    CoinManager.AddCoinsStatic(-bobber.intPrice);
                    bobber.boolIsOwned = true;
                    Debug.Log("Bought: " + bobber.strBobberName + " for " + bobber.intPrice + " coins");
                }
                else
                {
                    // +++ NOTIF TIDAK CUKUP COIN +++
                    notificationManager?.ShowErrorNotification("Not enough coins!");
                    Debug.Log("Not enough coins for: " + bobber.strBobberName);
                    return;
                }
            }

            EquipBobber(bobberIndex);
            UpdateBobberButtons();
        }

        protected void EquipBobber(int bobberIndex)
        {
            // UNEQUIP SEMUA BOBBER DULU
            foreach (BobberData bobber in listBobberData)
            {
                bobber.boolIsEquipped = false;
            }

            // EQUIP BOBBER YANG DIPILIH
            listBobberData[bobberIndex].boolIsEquipped = true;
            BobberData.currentEquippedBobber = listBobberData[bobberIndex]; // +++ PASTIKAN INI ADA +++

            Debug.Log("Equipped: " + listBobberData[bobberIndex].strBobberName +
                     " (Speed Reduction: " + (listBobberData[bobberIndex].floatSpeedReduction * 100) + "%)");
        }
    }
}