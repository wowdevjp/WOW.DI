using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WOW.DI
{
    [DisallowMultipleComponent]
    public class Provider : MonoBehaviour, IProvider
    {
        public class TypeBinder<InterfaceT> where InterfaceT : class
        {
            private Provider provider = null;

            public TypeBinder(Provider provider)
            {
                Provider.ThrowExceptionIsNotInterface<InterfaceT>();

                this.provider = provider;
            }

            public T AsSingle<T>() where T : class
            {
                return this.provider.AddSingle<InterfaceT, T>();
            }

            public T AsSingle<T>(params object[] args) where T : class
            {
                return this.provider.AddSingle<InterfaceT, T>(args);
            }

            public T WithKey<T>(object key) where T : class
            {
                return this.provider.AddWithKey<T>(key);
            }

            public T WithKey<T>(object key, params object[] args) where T : class
            {
                return this.provider.AddWithKey<T>(key, args);
            }
        }

        private Dictionary<object, object> keyAssignedInstances = new Dictionary<object, object>();
        private Dictionary<System.Type, object> singleInstances = new Dictionary<System.Type, object>();
        private bool isInstalled = false;
        
        protected static void ThrowExceptionIsNotInterface(System.Type interfaceType)
        {
            if(interfaceType.IsInterface == false)
            {
                throw new ProviderException($"{nameof(interfaceType)} is not interface.");
            }
        }

        protected static void ThrowExceptionIsNotInterface<T>()
        {
            ThrowExceptionIsNotInterface(typeof(T));
        }

        protected static void ThrowExceptionIsNotImplemented(System.Type interfaceType, System.Type instanceType)
        {
            ThrowExceptionIsNotInterface(interfaceType);

            if(interfaceType.IsAssignableFrom(instanceType) == false)
            {
                throw new ProviderException($"{nameof(instanceType)} is not implement {nameof(interfaceType)}.");
            }
        }

        protected static void ThrowExceptionIsNotImplemented<InterfaceT, InstanceT>()
        {
            ThrowExceptionIsNotInterface<InterfaceT>();
            ThrowExceptionIsNotImplemented(typeof(InterfaceT), typeof(InstanceT));
        }

        public virtual TypeBinder<T> Provide<T>() where T : class
        {
            ThrowExceptionIsNotInterface<T>();
            return new TypeBinder<T>(this);
        }

        public virtual object Inject(System.Type type)
        {
            ThrowExceptionIsNotInterface(type);
            if (singleInstances.ContainsKey(type))
            {
                return singleInstances[type];
            }
            throw new ProviderTypeNotFoundException($"{nameof(type)} is not provided.");
        }

        public virtual object Inject(System.Type type, object key)
        {
            if (keyAssignedInstances.ContainsKey(key))
            {
                if (keyAssignedInstances[key].GetType().GetInterfaces().Contains(type) == false)
                {
                    throw new ProviderTypeNotFoundException($"Key: {key}({type.ToString()}) is not provided.");
                }

                return keyAssignedInstances[key];
            }
            throw new ProviderKeyNotFoundException($"Key: {key} is not provided.");
        }

        public virtual T Inject<T>() where T : class
        {
            ThrowExceptionIsNotInterface<T>();

            if (singleInstances.ContainsKey(typeof(T)))
            {
                return singleInstances[typeof(T)] as T;
            }

            throw new ProviderTypeNotFoundException($"{nameof(T)} is not provided.");
        }

        public virtual T Inject<T>(object key) where T : class
        {
            if (keyAssignedInstances.ContainsKey(key))
            {
                return keyAssignedInstances[key] as T;
            }
            throw new ProviderKeyNotFoundException($"Key: {key} is not provided.");
        }

        public virtual void Remove(object key)
        {
            if(keyAssignedInstances.ContainsKey(key))
            {
                keyAssignedInstances.Remove(key);
            }
            else
            {
                throw new ProviderKeyNotFoundException($"Key: {key} is not provided.");
            }
        }

        public virtual void Remove<T>()
        {
            if (singleInstances.ContainsKey(typeof(T)))
            {
                singleInstances.Remove(typeof(T));
            }
            else
            {
                throw new ProviderKeyNotFoundException($"{nameof(T)} is not provided.");
            }
        }

        public T InjectInstantiate<T>(T behaviour, Transform parent) where T : Behaviour
        {
            var instance = Instantiate(behaviour, parent);
            Injection.InjectTo(typeof(T), instance);
            return instance;
        }

        public T InjectInstantiate<T>(T behaviour) where T : Behaviour
        {
            return InjectInstantiate(behaviour, this.transform);
        }

        protected InstanceT AddSingle<InstanceT>(System.Type interfaceType, params object[] args) where InstanceT : class
        {
            ThrowExceptionIsNotImplemented(interfaceType, typeof(InstanceT));

            if (singleInstances.ContainsKey(interfaceType))
            {
                throw new ProviderException($"{nameof(interfaceType)} already exists.");
            }

            var instance = System.Activator.CreateInstance(typeof(InstanceT), args);
            singleInstances[interfaceType] = instance;

            return instance as InstanceT;
        }

        private InstanceT AddSingle<InterfaceT, InstanceT>(params object[] args) where InterfaceT : class where InstanceT : class
        {
            return AddSingle<InstanceT>(typeof(InterfaceT), args);
        }

        private InstanceT AddSingle<InterfaceT, InstanceT>() where InterfaceT : class where InstanceT : class
        {
            ThrowExceptionIsNotImplemented<InterfaceT, InstanceT>();

            if (singleInstances.ContainsKey(typeof(InterfaceT)))
            {
                throw new ProviderException($"{nameof(InterfaceT)} already exists.");
            }

            var instance = System.Activator.CreateInstance(typeof(InstanceT));
            singleInstances[typeof(InterfaceT)] = instance;
            return instance as InstanceT;
        }

        private T AddWithKey<T>(object key, params object[] args) where T : class
        {
            if(keyAssignedInstances.ContainsKey(key))
            {
                throw new ProviderException($"Key: {key} already exists.");
            }

            var instance = System.Activator.CreateInstance(typeof(T), args);
            keyAssignedInstances.Add(key, instance);
            return instance as T;
        }

        private T AddWithKey<T>(object key) where T : class
        {
            if (keyAssignedInstances.ContainsKey(key))
            {
                throw new ProviderException($"Key: {key} already exists.");
            }

            var instance = System.Activator.CreateInstance(typeof(T));
            keyAssignedInstances.Add(key, instance);
            return instance as T;
        }

        protected void DisposeKeyAssignedInstances()
        {
            foreach(var instance in keyAssignedInstances.Values)
            {
                if(instance is System.IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            keyAssignedInstances.Clear();
        }

        protected void DisposeSingletons()
        {
            foreach (var instance in singleInstances.Values)
            {
                if (instance is System.IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            singleInstances.Clear();
        }

        public virtual void Install()
        {
            isInstalled = true;
        }

        protected void Awake()
        {
            var providers = GetComponents<Provider>();
            if(providers.Length > 1)
            {
                throw new ProviderException($"Provider is already installed.");
            }
        }

        protected void Start()
        {
            if (!isInstalled)
            {
                Install();
            }
        }

        protected void OnDestroy()
        {
            DisposeKeyAssignedInstances();
            DisposeSingletons();
        }
    }
}