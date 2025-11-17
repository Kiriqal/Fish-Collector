using UnityEngine;
using UnityEngine.UI;

namespace Anoa
{
    public class FishCounterController : MonoBehaviour
    {
        [SerializeField] protected Text textFishCounter;
        [SerializeField] protected int intMaxFishCount = 5; // BISA EDIT DI INSPECTOR
        protected int intCurrentFishCount;

        protected void Start()
        {
            UpdateCounter();
        }

        public void AddFish()
        {
            intCurrentFishCount++;
            UpdateCounter();
        }

        protected void UpdateCounter()
        {
            textFishCounter.text = intCurrentFishCount + "/" + intMaxFishCount;
        }

        public bool IsCollectionComplete()
        {
            return intCurrentFishCount >= intMaxFishCount;
        }

        public int GetCurrentFishCount()
        {
            return intCurrentFishCount;
        }

        public void ResetCounter()
        {
            intCurrentFishCount = 0;
            UpdateCounter();
        }
    }
}