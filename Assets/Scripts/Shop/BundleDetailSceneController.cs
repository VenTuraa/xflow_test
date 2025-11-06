using System;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shop
{
    public class BundleDetailSceneController : MonoBehaviour
    {
        private const string SHOP_SCENE = "ShopScene";
        
        [SerializeField] private Transform _bundleCardContainer;
        [SerializeField] private BundleCardView _bundleCardPrefab;
        [SerializeField] private UnityEngine.UI.Button _backButton;
        
        private Bundle _selectedBundle;
        private BundleCardView _cardView;
        
        private void Start()
        {
            _selectedBundle = BundleDetailSceneData.SelectedBundle;
            
            if (!_selectedBundle)
            {
                Debug.LogError("No bundle selected for detail scene!");
                return;
            }
            
            _cardView = Instantiate(_bundleCardPrefab, _bundleCardContainer);
            
            if (_cardView)
            {
                _cardView.Initialize(_selectedBundle, OnBundleBuyClicked, null);
                _cardView.ShowInfoButton(false);
            }
            
            _backButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SHOP_SCENE);
            });
            
            InitializeResourceViews(OnResourcesChanged);
        }
        
        private void InitializeResourceViews(Action onResourcesChanged)
        {
            ResourceViewInitializationRegistry.Instance.InitializeAll(onResourcesChanged);
        }
        
        private void OnBundleBuyClicked(Bundle bundle)
        {
            if (!_cardView) return;
            _cardView.SetProcessing(true);
            StartCoroutine(ShopController.Instance.PurchaseBundleAsync(bundle, () =>
            {
                _cardView.SetProcessing(false);
                RefreshAllResources();
                _cardView.UpdateButtonState();
            }));
        }
        
        private void RefreshAllResources()
        {
            ResourceViewUpdaterRegistry.Instance.RefreshAll();
        }
        
        private void OnResourcesChanged()
        {
            if (_cardView)
            {
                _cardView.UpdateButtonState();
            }
        }
    }
}
