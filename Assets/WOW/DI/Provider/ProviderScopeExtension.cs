using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WOW.DI
{
    public static class ProviderScopeExtension
    {
        public static IProvider[] GetProviderComponents(this Behaviour behaviour)
        {
            var scopes = behaviour.GetComponentsInParent<Provider>();

            if (scopes.Length < 1)
            {
                Debug.Assert(ProviderApp.Provider != null);
                return new[] { ProviderApp.Provider };
            }

            Array.Resize(ref scopes, scopes.Length + 1);
            scopes[scopes.Length - 1] = ProviderApp.Provider;
            return scopes;
        }

        public static Provider.TypeBinder<T> Provide<T>(this Behaviour obj) where T : class
        {
            var component = obj.GetComponent<Provider>();
            if(component == null)
            {
                component = obj.gameObject.AddComponent<Provider>();
            }

            return component.Provide<T>();
        }

        public static object Inject(this Behaviour behaviour, System.Type type)
        {
            var providers = GetProviderComponents(behaviour);

            foreach(var provider in providers)
            {
                try
                {
                    return provider.Inject(type);
                }
                catch (ProviderKeyNotFoundException)
                {
                    continue;
                }
                catch (ProviderTypeNotFoundException)
                {
                    continue;
                }
            }

            throw new ProviderException("Provider not found.");
        }

        public static object Inject(this Behaviour behaviour, System.Type type, object key)
        {
            var providers = GetProviderComponents(behaviour);

            foreach (var provider in providers)
            {
                try
                {
                    return provider.Inject(type, key);
                }
                catch (ProviderKeyNotFoundException)
                {
                    continue;
                }
                catch (ProviderTypeNotFoundException)
                {
                    continue;
                }
            }

            throw new ProviderException("Provider not found.");
        }

        public static T Inject<T>(this Behaviour behaviour) where T : class
        {
            var providers = GetProviderComponents(behaviour);

            foreach (var provider in providers)
            {
                try
                {
                    return provider.Inject<T>();
                }
                catch (ProviderKeyNotFoundException)
                {
                    continue;
                }
                catch (ProviderTypeNotFoundException)
                {
                    continue;
                }
            }

            throw new ProviderException("Provider not found.");
        }

        public static T Inject<T>(this Behaviour behaviour, object key) where T : class
        {
            var providers = GetProviderComponents(behaviour);

            foreach (var provider in providers)
            {
                try
                {
                    return provider.Inject<T>(key);
                }
                catch (ProviderKeyNotFoundException)
                {
                    continue;
                }
                catch (ProviderTypeNotFoundException)
                {
                    continue;
                }
            }

            throw new ProviderException("Provider not found.");
        }

        public static T InjectInstantiate<T>(this Behaviour behaviour, T prefab) where T : Behaviour
        {
            var providers = GetProviderComponents(behaviour);
            foreach (var provider in providers)
            {
                try
                {
                    return provider.InjectInstantiate<T>(prefab);
                }
                catch (ProviderKeyNotFoundException)
                {
                    continue;
                }
                catch (ProviderTypeNotFoundException)
                {
                    continue;
                }
            }
            throw new ProviderException("Provider not found.");
        }

        public static T InjectInstantiate<T>(this Behaviour behaviour, T prefab, Transform parent) where T : Behaviour
        {
            var providers = GetProviderComponents(behaviour);
            foreach (var provider in providers)
            {
                try
                {
                    return provider.InjectInstantiate<T>(prefab, parent);
                }
                catch (ProviderKeyNotFoundException)
                {
                    continue;
                }
                catch (ProviderTypeNotFoundException)
                {
                    continue;
                }
            }
            throw new ProviderException("Provider not found.");
        }
    }
}