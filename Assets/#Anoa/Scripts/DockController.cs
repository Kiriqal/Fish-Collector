using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Anoa
{
    public class DockController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] protected GameObject gameObjectDockUI;
        [SerializeField] protected GameObject gameObjectSellConfirmPanel;
        [SerializeField] protected FishCounterController fishCounterController;
        [SerializeField] protected GameObject gameObjectNoFishNotification;

        [Header("Detection Settings")]
        [SerializeField] protected float floatDetectionRange = 2f;

        [SerializeField] protected FishingRodController fishingRodController;
        [SerializeField] protected CoinManager coinManager;


        protected Transform transBoat;
        protected bool boolIsBoatNearby;
        protected static Vector3 savedBoatPosition;

        protected void Start()
        {
            transBoat = GameObject.FindGameObjectWithTag("Player").transform;

            // LOAD POSISI JIKA ADA YANG DISIMPAN
            if (savedBoatPosition != Vector3.zero)
            {
                transBoat.position = savedBoatPosition;
            }

            InitializeUI();
        }

        public void OnShopClick()
        {
            // SIMPAN POSISI SEBELUM PINDAH SCENE
            if (transBoat != null)
            {
                savedBoatPosition = transBoat.position;
            }

            LoadShopScene();
        }

        protected void LoadShopScene()
        {
            SceneManager.LoadScene("ShopScene");
        }

        protected void InitializeUI()
        {
            gameObjectDockUI.SetActive(false);
            gameObjectSellConfirmPanel.SetActive(false);
        }

        protected void Update()
        {
            CheckBoatDistance();
        }

        protected void CheckBoatDistance()
        {
            if (transBoat == null) return;

            float distance = Vector2.Distance(transform.position, transBoat.position);
            bool wasNearby = boolIsBoatNearby;
            boolIsBoatNearby = distance <= floatDetectionRange;

            if (boolIsBoatNearby != wasNearby)
            {
                gameObjectDockUI.SetActive(boolIsBoatNearby);
            }
        }

        public void OnSellClick()
        {
            if (fishCounterController != null)
            {
                if (fishCounterController.GetCurrentFishCount() > 0)
                {
                    ShowSellConfirm();
                }
                else
                {
                    ShowNoFishNotification();
                }
            }
        }

        protected void ShowNoFishNotification()
        {
            if (gameObjectNoFishNotification != null)
            {
                gameObjectNoFishNotification.SetActive(true);
                StartCoroutine(HideNoFishNotification());
            }
        }

        protected IEnumerator HideNoFishNotification()
        {
            yield return new WaitForSeconds(2f);
            gameObjectNoFishNotification.SetActive(false);
        }

        protected void ShowSellConfirm()
        {
            gameObjectSellConfirmPanel.SetActive(true);
        }

        public void ConfirmSell()
        {
            if (fishingRodController != null && coinManager != null)
            {
                List<FishData> caughtFishes = fishingRodController.GetCaughtFishes();
                int totalCoins = 0;

                foreach (FishData fish in caughtFishes)
                {
                    if (fish.intRarity == 1) totalCoins += 5;      // Common
                    else if (fish.intRarity == 2) totalCoins += 10; // Epic
                    else if (fish.intRarity == 3) totalCoins += 20; // Mythic
                }

                coinManager.AddCoins(totalCoins);
                fishingRodController.ClearCaughtFishes();
                fishCounterController.ResetCounter();
            }
            gameObjectSellConfirmPanel.SetActive(false);
        }

        public void CancelSell()
        {
            gameObjectSellConfirmPanel.SetActive(false);
        }

        
    }
}