using System;
using Core;

namespace VIP
{
    public class VIPController
    {
        private static VIPController _instance;
        
        public static VIPController Instance => _instance ??= new VIPController();
        
        private readonly VIPData _data;
        
        private VIPController()
        {
            _data = PlayerData.Instance.GetData<VIPData>();
        }
        
        public TimeSpan GetVIPDuration()
        {
            return _data.VIPDuration;
        }
        
        public void AddVIPTime(TimeSpan time)
        {
            _data.VIPDuration = _data.VIPDuration.Add(time);
        }
        
        public void RemoveVIPTime(TimeSpan time)
        {
            _data.VIPDuration = _data.VIPDuration.Subtract(time);
            if (_data.VIPDuration < TimeSpan.Zero)
            {
                _data.VIPDuration = TimeSpan.Zero;
            }
        }
        
        public bool HasEnoughVIPTime(TimeSpan time)
        {
            return _data.VIPDuration >= time;
        }
        
        public void SetVIPDuration(TimeSpan duration)
        {
            _data.VIPDuration = duration;
        }
    }
}
