using UnityEngine;

namespace Anoa
{
    public enum BobberType
    {

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
    }
}