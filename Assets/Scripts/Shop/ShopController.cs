using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Shop
{
    public class ShopController
    {
        private const float PURCHASE_DELAY_SECONDS = 3f;
        
        private static ShopController _instance;
        
        public static ShopController Instance => _instance ??= new ShopController();

        private ShopInventory _inventory;
        
        private ShopController()
        {
        }
        
        public void SetInventory(ShopInventory inventory)
        {
            if (!inventory)
            {
                Debug.LogWarning("[ShopController] Attempting to set null inventory");
                return;
            }
            _inventory = inventory;
        }
        
        public List<Bundle> GetBundles()
        {
            return _inventory ? _inventory.Bundles : new List<Bundle>();
        }
        
        public IEnumerator PurchaseBundleAsync(Bundle bundle, Action onComplete)
        {
            yield return new WaitForSeconds(PURCHASE_DELAY_SECONDS);
            
            bundle.Purchase();
            
            onComplete?.Invoke();
        }
    }
}
