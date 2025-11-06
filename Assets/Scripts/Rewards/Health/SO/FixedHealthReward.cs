using Core;
using UnityEngine;

namespace Health
{
    [CreateAssetMenu(fileName = "FixedHealthReward", menuName = "Bundle/Fixed Health Reward")]
    public class FixedHealthReward : BundleCostReward, IBundleReward
    {
        [SerializeField] private int _healthAmount;
        
        public void Apply()
        {
            HealthController.Instance.AddHealth(_healthAmount);
        }
    }
}
