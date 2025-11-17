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

        public void BuyWormBait() { TryBuyBait(0); }
        public void BuySweetCornBait() { TryBuyBait(1); }
        public void BuyCricketsBait() { TryBuyBait(2); }

        protected void TryBuyBait(int baitIndex)
        {
            if (listBaitData == null || listBaitData.Count <= baitIndex) return;

            BaitData bait = listBaitData[baitIndex];
            bait.intQuantity++;
            ShowBaitNotification(bait.strBaitName);
            Debug.Log("Bought " + bait.strBaitName + ", Total: " + bait.intQuantity);
        }

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