using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shop
{
    public class ShopSceneController : MonoBehaviour
    {
        private const string SCENE_NAME = "BundleDetailScene";

        [SerializeField] private ShopInventory _shopInventory;
        [SerializeField] private ShopView _shopView;

        private void Start()
        {
            ShopController.Instance.SetInventory(_shopInventory);
            var bundles = ShopController.Instance.GetBundles();

            _shopView.Initialize(
                bundles,
                OnBundleBuyClicked,
                OnBundleInfoClicked,
                OnResourcesChanged
            );
        }

        private void OnBundleBuyClicked(Bundle bundle)
        {
            BundleCardView cardView = FindCardViewForBundle(bundle);
            if (cardView)
            {
                cardView.SetProcessing(true);
                StartCoroutine(ShopController.Instance.PurchaseBundleAsync(bundle, () =>
                {
                    cardView.SetProcessing(false);
                    RefreshAllResources();
                    _shopView.UpdateCardsState();
                }));
            }
        }

        private void RefreshAllResources()
        {
            ResourceViewUpdaterRegistry.Instance.RefreshAll();
        }

        private void OnBundleInfoClicked(Bundle bundle)
        {
            BundleDetailSceneData.SelectedBundle = bundle;
            SceneManager.LoadScene(SCENE_NAME);
        }

        private void OnResourcesChanged()
        {
            _shopView.UpdateCardsState();
        }

        private BundleCardView FindCardViewForBundle(Bundle bundle)
        {
            foreach (var cardView in _shopView.GetBundleCardViews())
            {
                if (cardView.GetBundle() == bundle)
                {
                    return cardView;
                }
            }

            return null;
        }
    }
}