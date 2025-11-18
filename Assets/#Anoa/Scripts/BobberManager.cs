using UnityEngine;

namespace Anoa
{
    public class BobberManager : MonoBehaviour
    {
        protected void Start()
        {
            InitializeBobber();
        }

        protected void InitializeBobber()
        {
            // Load equipped bobber dari static data
            if (BobberData.currentEquippedBobber != null)
            {
                Debug.Log("Loaded equipped bobber: " + BobberData.currentEquippedBobber.strBobberName);
            }
            else
            {
                CreateDefaultBobber();
            }
        }

        protected void CreateDefaultBobber()
        {
            // Default = no bobber (buff 0%)
            BobberData defaultBobber = ScriptableObject.CreateInstance<BobberData>();
            defaultBobber.bobberType = BobberType.None;
            defaultBobber.strBobberName = "No Bobber";
            defaultBobber.floatSpeedReduction = 0f;
            defaultBobber.boolIsOwned = true;
            defaultBobber.boolIsEquipped = true;
            BobberData.currentEquippedBobber = defaultBobber;
        }

        public float GetSpeedReductionPercent()
        {
            if (BobberData.currentEquippedBobber != null)
            {
                return BobberData.currentEquippedBobber.floatSpeedReduction;
            }
            return 0f;
        }

        public BobberData GetEquippedBobber()
        {
            return BobberData.currentEquippedBobber;
        }
    }
}