using System;
using Core;
using UnityEngine;

namespace VIP
{
    [CreateAssetMenu(fileName = "VIPReward", menuName = "Bundle/VIP Reward")]
    public class VIPReward : BundleCostReward, IBundleReward
    {
        [SerializeField] private int _days;
        [SerializeField] private int _hours;
        [SerializeField] private int _minutes;
        [SerializeField] private int _seconds;
        
        public void Apply()
        {
            TimeSpan time = new TimeSpan(_days, _hours, _minutes, _seconds);
            VIPController.Instance.AddVIPTime(time);
        }
    }
}
