using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public abstract class ResourceViewBase : MonoBehaviour, IResourceViewUpdater, IResourceViewInitializable
    {
        [SerializeField] protected TMP_Text _text;
        [SerializeField] protected Button _button;
        
        protected Action _onResourceChanged;
        
        protected virtual void Awake()
        {
            ResourceViewUpdaterRegistry.Instance.Register(this);
            ResourceViewInitializationRegistry.Instance.Register(this);
        }
        
        protected virtual void Start()
        {
            UpdateView();
        }
        
        protected virtual void OnDestroy()
        {
            ResourceViewUpdaterRegistry.Instance.Unregister(this);
            ResourceViewInitializationRegistry.Instance.Unregister(this);
            
            if (_button)
                _button.onClick.RemoveAllListeners();
        }
        
        protected virtual void OnEnable()
        {
            UpdateView();
        }
        
        public void Initialize(Action onResourcesChanged)
        {
            _onResourceChanged = () => ResourceViewCallbackRegistry.Instance.InvokeAll();
            
            if (onResourcesChanged != null)
            {
                ResourceViewCallbackRegistry.Instance.Register(onResourcesChanged);
            }
            
            SetupButton();
        }
        
        public void Refresh()
        {
            UpdateView();
        }

        protected virtual void UpdateView()
        {
            string displayText = GetDisplayText();
            if (_text)
                _text.text = displayText;
        }
        
        protected abstract string GetDisplayText();
        
        protected virtual void SetupButton()
        {
            if (_button)
            {
                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(OnButtonClicked);
            }
        }
        
        protected virtual void OnButtonClicked() { }
    }
}

