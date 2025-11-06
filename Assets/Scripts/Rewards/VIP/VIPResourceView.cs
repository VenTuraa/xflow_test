using System;
using Core;
using TMPro;
using UnityEngine;

namespace VIP
{
    public class VIPResourceView : ResourceViewBase
    {
        protected override void UpdateView()
        {
            string displayText = GetDisplayText();
            if (_text)
            {
                _text.text = displayText;
            }
        }

        protected override string GetDisplayText()
        {
            TimeSpan vipDuration = VIPController.Instance.GetVIPDuration();
            int totalSeconds = (int)vipDuration.TotalSeconds;
            return $"{totalSeconds} сек";
        }
        
        protected override void OnButtonClicked()
        {
            VIPController.Instance.AddVIPTime(new TimeSpan(0, 0, 30));
            Refresh();
            _onResourceChanged?.Invoke();
        }
    }
}


