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

        [Header("Minigame Settings")]
        [SerializeField] protected float floatTargetSpeed = 200f;
        [SerializeField] protected float floatProgressOnPerfect = 0.25f;
        [SerializeField] protected float floatProgressOnGood = 0.1f;
        [SerializeField] protected float floatProgressOnMiss = -0.15f;

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

            UpdateProgressBar();
            ResetTargetPosition();
            gameObjectMinigamePanel.SetActive(true);
        }

        protected void Update()
        {
            if (!boolIsMinigameActive) return;

            MoveTarget();

            // INPUT UNTUK MOUSE (EDITOR) & TOUCH (MOBILE)
            if (Input.GetMouseButtonDown(0))
            {
                OnTap();
            }
        }

        protected void MoveTarget()
        {
            // Dapatkan width parent area
            RectTransform parentArea = rectTransformTarget.parent.GetComponent<RectTransform>();
            float areaWidth = parentArea.rect.width;
            float targetWidth = rectTransformTarget.rect.width;

            // Gerakkan target
            float newX = rectTransformTarget.anchoredPosition.x + (floatTargetSpeed * intTargetDirection * Time.deltaTime);

            // Batasi agar tidak keluar area
            float bound = (areaWidth - targetWidth) / 2f;
            newX = Mathf.Clamp(newX, -bound, bound);

            rectTransformTarget.anchoredPosition = new Vector2(newX, rectTransformTarget.anchoredPosition.y);

            // Balik arah jika sampai ujung
            if (Mathf.Abs(newX) >= bound - 1f) // -1f untuk tolerance
            {
                intTargetDirection *= -1;
            }
        }

        protected void OnTap()
        {
            if (!boolIsMinigameActive) return;

            // Cek posisi target relatif terhadap player bar
            float distance = Mathf.Abs(rectTransformTarget.anchoredPosition.x - rectTransformPlayerBar.anchoredPosition.x);
            float maxDistance = 50f;

            if (distance < 15f) // Perfect
            {
                floatCurrentProgress += floatProgressOnPerfect;
                Debug.Log("Perfect! Progress: " + (floatCurrentProgress * 100f) + "%");
            }
            else if (distance < maxDistance) // Good
            {
                floatCurrentProgress += floatProgressOnGood;
                Debug.Log("Good! Progress: " + (floatCurrentProgress * 100f) + "%");
            }
            else // Miss
            {
                floatCurrentProgress += floatProgressOnMiss;
                Debug.Log("Miss! Progress: " + (floatCurrentProgress * 100f) + "%");
            }

            // Clamp progress antara 0-1
            floatCurrentProgress = Mathf.Clamp01(floatCurrentProgress);
            UpdateProgressBar();

            // CEK JIKA PROGRESS 0% (GAGAL) ATAU 100% (BERHASIL)
            if (floatCurrentProgress >= 1f)
            {
                MinigameSuccess();
            }
            else if (floatCurrentProgress <= 0f) // TAMBAH INI
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
            Debug.Log("Berhasil dapat ikan: " + currentFishData.strFishName);
            boolIsMinigameActive = false;
            gameObjectMinigamePanel.SetActive(false);

            // TODO: Panggil reset fishing line
        }

        protected void MinigameFailed()
        {
            Debug.Log("Minigame gagal! Ikan lepas!");
            boolIsMinigameActive = false;
            gameObjectMinigamePanel.SetActive(false);

            // TODO: Panggil reset fishing line
        }

        public bool IsMinigameActive()
        {
            return boolIsMinigameActive;
        }
    }
}