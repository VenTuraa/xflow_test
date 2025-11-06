using Core;
using UnityEngine;

namespace Location
{
    [CreateAssetMenu(fileName = "LocationReward", menuName = "Bundle/Location Reward")]
    public class LocationReward : BundleCostReward, IBundleReward
    {
        [SerializeField] private string _locationName;
        
        public void Apply()
        {
            LocationController.Instance.SetLocation(_locationName);
        }
    }
}
