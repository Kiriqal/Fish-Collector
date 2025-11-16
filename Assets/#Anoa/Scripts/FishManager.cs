using UnityEngine;
using System.Collections.Generic;

namespace Anoa
{
    public class FishManager : MonoBehaviour
    {
        [SerializeField] protected List<FishData> listFishData;

        protected void Start()
        {
            InitializeFishData();
        }

        protected void InitializeFishData()
        {
            // Buat 3 ikan dummy jika belum ada
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

            float randomValue = Random.Range(0f, 100f);

            if (randomValue < 95f) // Common 95%
            {
                Debug.Log("Dapat Common fish");
                return GetFishByRarity(1);
            }
            else if (randomValue < 99f) // Epic 4% 
            {
                Debug.Log("Dapat Epic fish");
                return GetFishByRarity(2);
            }
            else // Mythic 1%
            {
                Debug.Log("Dapat Mythic fish");
                return GetFishByRarity(3);
            }
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