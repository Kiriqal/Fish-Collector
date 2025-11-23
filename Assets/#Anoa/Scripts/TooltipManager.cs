using UnityEngine;
using UnityEngine.UI;

namespace Anoa
{
    public class TooltipManager : MonoBehaviour
    {
        [SerializeField] protected GameObject gameObjectTooltipPanel;
        [SerializeField] protected Text textTooltip;
        [SerializeField] protected Vector3 tooltipOffset = new Vector3(0, 40, 0);

        public void ShowTooltip(string fishName, Vector3 slotPosition, int rarity, bool isUnlocked)
        {
            if (gameObjectTooltipPanel != null && textTooltip != null)
            {
                
                string displayName = isUnlocked ? fishName : "???";
                textTooltip.text = displayName;
                textTooltip.color = GetRarityColor(rarity);
                gameObjectTooltipPanel.transform.position = slotPosition + tooltipOffset;
                gameObjectTooltipPanel.SetActive(true);
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

        public void HideTooltip()
        {
            if (gameObjectTooltipPanel != null)
            {
                gameObjectTooltipPanel.SetActive(false);
            }
        }
    }
}