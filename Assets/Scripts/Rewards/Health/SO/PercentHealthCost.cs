using Core;
using UnityEngine;

namespace Health
{
    [CreateAssetMenu(fileName = "PercentHealthCost", menuName = "Bundle/Percent Health Cost")]
    public class PercentHealthCost : BundleCostReward, IBundleCost
    {
        [SerializeField] [Range(0f, 100f)] private float _healthPercent;
        
        public bool IsCanBuy()
        {
            int currentHealth = HealthController.Instance.GetCurrentHealth();
            int amountToRemove = Mathf.RoundToInt(currentHealth * _healthPercent / 100f);
            return HealthController.Instance.HasEnoughHealth(amountToRemove);
        }
        
        public void Apply()
        {
            int currentHealth = HealthController.Instance.GetCurrentHealth();
            int amountToRemove = Mathf.RoundToInt(currentHealth * _healthPercent / 100f);
            HealthController.Instance.RemoveHealth(amountToRemove);
        }
    }
}
