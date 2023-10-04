using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOW.DI
{
    [DisallowMultipleComponent]
    public class ProviderApp : Provider
    {
        private static ProviderApp instance = null;
        public static Provider Provider { get => instance as Provider; }

        [SerializeField]
        private ProviderAppInstallerBase[] installers = new ProviderAppInstallerBase[0];

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateInstance()
        {
            if(instance != null)
            {
                return;
            }

            var providers = Resources.LoadAll<ProviderApp>("ProviderApp");
            if(providers.Length < 1)
            {
                var provider = new GameObject(nameof(ProviderApp)).AddComponent<ProviderApp>();
                instance = provider;
            }
            else
            {
                instance = ProviderApp.Instantiate(providers[0]);
            }

            instance.Initialize();

            DontDestroyOnLoad(instance.gameObject);
        }

        public new static T Inject<T>() where T : class
        {
            return Provider.Inject<T>();
        }

        public new static T Inject<T>(object key) where T : class
        {
            return Provider.Inject<T>(key);
        }

        public new static void Remove(object key)
        {
            Provider.Remove(key);
        }

        public new static void Remove<T>()
        {
            Provider.Remove<T>();
        }

        public new static TypeBinder<T> Provide<T>() where T : class
        {
            return Provider.Provide<T>();
        }

        public override void Install()
        {
            base.Install();

            foreach(var installer in installers)
            {
                installer.InstallTo(this);
            }
        }

        private void Initialize()
        {
            this.Install();

            var providers = GameObject.FindObjectsByType<Provider>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var provider in providers)
            {
                if (provider == this)
                {
                    continue;
                }
                provider.Install();
            }

            var monoBehaviours = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var monoBehaviour in monoBehaviours)
            {
                if (monoBehaviour == this)
                {
                    continue;
                }

                Injection.InjectTo(monoBehaviour.GetType(), monoBehaviour);
            }
        }

        protected new void Awake()
        {
            // :)
        }

        private void Start()
        {
            
        }
    }
}