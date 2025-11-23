using UnityEngine;
using UnityEngine.UI;

namespace Anoa
{
    public class ProfileUIController : MonoBehaviour
    {
        [Header("Profile References")]
        [SerializeField] protected Button buttonProfileButton;
        [SerializeField] protected GameObject gameObjectProfilePanel;
        [SerializeField] protected Text textRodBuff;
        [SerializeField] protected Text textBobberBuff;
        [SerializeField] protected Text textBaitBuff;

        [Header("Manager References")]
        [SerializeField] protected RodManager rodManager;
        [SerializeField] protected BobberManager bobberManager;
        [SerializeField] protected BaitSelectionManager baitSelectionManager;

        protected bool boolIsProfileOpen = false;

        protected void Start()
        {
            buttonProfileButton.onClick.AddListener(ShowProfilePanel);

            Button closeButton = gameObjectProfilePanel.GetComponentInChildren<Button>();
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(CloseProfilePanel);
            }

            gameObjectProfilePanel.SetActive(false);
            UpdateBuffTexts();
        }

        protected void ShowProfilePanel()
        {
            boolIsProfileOpen = true;
            gameObjectProfilePanel.SetActive(true);
            SetOtherButtonsInteractable(false);
            UpdateBuffTexts();
        }

        protected void CloseProfilePanel()
        {
            boolIsProfileOpen = false;
            gameObjectProfilePanel.SetActive(false);
            SetOtherButtonsInteractable(true);
        }

        protected void SetOtherButtonsInteractable(bool interactable)
        {
            Button[] allButtons = FindObjectsByType<Button>(FindObjectsSortMode.None);
            Button closeButton = gameObjectProfilePanel.GetComponentInChildren<Button>();

            foreach (Button btn in allButtons)
            {
                if (btn != buttonProfileButton && btn != closeButton)
                {
                    btn.interactable = interactable;
                }
            }
        }
        protected void UpdateBuffTexts()
        {
            UpdateRodBuffText();
            UpdateBobberBuffText();
            UpdateBaitBuffText();
        }

        protected void UpdateRodBuffText()
        {
            if (rodManager != null)
            {
                RodData equippedRod = rodManager.GetEquippedRod();
                if (equippedRod != null)
                {
                    float barSize = GetRodBarSize(equippedRod.rodType);
                    textRodBuff.text = "Rod: +" + barSize + " Size";
                }
                else
                {
                    textRodBuff.text = "Rod: +18 Size (Default)";
                }
            }
        }

        protected void UpdateBobberBuffText()
        {
            if (bobberManager != null)
            {
                BobberData equippedBobber = bobberManager.GetEquippedBobber();
                if (equippedBobber != null && equippedBobber.bobberType != BobberType.None)
                {
                    int reductionPercent = Mathf.RoundToInt(equippedBobber.floatSpeedReduction * 100);
                    textBobberBuff.text = "Bobber: -" + reductionPercent + "% Speed";
                }
                else
                {
                    textBobberBuff.text = "Bobber: -0% Speed";
                }
            }
        }

        protected void UpdateBaitBuffText()
        {
            if (baitSelectionManager != null)
            {
                BaitData equippedBait = baitSelectionManager.GetEquippedBait();
                if (equippedBait != null)
                {
                    int luckPercent = Mathf.RoundToInt(equippedBait.floatLuckBonus * 100);
                    textBaitBuff.text = "Bait: +" + luckPercent + "% Luck";
                }
                else
                {
                    textBaitBuff.text = "Bait: +0% Luck";
                }
            }
        }

        protected float GetRodBarSize(RodType rodType)
        {
            switch (rodType)
            {
                case RodType.Default: return 18f;
                case RodType.Iron: return 29f;
                case RodType.Gold: return 40f;
                case RodType.Diamond: return 50f;
                default: return 18f;
            }
        }
    }
}