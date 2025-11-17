using UnityEngine;

namespace Anoa
{
    public enum RodType
    {
        Iron,
        Gold,
        Diamond
    }

    [CreateAssetMenu(fileName = "RodData", menuName = "Anoa/RodData")]
    public class RodData : ScriptableObject
    {
        public RodType rodType;
        public string strRodName;
        public int intPrice;
        public Sprite spriteRod;
        public bool boolIsOwned;
        public bool boolIsEquipped;
    }
}