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

        // +++ BUAT STATIC UNTUK PERSISTENCE +++
        protected static BaitData currentEquippedBaitStatic;

        protected void Start()
        {
            InitializeBaitData();
            buttonBaitButton.onClick.AddListener(ShowBaitPanel);
            UpdateBaitButtons();
        }

        protected void InitializeBaitData()
        {
            // +++ GUNAKAN STATIC VALUE +++
            if (currentEquippedBaitStatic != null)
            {
                Debug.Log($"Loaded equipped bait: {currentEquippedBaitStatic.strBaitName}");
            }
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

            if (bait.intQuantity == 0)
            {
                buttonText.text = "EMPTY";
                button.interactable = false;
                button.GetComponent<Image>().color = colorEmpty;
            }
            else if (bait == currentEquippedBaitStatic) // +++ PAKAI STATIC +++
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
                currentEquippedBaitStatic = bait; // +++ SIMPAN KE STATIC +++
                UpdateBaitButtons();
                Debug.Log("Equipped: " + bait.strBaitName + " (Luck: " + (bait.floatLuckBonus * 100) + "%)");
            }
        }

        public bool HasEquippedBait()
        {
            return currentEquippedBaitStatic != null && currentEquippedBaitStatic.intQuantity > 0; // +++ PAKAI STATIC +++
        }

        public float GetEquippedBaitLuck()
        {
            if (currentEquippedBaitStatic != null) // +++ PAKAI STATIC +++
            {
                return currentEquippedBaitStatic.floatLuckBonus;
            }
            return 0f;
        }

        public BaitData GetEquippedBait()
        {
            return currentEquippedBaitStatic; // +++ PAKAI STATIC +++
        }

        public void UseEquippedBait()
        {
            if (currentEquippedBaitStatic != null && currentEquippedBaitStatic.intQuantity > 0) // +++ PAKAI STATIC +++
            {
                currentEquippedBaitStatic.intQuantity--;
                Debug.Log($"Used 1 {currentEquippedBaitStatic.strBaitName}, Remaining: {currentEquippedBaitStatic.intQuantity}");

                UpdateBaitQuantities();
                UpdateBaitButtons();

                if (currentEquippedBaitStatic.intQuantity == 0)
                {
                    AutoEquipOtherBait();
                }
            }
        }

        protected void AutoEquipOtherBait()
        {
            foreach (BaitData bait in listBaitData)
            {
                if (bait.intQuantity > 0 && bait != currentEquippedBaitStatic) // +++ PAKAI STATIC +++
                {
                    currentEquippedBaitStatic = bait; // +++ SIMPAN KE STATIC +++
                    Debug.Log($"Auto equipped: {bait.strBaitName} (Luck: {bait.floatLuckBonus * 100}%)");
                    UpdateBaitButtons();
                    return;
                }
            }

            currentEquippedBaitStatic = null; // +++ SIMPAN KE STATIC +++
            Debug.Log("Semua bait habis! Beli di shop.");
            UpdateBaitButtons();
        }
    }
}