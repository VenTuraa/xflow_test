using Core;
using UnityEngine;

namespace Health
{
    public class HealthResourceView : ResourceViewBase
    {
        protected override string GetDisplayText()
        {
            int currentHealth = HealthController.Instance.GetCurrentHealth();
            return $"{currentHealth}";
        }
        
        protected override void OnButtonClicked()
        {
            HealthController.Instance.AddHealth(50);
            Refresh();
            _onResourceChanged?.Invoke();
        }
    }
}
