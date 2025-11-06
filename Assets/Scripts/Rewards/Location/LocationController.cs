using Core;

namespace Location
{
    public class LocationController
    {
        private static LocationController _instance;
        
        public static LocationController Instance => _instance ??= new LocationController();

        private readonly LocationData _data;
        
        private LocationController()
        {
            _data = PlayerData.Instance.GetData<LocationData>();
        }
        
        public string GetCurrentLocation()
        {
            return _data.CurrentLocation;
        }
        
        public void SetLocation(string location)
        {
            _data.CurrentLocation = location;
        }
        
        public void ResetToDefault()
        {
            _data.CurrentLocation = "Forest";
        }
    }
}
