using System;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class BundleCardView : MonoBehaviour
    {
        private const string PROCESSING = "Обработка...";
        private const string BUY = "Купить";

        [SerializeField] private TMP_Text _bundleNameText;
        [SerializeField] private TMP_Text _buyButtonText;

        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _infoButton;

        private Bundle _bundle;
        private Action<Bundle> _onBuyClicked;
        private Action<Bundle> _onInfoClicked;

        private bool _isProcessing;
        private Action _onResourcesChangedCallback;

        public void Initialize(Bundle bundle, Action<Bundle> onBuyClicked, Action<Bundle> onInfoClicked)
        {
            if (bundle == null)
            {
                Debug.LogError("[BundleCardView] Cannot initialize: bundle is null", this);
                return;
            }
            
            if (_buyButton == null)
            {
                Debug.LogError("[BundleCardView] _buyButton is not assigned in Inspector", this);
                return;
            }
            
            if (_onResourcesChangedCallback != null)
                ResourceViewCallbackRegistry.Instance.Unregister(_onResourcesChangedCallback);

            _bundle = bundle;
            _onBuyClicked = onBuyClicked;
            _onInfoClicked = onInfoClicked;

            if (_bundleNameText)
                _bundleNameText.text = bundle.BundleName;

            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(() =>
            {
                if (!_isProcessing && _bundle && _bundle.CanAfford())
                    _onBuyClicked?.Invoke(_bundle);
            });

            if (_infoButton != null)
            {
                _infoButton.onClick.RemoveAllListeners();
                _infoButton.onClick.AddListener(() => { _onInfoClicked?.Invoke(_bundle); });
            }

            _onResourcesChangedCallback = UpdateButtonState;
            ResourceViewCallbackRegistry.Instance.Register(_onResourcesChangedCallback);

            UpdateButtonState();
        }

        public void UpdateButtonState()
        {
            if (!_buyButton) return;

            bool canAfford = _bundle && _bundle.CanAfford();
            _buyButton.interactable = canAfford && !_isProcessing;
        }

        public void SetProcessing(bool isProcessing)
        {
            _isProcessing = isProcessing;

            if (_buyButtonText)
                _buyButtonText.text = _isProcessing ? PROCESSING : BUY;

            UpdateButtonState();
        }

        public void ShowInfoButton(bool show)
        {
            if (_infoButton)
                _infoButton.gameObject.SetActive(show);
        }

        public Bundle GetBundle()
        {
            return _bundle;
        }

        private void OnDisable()
        {
            if (_onResourcesChangedCallback != null)
                ResourceViewCallbackRegistry.Instance.Unregister(_onResourcesChangedCallback);
        }

        private void OnDestroy()
        {
            if (_onResourcesChangedCallback != null)
            {
                ResourceViewCallbackRegistry.Instance.Unregister(_onResourcesChangedCallback);
                _onResourcesChangedCallback = null;
            }

            if (_buyButton)
                _buyButton.onClick.RemoveAllListeners();

            if (_infoButton)
                _infoButton.onClick.RemoveAllListeners();
        }
    }
}