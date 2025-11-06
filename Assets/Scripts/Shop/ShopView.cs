using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Shop
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private Transform _bundleCardsContainer;
        [SerializeField] private BundleCardView _bundleCardPrefab;
        
        private readonly List<BundleCardView> _cardViews = new();
        
        public void Initialize(List<Bundle> bundles, Action<Bundle> onBuyClicked, Action<Bundle> onInfoClicked, Action onResourcesChanged)
        {
            if (bundles == null)
            {
                Debug.LogError("[ShopView] Cannot initialize: bundles list is null", this);
                return;
            }
            
            if (!_bundleCardPrefab)
            {
                Debug.LogError("[ShopView] _bundleCardPrefab is not assigned in Inspector", this);
                return;
            }
            
            ClearCards();
            
            foreach (var bundle in bundles)
            {
                if (!bundle)
                {
                    Debug.LogWarning("[ShopView] Skipping null bundle in list", this);
                    continue;
                }
                
                BundleCardView cardView = Instantiate(_bundleCardPrefab, _bundleCardsContainer);
                
                if (!cardView)
                {
                    Debug.LogWarning($"[ShopView] Failed to instantiate BundleCardView for bundle '{bundle.BundleName}'", this);
                    continue;
                }
                
                cardView.Initialize(bundle, onBuyClicked, onInfoClicked);
                _cardViews.Add(cardView);
            }
            
            InitializeResourceViews(onResourcesChanged);
        }
        
        private void InitializeResourceViews(Action onResourcesChanged)
        {
            ResourceViewInitializationRegistry.Instance.InitializeAll(onResourcesChanged);
        }
        
        public void UpdateCardsState()
        {
            foreach (var cardView in _cardViews)
            {
                if (cardView)
                {
                    cardView.UpdateButtonState();
                }
            }
        }
        
        public BundleCardView[] GetBundleCardViews()
        {
            return _cardViews.ToArray();
        }
        
        private void ClearCards()
        {
            foreach (var cardView in _cardViews)
            {
                if (cardView)
                {
                    Destroy(cardView.gameObject);
                }
            }

            _cardViews.Clear();
        }
    }
}
