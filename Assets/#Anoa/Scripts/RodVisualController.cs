using UnityEngine;

namespace Anoa
{
    public class RodVisualController : MonoBehaviour
    {
        [Header("Rod Models")]
        [SerializeField] protected GameObject defaultRodModel;
        [SerializeField] protected GameObject ironRodModel;
        [SerializeField] protected GameObject goldRodModel;
        [SerializeField] protected GameObject diamondRodModel;

        public void UpdateRodVisual(RodType rodType)
        {
            // Nonaktifkan semua model
            defaultRodModel.SetActive(false);
            ironRodModel.SetActive(false);
            goldRodModel.SetActive(false);
            diamondRodModel.SetActive(false);

            // Aktifkan model sesuai rod type
            switch (rodType)
            {
                case RodType.Default: defaultRodModel.SetActive(true); break;
                case RodType.Iron: ironRodModel.SetActive(true); break;
                case RodType.Gold: goldRodModel.SetActive(true); break;
                case RodType.Diamond: diamondRodModel.SetActive(true); break;
            }
        }
    }
}