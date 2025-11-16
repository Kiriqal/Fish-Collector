using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Anoa
{
    public class FishingRodController : MonoBehaviour
    {
        [SerializeField] protected Transform transFishingLine;
        [SerializeField] protected float floatLineSpeed = 3f;
        [SerializeField] protected float floatMaxLineLength = 3f;
        [SerializeField] protected float floatWaitTime = 2f;

        // Reference ke BoatMovementController
        [SerializeField] protected BoatMovementController boatMovementController;
        [SerializeField] protected FishManager fishManager;
        [SerializeField] protected MinigameController minigameController;

        [SerializeField] protected GameObject gameObjectExclamationMark;
        [SerializeField] protected Text textExclamation; // Ganti dari SpriteRenderer


        protected bool boolIsFishing;
        protected float floatCurrentLineLength;
        protected Vector3 vecOriginalLinePosition;

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
                StartCoroutine(FishingProcess());
            }
        }

        protected IEnumerator FishingProcess()
        {
            boolIsFishing = true;

            if (boatMovementController != null)
            {
                boatMovementController.SetCanMove(false);
            }

            // TURUNKAN TALI
            while (floatCurrentLineLength < floatMaxLineLength)
            {
                floatCurrentLineLength += floatLineSpeed * Time.deltaTime;
                UpdateLineLength();
                yield return null;
            }

            yield return new WaitForSeconds(floatWaitTime);

            // SELALU DAPAT IKAN
            if (fishManager != null)
            {
                FishData caughtFish = fishManager.GetRandomFish();

                // TAMPILKAN TANDA SERU SEBELUM MINIGAME
                ShowExclamationMark(caughtFish.intRarity);
                yield return new WaitForSeconds(1f); // Tampil 1 detik
                HideExclamationMark();

                // LANJUT KE MINIGAME
                if (minigameController != null)
                {
                    minigameController.StartMinigame(caughtFish);
                    yield return StartCoroutine(WaitForMinigameToComplete());
                }
            }
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