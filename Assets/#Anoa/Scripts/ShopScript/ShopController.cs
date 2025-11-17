using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Anoa
{
    public class ShopController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] protected Text textCoinCounter;
        [SerializeField] protected Button buttonBack;

        [Header("Category Buttons")]
        [SerializeField] protected Button buttonRod;
        [SerializeField] protected Button buttonBobber;
        [SerializeField] protected Button buttonBait;

        [Header("Content Pages")]
        [SerializeField] protected GameObject gameObjectRodPage;
        [SerializeField] protected GameObject gameObjectBobberPage;
        [SerializeField] protected GameObject gameObjectBaitPage;

        [Header("Button Colors")]
        [SerializeField] protected Color colorSelected = new Color(0.8f, 0.8f, 0.8f);
        [SerializeField] protected Color colorNormal = Color.white;

        protected void Start()
        {
            UpdateCoinDisplay(); // PASTIKAN ADA
            InitializeButtons();
            ShowRodPage();
        }

        protected void InitializeButtons()
        {
            buttonBack.onClick.AddListener(BackToFishing);
            buttonRod.onClick.AddListener(ShowRodPage);
            buttonBobber.onClick.AddListener(ShowBobberPage);
            buttonBait.onClick.AddListener(ShowBaitPage);
        }

        // PASTIKAN FUNCTION INI ADA
        protected void UpdateCoinDisplay()
        {
            int coins = CoinManager.GetStaticCoins();
            if (textCoinCounter != null)
            {
                textCoinCounter.text = coins.ToString();
            }
        }

        protected void ShowRodPage()
        {
            gameObjectRodPage.SetActive(true);
            gameObjectBobberPage.SetActive(false);
            gameObjectBaitPage.SetActive(false);

            SetButtonColor(buttonRod, colorSelected);
            SetButtonColor(buttonBobber, colorNormal);
            SetButtonColor(buttonBait, colorNormal);
        }

        protected void ShowBobberPage()
        {
            gameObjectRodPage.SetActive(false);
            gameObjectBobberPage.SetActive(true);
            gameObjectBaitPage.SetActive(false);

            SetButtonColor(buttonRod, colorNormal);
            SetButtonColor(buttonBobber, colorSelected);
            SetButtonColor(buttonBait, colorNormal);
        }

        protected void ShowBaitPage()
        {
            gameObjectRodPage.SetActive(false);
            gameObjectBobberPage.SetActive(false);
            gameObjectBaitPage.SetActive(true);

            SetButtonColor(buttonRod, colorNormal);
            SetButtonColor(buttonBobber, colorNormal);
            SetButtonColor(buttonBait, colorSelected);
        }

        protected void SetButtonColor(Button button, Color color)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = color;
            colors.selectedColor = color;
            button.colors = colors;
        }

        protected void BackToFishing()
        {
            SceneManager.LoadScene("ScenePlayGame");
        }
    }
}