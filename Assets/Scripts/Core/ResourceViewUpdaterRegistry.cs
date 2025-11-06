using System.Collections.Generic;

namespace Core
{
    public class ResourceViewUpdaterRegistry
    {
        private static ResourceViewUpdaterRegistry _instance;
        
        public static ResourceViewUpdaterRegistry Instance => _instance ??= new ResourceViewUpdaterRegistry();

        private List<IResourceViewUpdater> _updaters = new();
        
        private ResourceViewUpdaterRegistry()
        {
        }
        
        public void Register(IResourceViewUpdater updater)
        {
            if (updater != null && !_updaters.Contains(updater))
                _updaters.Add(updater);
        }
        
        public void Unregister(IResourceViewUpdater updater)
        {
            _updaters.Remove(updater);
        }
        
        public void RefreshAll()
        {
            foreach (var updater in _updaters)
                updater?.Refresh();
        }
    }
}
