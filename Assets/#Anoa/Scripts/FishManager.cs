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
            if (listFishData.Count > 0)
            {
                // Random berdasarkan rarity
                int randomIndex = Random.Range(0, listFishData.Count);
                return listFishData[randomIndex];
            }
            return null;
        }
    }
}