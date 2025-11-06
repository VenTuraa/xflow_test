using System;
using Core;
using UnityEngine;

namespace VIP
{
    [CreateAssetMenu(fileName = "VIPCost", menuName = "Bundle/VIP Cost")]
    public class VipCost : BundleCostReward, IBundleCost
    {
        [SerializeField] private int _days;
        [SerializeField] private int _hours;
        [SerializeField] private int _minutes;
        [SerializeField] private int _seconds;
        
        public bool IsCanBuy()
        {
            TimeSpan time = new TimeSpan(_days, _hours, _minutes, _seconds);
            return VIPController.Instance.HasEnoughVIPTime(time);
        }
        
        public void Apply()
        {
            TimeSpan time = new TimeSpan(_days, _hours, _minutes, _seconds);
            VIPController.Instance.RemoveVIPTime(time);
        }
    }
}
