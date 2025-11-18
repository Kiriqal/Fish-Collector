using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Anoa
{
    public class RodShopManager : MonoBehaviour
    {
        [Header("Rod Items")]
        [SerializeField] protected List<RodData> listRodData;

        [Header("UI References")]
        [SerializeField] protected Button buttonIronRodBuy;
        [SerializeField] protected Button buttonGoldRodBuy;
        [SerializeField] protected Button buttonDiamondRodBuy;
        [SerializeField] protected Text textIronRodPrice;
        [SerializeField] protected Text textGoldRodPrice;
        [SerializeField] protected Text textDiamondRodPrice;

        protected void Start()
        {
            InitializeRodShop();
        }

        protected void InitializeRodShop()
        {
            // REGISTER DATA KE STATIC LIST
            foreach (RodData rod in listRodData)
            {
                if (!RodData.listAllRodData.Contains(rod))
                {
                    RodData.listAllRodData.Add(rod);
                }
            }

            // LOAD STATE DARI STATIC DATA
            LoadRodStates();
            UpdateRodButtons();
        }

        protected void LoadRodStates()
        {
            foreach (RodData staticRod in RodData.listAllRodData)
            {
                foreach (RodData sceneRod in listRodData)
                {
                    if (sceneRod.rodType == staticRod.rodType)
                    {
                        sceneRod.boolIsOwned = staticRod.boolIsOwned;
                        sceneRod.boolIsEquipped = staticRod.boolIsEquipped;
                    }
                }
            }
        }

        protected void SaveRodStates()
        {
            foreach (RodData sceneRod in listRodData)
            {
                foreach (RodData staticRod in RodData.listAllRodData)
                {
                    if (staticRod.rodType == sceneRod.rodType)
                    {
                        staticRod.boolIsOwned = sceneRod.boolIsOwned;
                        staticRod.boolIsEquipped = sceneRod.boolIsEquipped;
                    }
                }
            }

            foreach (RodData rod in listRodData)
            {
                if (rod.boolIsEquipped)
                {
                    RodData.currentEquippedRod = rod;
                    break;
                }
            }
        }

        protected void UpdateRodButtons()
        {
            // +++ PERBAIKI: INDEX HARUS SAMA DENGAN LIST ORDER +++
            if (buttonIronRodBuy != null) UpdateSingleRodButton(0, buttonIronRodBuy, textIronRodPrice);    // Index 0 = Iron
            if (buttonGoldRodBuy != null) UpdateSingleRodButton(1, buttonGoldRodBuy, textGoldRodPrice);    // Index 1 = Gold
            if (buttonDiamondRodBuy != null) UpdateSingleRodButton(2, buttonDiamondRodBuy, textDiamondRodPrice); // Index 2 = Diamond
        }

        protected void UpdateSingleRodButton(int rodIndex, Button button, Text priceText)
        {
            if (listRodData == null || listRodData.Count <= rodIndex) return;
            if (button == null) return;

            RodData rod = listRodData[rodIndex];
            Text buttonText = button.GetComponentInChildren<Text>();
            if (buttonText == null) return;

            if (rod.boolIsEquipped)
            {
                buttonText.text = "EQUIPPED";
                button.interactable = false;
                if (priceText != null) priceText.text = "OWNED";
            }
            else if (rod.boolIsOwned)
            {
                buttonText.text = "EQUIP";
                button.interactable = true;
                if (priceText != null) priceText.text = "OWNED";
            }
            else
            {
                buttonText.text = "BUY";
                button.interactable = true;
                if (priceText != null) priceText.text = "Price: " + rod.intPrice;
            }
        }

        public void BuyIronRod() { TryBuyRod(0); }     // Index 0 = Iron
        public void BuyGoldRod() { TryBuyRod(1); }     // Index 1 = Gold
        public void BuyDiamondRod() { TryBuyRod(2); }  // Index 2 = Diamond

        protected void TryBuyRod(int rodIndex)
        {
            if (listRodData == null || listRodData.Count <= rodIndex) return;

            RodData rod = listRodData[rodIndex];

            if (!rod.boolIsOwned)
            {
                rod.boolIsOwned = true;
                Debug.Log("Bought: " + rod.strRodName);
            }

            EquipRod(rodIndex);
        }

        protected void EquipRod(int rodIndex)
        {
            if (listRodData == null || listRodData.Count <= rodIndex) return;

            foreach (RodData rod in listRodData)
            {
                rod.boolIsEquipped = false;
            }

            listRodData[rodIndex].boolIsEquipped = true;
            SaveRodStates();
            UpdateRodButtons();
            Debug.Log("Equipped: " + listRodData[rodIndex].strRodName);
        }
    }
}