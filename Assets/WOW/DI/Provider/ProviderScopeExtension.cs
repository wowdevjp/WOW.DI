using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WOW.DI.Utility;
using System.Linq;

namespace WOW.DI
{
    public static class ProviderScopeExtension
    {
        public static IProvider[] GetProviderComponents(this Behaviour behaviour)
        {
            var scopes = behaviour.GetComponentsInParent<Provider>();

            var rootObjects = SceneUtilityDI.GetRootGameObjects();
            var rootScopes = rootObjects.SelectMany(r => r.GetComponents<Provider>()).Distinct().ToArray();

            scopes = scopes.Concat(rootScopes).Concat(new[] { ProviderApp.Provider }).ToArray();

            if (scopes.Length < 1)
            {
                Debug.Assert(ProviderApp.Provider != null);
                return new[] { ProviderApp.Provider };
            }

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

            throw new ProviderException($"Provider not found. {type.Name}");
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

            throw new ProviderException($"Provider not found. {type.Name}");
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

            throw new ProviderException($"Provider not found. {typeof(T).Name}");
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

            throw new ProviderException($"Provider not found. {typeof(T).Name}");
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
            throw new ProviderException($"Provider not found. {typeof(T).Name}");
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
            throw new ProviderException($"Provider not found. {typeof(T).Name}");
        }
    }
}