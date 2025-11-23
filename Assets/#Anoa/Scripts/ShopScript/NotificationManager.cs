using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Anoa
{
    public class NotificationManager : MonoBehaviour
    {
        [SerializeField] protected GameObject gameObjectNotificationPanel;
        [SerializeField] protected Text textNotification;
        [SerializeField] protected Image imageBackground;
        [SerializeField] protected float floatShowDuration = 2f;

        public void ShowErrorNotification(string message)
        {
            if (gameObjectNotificationPanel != null && textNotification != null && imageBackground != null)
            {
                textNotification.text = message;
                imageBackground.color = new Color(0.8f, 0.2f, 0.2f, 0.8f); // MERAH
                gameObjectNotificationPanel.SetActive(true);
                StartCoroutine(HideNotificationAfterDelay());
            }
        }

        protected IEnumerator HideNotificationAfterDelay()
        {
            yield return new WaitForSeconds(floatShowDuration);
            gameObjectNotificationPanel.SetActive(false);
        }
    }
}