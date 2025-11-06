using System;
using System.Collections.Generic;

namespace Core
{
    public class PlayerData
    {
        private static PlayerData _instance;
        
        public static PlayerData Instance => _instance ??= new PlayerData();

        private readonly Dictionary<Type, object> _data = new();
        
        public T GetData<T>() where T : class, new()
        {
            Type type = typeof(T);
            if (!_data.ContainsKey(type))
            {
                _data[type] = new T();
            }
            return _data[type] as T;
        }

        public void SetData<T>(T data) where T : class
        {
            _data[typeof(T)] = data;
        }
        
        private PlayerData()
        {
        }
    }
}
