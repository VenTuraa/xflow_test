using Core;
using UnityEngine;

namespace Gold
{
    [CreateAssetMenu(fileName = "FixedGoldCost", menuName = "Bundle/Fixed Gold Cost")]
    public class FixedGoldCost : BundleCostReward, IBundleCost
    {
        [SerializeField] private int _goldAmount;
        
        public bool IsCanBuy()
        {
            return GoldController.Instance.HasEnoughGold(_goldAmount);
        }
        
        public void Apply()
        {
            GoldController.Instance.RemoveGold(_goldAmount);
        }
    }
}
