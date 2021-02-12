
using UnityEngine;

namespace MyMarmot.Tools
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {

        private static T instance;
        public static T GetInstance
        {
            get => instance;
        }

        public static bool IsInit
        {
            get => GetInstance != null;
        }
        protected virtual void Awake()
        {
            instance = this as T;
        }



    }
}
