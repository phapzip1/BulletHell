using System;
using UnityEngine;
using UnityEngine.Events;

namespace Phac.Utility
{
    public class Observer<T>
    {
        [SerializeField] private T m_Value;
        [SerializeField] UnityEvent<T> m_OnValueChanged;

        public T Value
        {
            get => m_Value;
            set => Set(value);
        }

        public Observer(T value, UnityAction<T> callback = null)
        {
            m_Value = value;
            m_OnValueChanged = new UnityEvent<T>();
            if (callback != null) m_OnValueChanged.AddListener(callback);
        }

        public void AddListener(UnityAction<T> callback)
        {
            if (callback == null) return;
            if (m_OnValueChanged == null) m_OnValueChanged = new UnityEvent<T>();
            m_OnValueChanged.AddListener(callback);
        }

        public void RemoveListener(UnityAction<T> callback)
        {
            if (callback == null || m_OnValueChanged == null) return;
            m_OnValueChanged.RemoveListener(callback);
        }

        public void RemoveAllListener() => m_OnValueChanged?.RemoveAllListeners();

        public void Dispose()
        {
            RemoveAllListener();
            m_OnValueChanged = null;
            m_Value = default;
        }

        private void Set(T value)
        {
            if (Equals(value, m_Value)) return;
            m_Value = value;
            Invoke();
        }

        private void Invoke() => m_OnValueChanged?.Invoke(m_Value);

    }
}