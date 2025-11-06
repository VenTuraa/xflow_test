using Core;
using UnityEngine;

namespace Gold
{
    [CreateAssetMenu(fileName = "FixedGoldReward", menuName = "Bundle/Fixed Gold Reward")]
    public class FixedGoldReward : BundleCostReward, IBundleReward
    {
        [SerializeField] private int _goldAmount;
        
        public void Apply()
        {
            GoldController.Instance.AddGold(_goldAmount);
        }
    }
}
