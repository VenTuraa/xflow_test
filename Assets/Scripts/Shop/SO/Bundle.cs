using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "Bundle", menuName = "Shop/Bundle")]
    public class Bundle : ScriptableObject
    {
        [Header("Bundle Info")]
        [Tooltip("Название бандла, которое будет отображаться в UI")]
        [SerializeField] private string _bundleName = "New Bundle";
        
        [Header("Costs")]
        [SerializeField] private List<BundleCostReward> _costs = new();
        
        [Header("Rewards")]
        [SerializeField] private List<BundleCostReward> _rewards = new();
        
        public string BundleName => _bundleName;
        
        public bool CanAfford()
        {
            foreach (var cost in _costs)
            {
                if (cost is not IBundleCost bundleCost) continue;
                if (!bundleCost.IsCanBuy())
                {
                    return false;
                }
            }
            return true;
        }
        
        public void Purchase()
        {
            if (!CanAfford())
            {
                Debug.LogWarning($"[Bundle] Cannot purchase '{BundleName}': insufficient resources");
                return;
            }
            
            foreach (var cost in _costs)
            {
                if (cost is IBundleCost bundleCost)
                {
                    bundleCost.Apply();
                }
            }
            
            foreach (var reward in _rewards)
            {
                if (reward is IBundleReward bundleReward)
                {
                    bundleReward.Apply();
                }
            }
        }
    }
}
