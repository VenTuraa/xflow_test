using System;
using System.Collections.Generic;

namespace Core
{
    public class ResourceViewCallbackRegistry
    {
        private static ResourceViewCallbackRegistry _instance;
        
        public static ResourceViewCallbackRegistry Instance => _instance ??= new ResourceViewCallbackRegistry();

        private readonly List<Action> _callbacks = new();
        
        private ResourceViewCallbackRegistry()
        {
        }
        
        public void Register(Action callback)
        {
            if (callback != null && !_callbacks.Contains(callback))
            {
                _callbacks.Add(callback);
            }
        }
        
        public void Unregister(Action callback)
        {
            if (callback != null)
            {
                _callbacks.Remove(callback);
            }
        }
        
        public void InvokeAll()
        {
            foreach (var callback in _callbacks)
            {
                callback?.Invoke();
            }
        }
    }
}
