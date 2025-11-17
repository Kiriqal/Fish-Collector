using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Anoa
{
    public class BobberShopManager : MonoBehaviour
    {
        [Header("Bobber Items")]
        [SerializeField] protected List<BobberData> listBobberData;
        [SerializeField] protected Button buttonBasicBobberBuy;
        [SerializeField] protected Button buttonAdvancedBobberBuy;
        [SerializeField] protected Button buttonProBobberBuy;

        [Header("UI References")]
        [SerializeField] protected Text textBasicBobberPrice;
        [SerializeField] protected Text textAdvancedBobberPrice;
        [SerializeField] protected Text textProBobberPrice;

        protected void Start()
        {
            InitializeBobberShop();
        }

        protected void InitializeBobberShop()
        {
            foreach (BobberData bobber in listBobberData)
            {
                bobber.boolIsOwned = false;
                bobber.boolIsEquipped = false;
            }
            UpdateBobberButtons();
        }

        protected void UpdateBobberButtons()
        {
            UpdateSingleBobberButton(0, buttonBasicBobberBuy, textBasicBobberPrice);
            UpdateSingleBobberButton(1, buttonAdvancedBobberBuy, textAdvancedBobberPrice);
            UpdateSingleBobberButton(2, buttonProBobberBuy, textProBobberPrice);
        }

        protected void UpdateSingleBobberButton(int bobberIndex, Button button, Text priceText)
        {
            BobberData bobber = listBobberData[bobberIndex];

            if (bobber.boolIsEquipped)
            {
                button.GetComponentInChildren<Text>().text = "EQUIPPED";
                button.interactable = false;
                priceText.text = "OWNED";
            }
            else if (bobber.boolIsOwned)
            {
                button.GetComponentInChildren<Text>().text = "EQUIP";
                button.interactable = true;
                priceText.text = "OWNED";
            }
            else
            {
                button.GetComponentInChildren<Text>().text = "BUY";
                button.interactable = true;
                priceText.text = "Price: " + bobber.intPrice;
            }
        }

        public void BuyBasicBobber() { TryBuyBobber(0); }
        public void BuyAdvancedBobber() { TryBuyBobber(1); }
        public void BuyProBobber() { TryBuyBobber(2); }

        protected void TryBuyBobber(int bobberIndex)
        {
            BobberData bobber = listBobberData[bobberIndex];

            if (!bobber.boolIsOwned)
            {
                bobber.boolIsOwned = true;
                Debug.Log("Bought: " + bobber.strBobberName);
            }

            EquipBobber(bobberIndex);
            UpdateBobberButtons();
        }

        protected void EquipBobber(int bobberIndex)
        {
            foreach (BobberData bobber in listBobberData)
            {
                bobber.boolIsEquipped = false;
            }
            listBobberData[bobberIndex].boolIsEquipped = true;
            Debug.Log("Equipped: " + listBobberData[bobberIndex].strBobberName);
        }
    }
}