using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Anoa
{
    public class MinigameController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] protected GameObject gameObjectMinigamePanel;
        [SerializeField] protected Slider sliderProgressBar;
        [SerializeField] protected RectTransform rectTransformTarget;
        [SerializeField] protected RectTransform rectTransformPlayerBar;
        [SerializeField] protected FishPopupController fishPopupController;

        [Header("Minigame Settings")]
        [SerializeField] protected float floatTargetSpeed = 200f;
        [SerializeField] protected FishCounterController fishCounterController;

        [Header("References")]
        [SerializeField] protected RodManager rodManager;
        [SerializeField] protected BobberManager bobberManager; // TAMBAH INI

        [Header("Fish Speed Multipliers")]
        [SerializeField] protected float floatCommonSpeedMultiplier = 0.4f;    // 40/100
        [SerializeField] protected float floatEpicSpeedMultiplier = 0.6f;      // 60/100  
        [SerializeField] protected float floatMythicSpeedMultiplier = 0.8f;    // 80/100

        protected bool boolIsMinigameActive;
        protected int intTargetDirection = 1;
        protected float floatCurrentProgress;
        protected FishData currentFishData;

        protected void Start()
        {
            InitializeMinigame();
        }

        protected void InitializeMinigame()
        {
            gameObjectMinigamePanel.SetActive(false);
        }

        public void StartMinigame(FishData fishData)
        {
            currentFishData = fishData;
            floatCurrentProgress = 0f;
            boolIsMinigameActive = true;

            UpdatePlayerBarSize();
            UpdateProgressBar();
            ResetTargetPosition();
            gameObjectMinigamePanel.SetActive(true);
        }

        protected void UpdatePlayerBarSize()
        {
            if (rodManager != null && rectTransformPlayerBar != null)
            {
                RodData equippedRod = rodManager.GetEquippedRod();
                float barSize = 18f; // Default 18

                if (equippedRod != null)
                {
                    switch (equippedRod.rodType)
                    {
                        case RodType.Default: barSize = 18f; break;
                        case RodType.Iron: barSize = 29f; break;
                        case RodType.Gold: barSize = 40f; break;
                        case RodType.Diamond: barSize = 50f; break;
                    }
                }

                rectTransformPlayerBar.sizeDelta = new Vector2(barSize, rectTransformPlayerBar.sizeDelta.y);
                Debug.Log($"Player Bar Size: {barSize}/100 (Rod: {equippedRod?.strRodName})");
            }
        }

        protected void Update()
        {
            if (!boolIsMinigameActive) return;

            MoveTarget();

            if (Input.GetMouseButtonDown(0))
            {
                OnTap();
            }
        }

        protected void MoveTarget()
        {
            RectTransform parentArea = rectTransformTarget.parent.GetComponent<RectTransform>();
            float areaWidth = parentArea.rect.width;
            float targetWidth = rectTransformTarget.rect.width;

            // HITUNG FINAL SPEED DENGAN BOBBER BUFF
            float finalSpeed = GetFinalTargetSpeed();
            float newX = rectTransformTarget.anchoredPosition.x + (finalSpeed * intTargetDirection * Time.deltaTime);

            float bound = (areaWidth - targetWidth) / 2f;
            newX = Mathf.Clamp(newX, -bound, bound);

            rectTransformTarget.anchoredPosition = new Vector2(newX, rectTransformTarget.anchoredPosition.y);

            if (Mathf.Abs(newX) >= bound - 1f)
            {
                intTargetDirection *= -1;
            }
        }

        protected float GetFinalTargetSpeed()
        {
            // BASE SPEED BERDASARKAN RARITY IKAN
            float baseSpeedMultiplier = 1f;

            if (currentFishData != null)
            {
                switch (currentFishData.intRarity)
                {
                    case 1: baseSpeedMultiplier = floatCommonSpeedMultiplier; break;    // Common: 40%
                    case 2: baseSpeedMultiplier = floatEpicSpeedMultiplier; break;      // Epic: 60%
                    case 3: baseSpeedMultiplier = floatMythicSpeedMultiplier; break;    // Mythic: 80%
                }
            }

            // APPLY BOBBER SPEED REDUCTION
            float bobberReduction = 0f;
            if (bobberManager != null)
            {
                bobberReduction = bobberManager.GetSpeedReductionPercent();
            }

            float finalSpeed = floatTargetSpeed * baseSpeedMultiplier * (1f - bobberReduction);

            Debug.Log($"Target Speed: {finalSpeed} (Base: {floatTargetSpeed * baseSpeedMultiplier}, " +
                     $"Bobber Reduction: {bobberReduction * 100}%, Fish Rarity: {currentFishData?.intRarity})");

            return finalSpeed;
        }

        protected void OnTap()
        {
            if (!boolIsMinigameActive) return;

            float playerBarHalfWidth = rectTransformPlayerBar.rect.width / 2f;
            float playerBarLeft = rectTransformPlayerBar.anchoredPosition.x - playerBarHalfWidth;
            float playerBarRight = rectTransformPlayerBar.anchoredPosition.x + playerBarHalfWidth;

            float targetPositionX = rectTransformTarget.anchoredPosition.x;
            bool isTargetInPlayerBar = (targetPositionX >= playerBarLeft && targetPositionX <= playerBarRight);

            if (isTargetInPlayerBar)
            {
                floatCurrentProgress += 0.25f;
                Debug.Log("Perfect! +25% progress (Target dalam player bar)");
            }
            else
            {
                floatCurrentProgress -= 0.15f;
                float distance = Mathf.Abs(targetPositionX - rectTransformPlayerBar.anchoredPosition.x);
                Debug.Log($"Miss! -15% progress (Target di luar player bar, jarak: {distance})");
            }

            floatCurrentProgress = Mathf.Clamp01(floatCurrentProgress);
            UpdateProgressBar();

            if (floatCurrentProgress >= 1f)
            {
                MinigameSuccess();
            }
            else if (floatCurrentProgress <= 0f)
            {
                MinigameFailed();
            }
        }

        protected void UpdateProgressBar()
        {
            sliderProgressBar.value = floatCurrentProgress;
        }

        protected void ResetTargetPosition()
        {
            rectTransformTarget.anchoredPosition = new Vector2(0, 0);
        }

        protected void MinigameSuccess()
        {
            Debug.Log("Dapat ikan: " + currentFishData.strFishName);

            if (fishCounterController != null)
            {
                fishCounterController.AddFish();
            }

            if (fishPopupController != null)
            {
                fishPopupController.ShowFishPopup(currentFishData);
            }

            boolIsMinigameActive = false;
            gameObjectMinigamePanel.SetActive(false);
        }

        protected void MinigameFailed()
        {
            Debug.Log("Minigame gagal! Ikan lepas!");
            boolIsMinigameActive = false;
            gameObjectMinigamePanel.SetActive(false);
        }

        public bool IsMinigameActive()
        {
            return boolIsMinigameActive;
        }
    }
}