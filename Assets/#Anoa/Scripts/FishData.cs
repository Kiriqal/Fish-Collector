using UnityEngine;

namespace Anoa
{
    // ScriptableObject untuk data ikan
    [CreateAssetMenu(fileName = "FishData", menuName = "Anoa/FishData")]
    public class FishData : ScriptableObject
    {
        public string strFishName;
        public Sprite spriteFish;
        public int intRarity; // 1-3 (common, rare, epic)
        public float floatCatchRate; // 0-1
    }
}