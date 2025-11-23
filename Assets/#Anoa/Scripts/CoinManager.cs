using UnityEngine;
using UnityEngine.UI;

namespace Anoa
{
    public class CoinManager : MonoBehaviour
    {
        [SerializeField] protected Text textCoinCounter;
        protected static int intCurrentCoins;

        protected void Start()
        {
            //PlayerPrefs.DeleteKey("PlayerCoins");
            //SetCoinsForTesting(200);

            LoadCoins();
            UpdateCoinDisplay();
        }

        protected void LoadCoins()
        {
            if (PlayerPrefs.HasKey("PlayerCoins"))
            {
                intCurrentCoins = PlayerPrefs.GetInt("PlayerCoins");
            }
            else
            {
                intCurrentCoins = 0;
                SaveCoins();
            }
        }

        protected void SaveCoins()
        {
            PlayerPrefs.SetInt("PlayerCoins", intCurrentCoins);
            PlayerPrefs.Save();
        }

        // +++ METHOD TESTING +++
        public static void SetCoinsForTesting(int amount)
        {
            intCurrentCoins = Mathf.Max(0, amount);
            SaveCoinsStatic();
            UpdateAllCoinDisplays();
            Debug.Log($"Coins set to: {intCurrentCoins} (TEST MODE)");
        }

        public static void AddCoinsStatic(int amount)
        {
            intCurrentCoins += amount;
            intCurrentCoins = Mathf.Max(0, intCurrentCoins);
            SaveCoinsStatic();
            UpdateAllCoinDisplays();
        }

        protected static void SaveCoinsStatic()
        {
            PlayerPrefs.SetInt("PlayerCoins", intCurrentCoins);
            PlayerPrefs.Save();
        }

        protected static void UpdateAllCoinDisplays()
        {
            CoinManager[] allCoinManagers = FindObjectsByType<CoinManager>(FindObjectsSortMode.None);
            foreach (CoinManager manager in allCoinManagers)
            {
                manager.UpdateCoinDisplay();
            }
        }

        public void AddCoins(int amount)
        {
            AddCoinsStatic(amount);
            SaveCoins();
            UpdateCoinDisplay();
        }

        protected void UpdateCoinDisplay()
        {
            if (textCoinCounter != null)
            {
                textCoinCounter.text = intCurrentCoins.ToString();
            }
        }

        public int GetCurrentCoins()
        {
            return intCurrentCoins;
        }

        public static int GetStaticCoins()
        {
            return intCurrentCoins;
        }
    }
}