using UnityEngine;
using UnityEngine.UI;

namespace Anoa
{
    public class FishCounterController : MonoBehaviour
    {
        [SerializeField] protected Text textFishCounter;
        protected int intCurrentFishCount;
        protected int intMaxFishCount = 30;

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
    }
}