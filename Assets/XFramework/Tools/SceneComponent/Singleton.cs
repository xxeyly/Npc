using UnityEngine;

namespace XFramework
{
    /// <summary>
    /// 通用单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                return _instance;
            }
        }

        protected virtual void Start()
        {
        }

        protected virtual void Awake()
        {
        }
    }
}