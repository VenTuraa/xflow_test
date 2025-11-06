using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "ShopInventory", menuName = "Shop/Shop Inventory")]
    public class ShopInventory : ScriptableObject
    {
        [SerializeField] private List<Bundle> _bundles = new();
        
        public List<Bundle> Bundles => _bundles;
    }
}
