using UnityEngine;
using UnityEngine.UI;

namespace Anoa
{
    public class CoinManager : MonoBehaviour
    {
        [SerializeField] protected Text textCoinCounter;
        protected static int intCurrentCoins; // STATIC untuk persist

        protected void Start()
        {
            UpdateCoinDisplay();
        }

        public void AddCoins(int amount)
        {
            intCurrentCoins += amount;
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

        // FUNCTION UNTUK SCENE SHOP
        public static int GetStaticCoins()
        {
            return intCurrentCoins;
        }
    }
}