
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Anoa
{
    public class FishingRodController : MonoBehaviour
    {
        [Header("Fishing Settings")]
        [SerializeField] protected Transform transFishingLine;
        [SerializeField] protected float floatLineSpeed = 3f;
        [SerializeField] protected float floatMaxLineLength = 3f;
        [SerializeField] protected float floatWaitTime = 2f;

        [Header("References")]
        [SerializeField] protected BoatMovementController boatMovementController;
        [SerializeField] protected FishManager fishManager;
        [SerializeField] protected MinigameController minigameController;
        [SerializeField] protected FishCounterController fishCounterController; // TAMBAH INI

        [Header("Exclamation Mark")]
        [SerializeField] protected GameObject gameObjectExclamationMark;
        [SerializeField] protected Text textExclamation;

        [SerializeField] protected GameObject gameObjectFullNotification;
        [SerializeField] protected BaitSelectionManager baitSelectionManager;
        [SerializeField] protected GameObject gameObjectBaitEmptyNotification;


        protected bool boolIsFishing;
        protected float floatCurrentLineLength;
        protected Vector3 vecOriginalLinePosition;
        protected List<FishData> listCaughtFishes = new List<FishData>();

        protected static List<FishData> listCaughtFishesStatic = new List<FishData>();


        protected void Start()
        {
            InitializeFishingLine();
        }

        protected void InitializeFishingLine()
        {
            floatCurrentLineLength = 0f;
            vecOriginalLinePosition = transFishingLine.localPosition;
            UpdateLineLength();
        }

        public void ThrowLine()
        {
            if (!boolIsFishing)
            {
                if (fishCounterController != null && fishCounterController.IsCollectionComplete())
                {
                    Debug.Log("Kapal penuh! Jual ikan di dermaga dulu.");
                    ShowFullNotification();
                }
                else if (baitSelectionManager != null && !baitSelectionManager.HasEquippedBait())
                {
                    ShowBaitEmptyNotification();
                }
                else
                {
                    StartCoroutine(FishingProcess());
                }
            }
        }

        protected void ShowBaitEmptyNotification()
        {
            if (gameObjectBaitEmptyNotification != null)
            {
                gameObjectBaitEmptyNotification.SetActive(true);
                StartCoroutine(HideBaitEmptyNotification());
            }
        }

        protected IEnumerator HideBaitEmptyNotification()
        {
            yield return new WaitForSeconds(2f);
            if (gameObjectBaitEmptyNotification != null)
            {
                gameObjectBaitEmptyNotification.SetActive(false);
            }
        }

        protected void ShowFullNotification()
        {
            if (gameObjectFullNotification != null)
            {
                gameObjectFullNotification.SetActive(true);
                StartCoroutine(HideNotificationAfterDelay());
            }
        }

        protected IEnumerator HideNotificationAfterDelay()
        {
            yield return new WaitForSeconds(2f);
            gameObjectFullNotification.SetActive(false);
        }


        protected IEnumerator FishingProcess()
        {
            boolIsFishing = true;

            if (baitSelectionManager != null)
            {
                baitSelectionManager.UseEquippedBait();
            }

            if (boatMovementController != null)
            {
                boatMovementController.SetCanMove(false);
            }

            while (floatCurrentLineLength < floatMaxLineLength)
            {
                floatCurrentLineLength += floatLineSpeed * Time.deltaTime;
                UpdateLineLength();
                yield return null;
            }

            yield return new WaitForSeconds(floatWaitTime);

            if (fishManager != null)
            {
                FishData caughtFish = fishManager.GetRandomFish();

                ShowExclamationMark(caughtFish.intRarity);
                yield return new WaitForSeconds(1f);
                HideExclamationMark();

                if (minigameController != null)
                {
                    minigameController.StartMinigame(caughtFish);
                    yield return StartCoroutine(WaitForMinigameToComplete());
                }

                // +++ SIMPAN KE STATIC LIST SETELAH MINIGAME SUKSES +++
                listCaughtFishesStatic.Add(caughtFish);
                Debug.Log($"Ikan tersimpan: {caughtFish.strFishName}. Total: {listCaughtFishesStatic.Count}");
            }

            StartCoroutine(ResetFishingLine());
        }


        public List<FishData> GetCaughtFishes()
        {
            return listCaughtFishesStatic;
        }

        public void ClearCaughtFishes()
        {
            listCaughtFishesStatic.Clear();
            Debug.Log("Semua ikan dijual, list cleared.");
        }


        protected void ShowExclamationMark(int rarity)
        {
            if (gameObjectExclamationMark != null && textExclamation != null)
            {
                Color markColor = GetRarityColor(rarity);
                textExclamation.color = markColor;
                gameObjectExclamationMark.SetActive(true);
            }
        }

        protected void HideExclamationMark()
        {
            if (gameObjectExclamationMark != null)
            {
                gameObjectExclamationMark.SetActive(false);
            }
        }

        protected Color GetRarityColor(int rarity)
        {
            switch (rarity)
            {
                case 1: return new Color(0.2f, 0.8f, 0.2f); // Hijau - Common
                case 2: return new Color(1f, 0.5f, 0f);     // Oren - Epic
                case 3: return new Color(1f, 0.2f, 0.2f);   // Merah - Mythic
                default: return Color.white;
            }
        }

        // COROUTINE UNTUK TUNGGU MINIGAME SELESAI
        protected IEnumerator WaitForMinigameToComplete()
        {
            while (minigameController.IsMinigameActive()) // BUTUH FUNCTION INI DI MINIGAMECONTROLLER
            {
                yield return null;
            }

            // MINIGAME SELESAI, RESET TALI
            StartCoroutine(ResetFishingLine());
        }

        protected void StartMinigame(FishData fishData)
        {
            Debug.Log("Dapat ikan: " + fishData.strFishName);
            // Nanti panggil minigame system dengan fishData
        }

        protected IEnumerator ResetFishingLine()
        {
            // NAIKKAN TALI
            while (floatCurrentLineLength > 0)
            {
                floatCurrentLineLength -= floatLineSpeed * Time.deltaTime;
                UpdateLineLength();
                yield return null;
            }

            // KAPAL BISA GERAK LAGI
            if (boatMovementController != null)
            {
                boatMovementController.SetCanMove(true);
            }

            boolIsFishing = false;
        }

        protected void UpdateLineLength()
        {
            if (transFishingLine != null)
            {
                transFishingLine.localScale = new Vector3(0.1f, floatCurrentLineLength, 1f);
                transFishingLine.localPosition = vecOriginalLinePosition + Vector3.down * (floatCurrentLineLength / 2f);
            }
        }


    }
}