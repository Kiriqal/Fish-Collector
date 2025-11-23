using UnityEngine;
using System.Collections.Generic;

namespace Anoa
{
    public class RodManager : MonoBehaviour
    {

        [Header("Visual Reference")]
        [SerializeField] protected RodVisualController rodVisualController;

        protected void Start()
        {
            InitializeRod();

            if (RodData.currentEquippedRod != null)
            {
                rodVisualController?.UpdateRodVisual(RodData.currentEquippedRod.rodType);
            }
        }

        protected void InitializeRod()
        {
            if (RodData.currentEquippedRod != null)
            {
                Debug.Log("Loaded equipped rod: " + RodData.currentEquippedRod.strRodName);
                // +++ UPDATE VISUAL ROD +++
                rodVisualController?.UpdateRodVisual(RodData.currentEquippedRod.rodType);
            }
            else
            {
                CreateDefaultRod();
            }
        }

        protected void CreateDefaultRod()
        {
            RodData defaultRod = ScriptableObject.CreateInstance<RodData>();
            defaultRod.rodType = RodType.Default;
            defaultRod.strRodName = "Default Rod";
            defaultRod.intProgressBonus = 18;
            RodData.currentEquippedRod = defaultRod;
            Debug.Log("Default rod created: " + defaultRod.strRodName);
        }

        public RodData GetEquippedRod()
        {
            return RodData.currentEquippedRod;
        }
    }
}