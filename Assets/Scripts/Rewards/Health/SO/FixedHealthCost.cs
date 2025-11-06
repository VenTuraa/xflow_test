using Core;
using UnityEngine;

namespace Health
{
    [CreateAssetMenu(fileName = "FixedHealthCost", menuName = "Bundle/Fixed Health Cost")]
    public class FixedHealthCost : BundleCostReward, IBundleCost
    {
        [SerializeField] private int _healthAmount;
        
        public bool IsCanBuy()
        {
            return HealthController.Instance.HasEnoughHealth(_healthAmount);
        }
        
        public void Apply()
        {
            HealthController.Instance.RemoveHealth(_healthAmount);
        }
    }
}
