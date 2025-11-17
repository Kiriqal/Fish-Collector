using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Anoa
{
    public class BaitSelectionManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] protected GameObject gameObjectBaitPanel;
        [SerializeField] protected Button buttonBaitButton;

        [Header("Bait Quantity Texts")]
        [SerializeField] protected Text textWormQuantity;
        [SerializeField] protected Text textSweetCornQuantity;
        [SerializeField] protected Text textCricketsQuantity;

        [Header("Equip Buttons")]
        [SerializeField] protected Button buttonWormEquip;
        [SerializeField] protected Button buttonSweetCornEquip;
        [SerializeField] protected Button buttonCricketsEquip;

        [Header("Button Colors")]
        [SerializeField] protected Color colorEquipped = new Color(0.8f, 0.8f, 0.8f);
        [SerializeField] protected Color colorNormal = Color.white;
        [SerializeField] protected Color colorEmpty = new Color(0.5f, 0.5f, 0.5f);

        [Header("Bait Data References")]
        [SerializeField] protected List<BaitData> listBaitData;

        protected BaitData currentEquippedBait;

        protected void Start()
        {
            InitializeBaitData();
            buttonBaitButton.onClick.AddListener(ShowBaitPanel);
            UpdateBaitButtons();
        }

        protected void InitializeBaitData()
        {
            // GUNAKAN BAITDATA YANG SAMA DENGAN SHOPScene
            // Pastikan di Inspector, listBaitData diisi dengan Bait_Worm, Bait_SweetCorn, Bait_Crickets
            // YANG SAMA dengan yang ada di BaitShopManager
        }



        public void ShowBaitPanel()
        {
            UpdateBaitQuantities();
            UpdateBaitButtons();
            gameObjectBaitPanel.SetActive(true);
        }

        public void CloseBaitPanel()
        {
            gameObjectBaitPanel.SetActive(false);
        }

        protected void UpdateBaitQuantities()
        {
            foreach (BaitData bait in listBaitData)
            {
                if (bait.baitType == BaitType.Worm)
                    textWormQuantity.text = bait.intQuantity.ToString();
                else if (bait.baitType == BaitType.SweetCorn)
                    textSweetCornQuantity.text = bait.intQuantity.ToString();
                else if (bait.baitType == BaitType.Crickets)
                    textCricketsQuantity.text = bait.intQuantity.ToString();
            }
        }

        protected void UpdateBaitButtons()
        {
            UpdateSingleBaitButton(BaitType.Worm, buttonWormEquip);
            UpdateSingleBaitButton(BaitType.SweetCorn, buttonSweetCornEquip);
            UpdateSingleBaitButton(BaitType.Crickets, buttonCricketsEquip);
        }

        protected void UpdateSingleBaitButton(BaitType baitType, Button button)
        {
            BaitData bait = GetBaitByType(baitType);
            if (bait == null) return;

            Text buttonText = button.GetComponentInChildren<Text>();

            Debug.Log($"Update button: {bait.strBaitName}, Quantity: {bait.intQuantity}");

            if (bait.intQuantity == 0)
            {
                buttonText.text = "EMPTY";
                button.interactable = false;
                button.GetComponent<Image>().color = colorEmpty;
            }
            else if (bait == currentEquippedBait)
            {
                buttonText.text = "EQUIPPED";
                button.interactable = false;
                button.GetComponent<Image>().color = colorEquipped;
            }
            else
            {
                buttonText.text = "EQUIP";
                button.interactable = true;
                button.GetComponent<Image>().color = colorNormal;
            }
        }

        protected BaitData GetBaitByType(BaitType baitType)
        {
            foreach (BaitData bait in listBaitData)
            {
                if (bait.baitType == baitType)
                    return bait;
            }
            return null;
        }

        public void EquipWormBait() { EquipBait(BaitType.Worm); }
        public void EquipSweetCornBait() { EquipBait(BaitType.SweetCorn); }
        public void EquipCricketsBait() { EquipBait(BaitType.Crickets); }

        protected void EquipBait(BaitType baitType)
        {
            BaitData bait = GetBaitByType(baitType);
            if (bait != null && bait.intQuantity > 0)
            {
                currentEquippedBait = bait;
                UpdateBaitButtons();
                Debug.Log("Equipped: " + bait.strBaitName);
            }
        }

        public bool HasEquippedBait()
        {
            return currentEquippedBait != null && currentEquippedBait.intQuantity > 0;
        }

        public void UseEquippedBait()
        {
            if (currentEquippedBait != null && currentEquippedBait.intQuantity > 0)
            {
                currentEquippedBait.intQuantity--;
                Debug.Log($"Used 1 {currentEquippedBait.strBaitName}, Remaining: {currentEquippedBait.intQuantity}");

                // UPDATE UI
                UpdateBaitQuantities();
                UpdateBaitButtons();

                // JIKA BAIT HABIS, AUTO EQUIP BAIT LAIN YANG ADA
                if (currentEquippedBait.intQuantity == 0)
                {
                    AutoEquipOtherBait();
                }
            }
        }

        protected void AutoEquipOtherBait()
        {
            // CARI BAIT LAIN YANG MASIH ADA QUANTITYNYA
            foreach (BaitData bait in listBaitData)
            {
                if (bait.intQuantity > 0 && bait != currentEquippedBait)
                {
                    currentEquippedBait = bait;
                    Debug.Log($"Auto equipped: {bait.strBaitName}");
                    UpdateBaitButtons();
                    return;
                }
            }

            // JIKA SEMUA BAIT HABIS
            currentEquippedBait = null;
            Debug.Log("Semua bait habis! Beli di shop.");
            UpdateBaitButtons();
        }
    }
}