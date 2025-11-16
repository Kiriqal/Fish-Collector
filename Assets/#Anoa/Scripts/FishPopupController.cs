using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Anoa
{
    public class FishPopupController : MonoBehaviour
    {
        [SerializeField] protected GameObject gameObjectPopupPanel;
        [SerializeField] protected Image imageFish;
        [SerializeField] protected Text textFishName;
        [SerializeField] protected Text textFishRarity;

        protected void Start()
        {
           
        }

        public void ShowFishPopup(FishData fishData)
        {
            textFishName.text = fishData.strFishName;
            textFishRarity.text = GetRarityName(fishData.intRarity);

            // SET WARNA BERDASARKAN RARITY
            SetRarityColor(fishData.intRarity);

            if (fishData.spriteFish != null)
                imageFish.sprite = fishData.spriteFish;

            gameObjectPopupPanel.SetActive(true);
            StartCoroutine(AutoClosePopup());
        }

        protected void SetRarityColor(int rarity)
        {
            switch (rarity)
            {
                case 1: // Common - Hijau
                    textFishRarity.color = new Color(0.2f, 0.8f, 0.2f); // Hijau
                    break;
                case 2: // Epic - Oren
                    textFishRarity.color = new Color(1f, 0.5f, 0f); // Oren
                    break;
                case 3: // Mythic - Merah cerah
                    textFishRarity.color = new Color(1f, 0.2f, 0.2f); // Merah cerah
                    break;
                default:
                    textFishRarity.color = Color.white;
                    break;
            }
        }

        protected IEnumerator AutoClosePopup()
        {
            yield return new WaitForSeconds(3f);
            gameObjectPopupPanel.SetActive(false);
        }

        protected string GetRarityName(int rarity)
        {
            switch (rarity)
            {
                case 1: return "Common";
                case 2: return "Epic";
                case 3: return "Mythic";
                default: return "Unknown";
            }
        }
    }
}