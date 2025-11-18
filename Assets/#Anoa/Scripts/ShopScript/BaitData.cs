using UnityEngine;
using System.Collections.Generic;

namespace Anoa
{
    public enum BaitType { Worm, SweetCorn, Crickets }

    [CreateAssetMenu(fileName = "BaitData", menuName = "Anoa/BaitData")]
    public class BaitData : ScriptableObject
    {
        public BaitType baitType;
        public string strBaitName;
        public int intPrice;
        public Sprite spriteBait;
        public int intQuantity;

        // TAMBAH INI - Buff luck (% chance upgrade ke rarity lebih tinggi)
        public float floatLuckBonus;

        public static List<BaitData> listAllBaitData = new List<BaitData>();
    }
}