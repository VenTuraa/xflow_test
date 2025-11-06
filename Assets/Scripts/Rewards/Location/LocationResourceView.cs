using Core;
using UnityEngine;

namespace Location
{
    public class LocationResourceView : ResourceViewBase
    {
        protected override string GetDisplayText()
        {
            string currentLocation = LocationController.Instance.GetCurrentLocation();
            return $"{currentLocation}";
        }
        
        protected override void OnButtonClicked()
        {
            LocationController.Instance.ResetToDefault();
            Refresh();
            _onResourceChanged?.Invoke();
        }
    }
}
