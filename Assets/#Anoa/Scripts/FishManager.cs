using UnityEngine;
using System.Collections.Generic;

namespace Anoa
{
    public class FishManager : MonoBehaviour
    {
        [Header("Bait Reference")]
        [SerializeField] protected BaitSelectionManager baitSelectionManager;

        [SerializeField] protected List<FishData> listFishData;

        [Header("Location Based Spawn")]
        [SerializeField] protected Transform transBoat;
        [SerializeField] protected float floatMinX = -0.27f;
        [SerializeField] protected float floatMaxX = 13.8f;

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

            // +++ TAMBAH ZONE SYSTEM +++
            int currentZone = GetCurrentZone();
            float zoneModifier = GetZoneModifier(currentZone);

            // DEBUG LOG ZONE INFO
            Debug.Log($"🎯 Zona {currentZone} aktif! Boat Position: {transBoat.position.x}, Zone Modifier: {zoneModifier}%");

            // APPLY LUCK BONUS KE RARITY CHANCE + ZONE MODIFIER
            if (randomValue < (95f - (luckBonus * 100f) - zoneModifier)) // Common berkurang
            {
                caughtFish = GetFishByRarity(1);
                Debug.Log($"🎣 Dapat Common fish (Zona {currentZone}, luck: {luckBonus * 100}%, zone bonus: {zoneModifier}%)");
            }
            else if (randomValue < (99f - (luckBonus * 50f) - (zoneModifier * 0.5f))) // Epic bertambah
            {
                caughtFish = GetFishByRarity(2);
                Debug.Log($"🎣 Dapat Epic fish (Zona {currentZone}, luck: {luckBonus * 100}%, zone bonus: {zoneModifier}%)");
            }
            else // Mythic bertambah
            {
                caughtFish = GetFishByRarity(3);
                Debug.Log($"🎣 Dapat Mythic fish (Zona {currentZone}, luck: {luckBonus * 100}%, zone bonus: {zoneModifier}%)");
            }

            return caughtFish;
        }

        // METHOD ZONE MODIFIER
        protected float GetZoneModifier(int zone)
        {
            switch (zone)
            {
                case 1: return 0f;    // Zona 1: no change
                case 2: return 2f;    // Zona 2: +2% chance rare (dari 5%)
                case 3: return 4f;    // Zona 3: +4% chance rare (dari 12%)
                default: return 0f;
            }
        }

        protected int GetCurrentZone()
        {
            if (transBoat == null)
            {
                Debug.LogError("Trans Boat belum di-set!");
                return 1;
            }

            float boatX = transBoat.position.x;
            float zoneWidth = (floatMaxX - floatMinX) / 3f;

            // DEBUG ZONE CALCULATION
            Debug.Log($"📍 Boat X: {boatX}, MinX: {floatMinX}, MaxX: {floatMaxX}, Zone Width: {zoneWidth}");

            if (boatX < floatMinX + zoneWidth) return 1;
            if (boatX < floatMinX + (zoneWidth * 2)) return 2;
            return 3;
        }

        protected FishData GetFishByRarity(int rarity)
        {
            List<FishData> fishWithRarity = listFishData.FindAll(fish => fish.intRarity == rarity);
            if (fishWithRarity.Count > 0)
            {
                return fishWithRarity[Random.Range(0, fishWithRarity.Count)];
            }
            return listFishData[0];
        }
    }
}