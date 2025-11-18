using UnityEngine;
using System.Collections.Generic;

namespace Anoa
{
    public class FishManager : MonoBehaviour
    {
        [Header("Bait Reference")]
        [SerializeField] protected BaitSelectionManager baitSelectionManager;

        [SerializeField] protected List<FishData> listFishData;

        protected void Start()
        {
            InitializeFishData();
        }

        protected void InitializeFishData()
        {
            if (listFishData.Count == 0)
            {
                Debug.Log("Buat 3 FishData di Project window: Create → Anoa → FishData");
            }
        }

        public FishData GetRandomFish()
        {
            if (listFishData.Count == 0)
            {
                Debug.LogError("FishManager: listFishData KOSONG!");
                return null;
            }

            float luckBonus = 0f;

            // DAPATKAN LUCK DARI BAIT YANG DIPAKAI
            if (baitSelectionManager != null && baitSelectionManager.HasEquippedBait())
            {
                luckBonus = baitSelectionManager.GetEquippedBaitLuck();
                Debug.Log($"Bait luck bonus: {luckBonus * 100}%");
            }

            float randomValue = Random.Range(0f, 100f);
            FishData caughtFish = null;

            // APPLY LUCK BONUS KE RARITY CHANCE
            if (randomValue < (95f - (luckBonus * 100f))) // Common berkurang
            {
                caughtFish = GetFishByRarity(1);
                Debug.Log($"Dapat Common fish (dengan bait luck: {luckBonus * 100}%)");
            }
            else if (randomValue < (99f - (luckBonus * 50f))) // Epic bertambah
            {
                caughtFish = GetFishByRarity(2);
                Debug.Log($"Dapat Epic fish (dengan bait luck: {luckBonus * 100}%)");
            }
            else // Mythic bertambah
            {
                caughtFish = GetFishByRarity(3);
                Debug.Log($"Dapat Mythic fish (dengan bait luck: {luckBonus * 100}%)");
            }

            return caughtFish;
        }

        protected FishData GetFishByRarity(int rarity)
        {
            List<FishData> fishWithRarity = listFishData.FindAll(fish => fish.intRarity == rarity);
            if (fishWithRarity.Count > 0)
            {
                return fishWithRarity[Random.Range(0, fishWithRarity.Count)];
            }
            return listFishData[0]; // Fallback
        }
    }
}