using UnityEngine;
using System.Collections.Generic;

namespace Anoa
{
    public enum BobberType
    {
        None,
        Iron,
        Gold,
        Diamond
    }

    [CreateAssetMenu(fileName = "BobberData", menuName = "Anoa/BobberData")]
    public class BobberData : ScriptableObject
    {
        public BobberType bobberType;
        public string strBobberName;
        public int intPrice;
        public Sprite spriteBobber;
        public bool boolIsOwned;
        public bool boolIsEquipped;

        // TAMBAH INI - Persentase reduction speed (0.1 = 10%, 0.2 = 20%, dll)
        public float floatSpeedReduction;

        // STATIC DATA
        public static BobberData currentEquippedBobber;
        public static List<BobberData> listAllBobberData = new List<BobberData>();
    }
}