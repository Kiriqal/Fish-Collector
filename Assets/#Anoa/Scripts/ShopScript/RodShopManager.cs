using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Anoa
{
    public class RodShopManager : MonoBehaviour
    {
        [Header("Rod Items")]
        [SerializeField] protected List<RodData> listRodData;
        [SerializeField] protected Button buttonWoodRodBuy;
        [SerializeField] protected Button buttonIronRodBuy;
        [SerializeField] protected Button buttonGoldRodBuy;

        [Header("UI References")]
        [SerializeField] protected Text textWoodRodPrice;
        [SerializeField] protected Text textIronRodPrice;
        [SerializeField] protected Text textGoldRodPrice;

        protected void Start()
        {
            InitializeRodShop();
        }

        protected void InitializeRodShop()
        {
            // SEMUA ROD AWALNYA BELUM DIMILIKI
            foreach (RodData rod in listRodData)
            {
                rod.boolIsOwned = false;
                rod.boolIsEquipped = false;
            }

            UpdateRodButtons();
        }

        protected void UpdateRodButtons()
        {
            UpdateSingleRodButton(0, buttonWoodRodBuy, textWoodRodPrice);
            UpdateSingleRodButton(1, buttonIronRodBuy, textIronRodPrice);
            UpdateSingleRodButton(2, buttonGoldRodBuy, textGoldRodPrice);
        }

        protected void UpdateSingleRodButton(int rodIndex, Button button, Text priceText)
        {
            RodData rod = listRodData[rodIndex];

            if (rod.boolIsEquipped)
            {
                // SEDANG DIPAKAI
                button.GetComponentInChildren<Text>().text = "EQUIPPED";
                button.interactable = false;
                priceText.text = "OWNED";
            }
            else if (rod.boolIsOwned)
            {
                // SUDAH DIMILIKI TAPI TIDAK DIPAKAI
                button.GetComponentInChildren<Text>().text = "EQUIP";
                button.interactable = true;
                priceText.text = "OWNED";
            }
            else
            {
                // BELUM DIMILIKI
                button.GetComponentInChildren<Text>().text = "BUY";
                button.interactable = true;
                priceText.text = "Price: " + rod.intPrice;
            }
        }

        public void BuyWoodRod()
        {
            TryBuyRod(0);
        }

        public void BuyIronRod()
        {
            TryBuyRod(1);
        }

        public void BuyGoldRod()
        {
            TryBuyRod(2);
        }

        protected void TryBuyRod(int rodIndex)
        {
            RodData rod = listRodData[rodIndex];

            if (!rod.boolIsOwned)
            {
                // BELI ROD
                rod.boolIsOwned = true;
                Debug.Log("Bought: " + rod.strRodName);
            }

            // EQUIP ROD (baik baru dibeli atau sudah dimiliki)
            EquipRod(rodIndex);
            UpdateRodButtons();
        }

        protected void EquipRod(int rodIndex)
        {
            // UNEQUIP SEMUA ROD DULU
            foreach (RodData rod in listRodData)
            {
                rod.boolIsEquipped = false;
            }

            // EQUIP ROD YANG DIPILIH
            listRodData[rodIndex].boolIsEquipped = true;
            Debug.Log("Equipped: " + listRodData[rodIndex].strRodName);
        }
    }
}