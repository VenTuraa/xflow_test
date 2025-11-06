using Core;

namespace Gold
{
    public class GoldController
    {
        private static GoldController _instance;
        
        public static GoldController Instance => _instance ??= new GoldController();

        private readonly GoldData _data;
        
        private GoldController()
        {
            _data = PlayerData.Instance.GetData<GoldData>();
        }
        
        public int GetCurrentGold()
        {
            return _data.CurrentGold;
        }
        
        public void AddGold(int amount)
        {
            _data.CurrentGold += amount;
        }
        
        public void RemoveGold(int amount)
        {
            _data.CurrentGold -= amount;
            if (_data.CurrentGold < 0)
                _data.CurrentGold = 0;
        }
        
        public bool HasEnoughGold(int amount)
        {
            return _data.CurrentGold >= amount;
        }
        
        public void SetGold(int gold)
        {
            _data.CurrentGold = gold;
        }
    }
}
