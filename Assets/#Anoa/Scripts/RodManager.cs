using UnityEngine;
using System.Collections.Generic;

namespace Anoa
{
    public class RodManager : MonoBehaviour
    {
        protected void Start()
        {
            InitializeRod();
        }

        protected void InitializeRod()
        {
            // +++ GUNAKAN DATA DARI STATIC +++
            if (RodData.currentEquippedRod != null)
            {
                Debug.Log("Loaded equipped rod: " + RodData.currentEquippedRod.strRodName);
            }
            else
            {
                // +++ BUAT DEFAULT ROD JIKA BELUM ADA +++
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