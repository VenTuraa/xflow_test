using System;
using System.Collections.Generic;

namespace Core
{
    public class ResourceViewInitializationRegistry
    {
        private static ResourceViewInitializationRegistry _instance;

        public static ResourceViewInitializationRegistry Instance =>
            _instance ??= new ResourceViewInitializationRegistry();

        private readonly List<IResourceViewInitializable> _initializables = new();

        private ResourceViewInitializationRegistry()
        {
        }

        public void Register(IResourceViewInitializable initializable)
        {
            if (initializable != null && !_initializables.Contains(initializable))
                _initializables.Add(initializable);
        }

        public void Unregister(IResourceViewInitializable initializable)
        {
            _initializables.Remove(initializable);
        }

        public void InitializeAll(Action onResourcesChanged)
        {
            foreach (IResourceViewInitializable initialize in _initializables)
                initialize?.Initialize(onResourcesChanged);
        }
    }

    public interface IResourceViewInitializable
    {
        void Initialize(Action onResourcesChanged);
    }
}