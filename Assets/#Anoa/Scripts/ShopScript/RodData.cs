using UnityEngine;
using System.Collections.Generic;

namespace Anoa
{
    public enum RodType { Default, Iron, Gold, Diamond }

    [CreateAssetMenu(fileName = "RodData", menuName = "Anoa/RodData")]
    public class RodData : ScriptableObject
    {
        public RodType rodType;
        public string strRodName;
        public int intPrice;
        public Sprite spriteRod;
        public bool boolIsOwned;
        public bool boolIsEquipped;
        public int intProgressBonus;

        // +++ STATIC DATA UNTUK PERSISTENCE +++
        public static List<RodData> listAllRodData = new List<RodData>();
        public static RodData currentEquippedRod;
    }
}