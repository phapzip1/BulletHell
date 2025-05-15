using UnityEngine;

namespace Phac.Utility
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T s_Instance;

        public static bool HasInstance => s_Instance != null;
        public static T TryGetInstance() => HasInstance ? s_Instance : null;

        public static T Instance
        {
            // Lazy loading
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindAnyObjectByType<T>();
                    if (s_Instance == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name + "Auto-Generated");
                        s_Instance = go.AddComponent<T>();
                    }
                }

                return s_Instance;
            }
        }

        /// <summary>
        /// Make sure to call base.Awake() in override if you need awake.
        /// </summary>
        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;

            s_Instance = this as T;
        }

    }

    /// <summary>
    /// Persistent Regulator singleton, will destroy any other older components of the same type it finds on awake
    /// </summary>
    public class RegulatorSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T s_Instance;

        public static bool HasInstance => s_Instance != null;

        public float InitializationTime { get; private set; }

        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindAnyObjectByType<T>();
                    if (s_Instance == null)
                    {
                        var go = new GameObject(typeof(T).Name + " Auto-Generated");
                        go.hideFlags = HideFlags.HideAndDontSave;
                        s_Instance = go.AddComponent<T>();
                    }
                }

                return s_Instance;
            }
        }

        /// <summary>
        /// Make sure to call base.Awake() in override if you need awake.
        /// </summary>
        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;
            InitializationTime = Time.time;
            DontDestroyOnLoad(gameObject);

            T[] oldInstances = FindObjectsByType<T>(FindObjectsSortMode.None);
            foreach (T old in oldInstances)
            {
                if (old.GetComponent<RegulatorSingleton<T>>().InitializationTime < InitializationTime)
                {
                    Destroy(old.gameObject);
                }
            }

            if (s_Instance == null)
            {
                s_Instance = this as T;
            }
        }
    }

    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        public bool AutoUnparentOnAwake = true;

        protected static T s_Instance;

        public static bool HasInstance => s_Instance != null;
        public static T TryGetInstance() => HasInstance ? s_Instance : null;

        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindAnyObjectByType<T>();
                    if (s_Instance == null)
                    {
                        var go = new GameObject(typeof(T).Name + " Auto-Generated");
                        s_Instance = go.AddComponent<T>();
                    }
                }

                return s_Instance;
            }
        }

        /// <summary>
        /// Make sure to call base.Awake() in override if you need awake.
        /// </summary>
        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;

            if (AutoUnparentOnAwake)
            {
                transform.SetParent(null);
            }

            if (s_Instance == null)
            {
                s_Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (s_Instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}