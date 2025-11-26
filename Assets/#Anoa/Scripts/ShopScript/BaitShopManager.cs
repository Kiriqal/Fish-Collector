using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Anoa
{
    public class BaitShopManager : MonoBehaviour
    {
        [Header("Bait Items")]
        [SerializeField] protected List<BaitData> listBaitData;

        [Header("Notification System")]
        [SerializeField] protected Transform transNotificationParent;
        [SerializeField] protected GameObject gameObjectNotificationTemplate;
        [SerializeField] protected int intMaxNotifications = 5;

        protected Queue<GameObject> queueActiveNotifications = new Queue<GameObject>();

        [Header("Notification Reference")]
        [SerializeField] protected NotificationManager notificationManager;

        protected void Start()
        {
            InitializeBaitShop();
        }

        protected void InitializeBaitShop()
        {
            foreach (BaitData bait in listBaitData)
            {
                if (!BaitData.listAllBaitData.Contains(bait))
                {
                    BaitData.listAllBaitData.Add(bait);
                }
            }
        }

        protected void TryBuyBait(BaitType baitType, int quantity)
        {
            BaitData bait = GetBaitByType(baitType);
            if (bait == null) return;

            int playerCoins = CoinManager.GetStaticCoins();
            int totalPrice = bait.intPrice * quantity;

            if (playerCoins >= totalPrice)
            {
                CoinManager.AddCoinsStatic(-totalPrice);
                bait.intQuantity += quantity;

                // +++ PAKAI STACK SYSTEM SENDIRI +++
                ShowBaitNotification(bait.strBaitName);

                UpdateBaitButtons();
            }
            else
            {
                // +++ PAKAI NOTIFICATION MANAGER UNTUK ERROR +++
                notificationManager?.ShowErrorNotification("Not enough coins!");
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

        protected void UpdateBaitButtons()
        {
            
        }

        // DAN TAMBAH METHOD INI JIKA BELUM ADA:
        public void BuyWormBait() { TryBuyBait(BaitType.Worm, 1); }
        public void BuySweetCornBait() { TryBuyBait(BaitType.SweetCorn, 1); }
        public void BuyCricketsBait() { TryBuyBait(BaitType.Crickets, 1); }

        protected void ShowBaitNotification(string baitName)
        {
            if (gameObjectNotificationTemplate == null) return;

            GameObject newNotification = Instantiate(gameObjectNotificationTemplate, transNotificationParent);
            newNotification.SetActive(true);

            Text notificationText = newNotification.GetComponentInChildren<Text>();
            if (notificationText != null)
            {
                notificationText.text = "+1 " + baitName;
            }

            RectTransform rectTransform = newNotification.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -40 * queueActiveNotifications.Count);

            queueActiveNotifications.Enqueue(newNotification);

            if (queueActiveNotifications.Count > intMaxNotifications)
            {
                GameObject oldestNotification = queueActiveNotifications.Dequeue();
                Destroy(oldestNotification);
                UpdateNotificationPositions();
            }

            StartCoroutine(HideNotificationAfterDelay(newNotification));
        }

        public void ResetNotifications()
        {
            StopAllCoroutines();

            queueActiveNotifications.Clear();

            if (transNotificationParent != null)
            {
                foreach (Transform child in transNotificationParent)
                {
                    if (child.gameObject != gameObjectNotificationTemplate)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }

            Debug.Log("Bait notifications reset");
        }

        protected void UpdateNotificationPositions()
        {
            int index = 0;
            foreach (GameObject notification in queueActiveNotifications)
            {
                RectTransform rectTransform = notification.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, -40 * index);
                index++;
            }
        }

        protected IEnumerator HideNotificationAfterDelay(GameObject notification)
        {
            yield return new WaitForSeconds(2f);

            if (notification != null)
            {
                Queue<GameObject> newQueue = new Queue<GameObject>();
                foreach (GameObject notif in queueActiveNotifications)
                {
                    if (notif != notification)
                        newQueue.Enqueue(notif);
                }
                queueActiveNotifications = newQueue;
                Destroy(notification);
            }
        }
    }
}