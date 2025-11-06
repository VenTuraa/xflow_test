using Core;

namespace Health
{
    public class HealthController
    {
        private static HealthController _instance;

        public static HealthController Instance => _instance ??= new HealthController();

        private readonly HealthData _data;

        private HealthController()
        {
            _data = PlayerData.Instance.GetData<HealthData>();
        }

        public int GetCurrentHealth()
        {
            return _data.CurrentHealth;
        }

        public void AddHealth(int amount)
        {
            _data.CurrentHealth += amount;
        }

        public void RemoveHealth(int amount)
        {
            _data.CurrentHealth -= amount;
            if (_data.CurrentHealth < 0)
            {
                _data.CurrentHealth = 0;
            }
        }

        public bool HasEnoughHealth(int amount)
        {
            return _data.CurrentHealth > 0 && _data.CurrentHealth >= amount;
        }

        public void SetHealth(int health)
        {
            _data.CurrentHealth = health;
        }
    }
}