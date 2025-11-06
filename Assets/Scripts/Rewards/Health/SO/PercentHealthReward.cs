using Core;
using UnityEngine;

namespace Health
{
    [CreateAssetMenu(fileName = "PercentHealthReward", menuName = "Bundle/Percent Health Reward")]
    public class PercentHealthReward : BundleCostReward, IBundleReward
    {
        [SerializeField] [Range(0f, 1000f)] private float _healthPercent;
        
        public void Apply()
        {
            int currentHealth = HealthController.Instance.GetCurrentHealth();
            int amountToAdd = Mathf.RoundToInt(currentHealth * _healthPercent / 100f);
            HealthController.Instance.AddHealth(amountToAdd);
        }
    }
}
