using Core;

namespace Gold
{
    public class GoldResourceView : ResourceViewBase
    {
        protected override string GetDisplayText()
        {
            int currentGold = GoldController.Instance.GetCurrentGold();
            return $"{currentGold}";
        }
        
        protected override void OnButtonClicked()
        {
            GoldController.Instance.AddGold(100);
            Refresh();
            _onResourceChanged?.Invoke();
        }
    }
}
