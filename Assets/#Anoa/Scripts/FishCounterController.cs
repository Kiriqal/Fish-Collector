using UnityEngine;
using UnityEngine.UI;

namespace Anoa
{
    public class FishCounterController : MonoBehaviour
    {
        [SerializeField] protected Text textFishCounter;
        [SerializeField] protected int intMaxFishCount = 5;

        // BUAT STATIC UNTUK PERSISTENCE
        protected static int intCurrentFishCountStatic;

        protected void Start()
        {
            // +++ GUNAKAN STATIC VALUE +++
            intCurrentFishCountStatic = Mathf.Clamp(intCurrentFishCountStatic, 0, intMaxFishCount);
            UpdateCounter();
        }

        public void AddFish()
        {
            intCurrentFishCountStatic++;
            UpdateCounter();
        }

        protected void UpdateCounter()
        {
            textFishCounter.text = intCurrentFishCountStatic + "/" + intMaxFishCount;
        }

        public bool IsCollectionComplete()
        {
            return intCurrentFishCountStatic >= intMaxFishCount;
        }

        public int GetCurrentFishCount()
        {
            return intCurrentFishCountStatic;
        }

        public void ResetCounter()
        {
            intCurrentFishCountStatic = 0;
            UpdateCounter();
        }
    }
}