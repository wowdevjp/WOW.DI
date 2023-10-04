using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;

namespace WOW.DI
{
    public static class Injection
    {
        public static void InjectTo<T>(System.Type type, T target, IProvider provider) where T : Behaviour
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<InjectAttribute>();
                if (attribute != null)
                {
                    if(attribute.Key != null)
                    {
                        field.SetValue(target, provider.Inject(field.FieldType, attribute.Key));
                    }
                    else
                    {
                        field.SetValue(target, provider.Inject(field.FieldType));
                    }
                }
            }

            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<InjectAttribute>();
                if (attribute != null)
                {
                    if (attribute.Key != null)
                    {
                        var args = method.GetParameters().Select(p => provider.Inject(p.ParameterType, attribute.Key)).ToArray();
                        method.Invoke(target, args);
                    }
                    else
                    {
                        var args = method.GetParameters().Select(p => provider.Inject(p.ParameterType)).ToArray();
                        method.Invoke(target, args);
                    }
                }
            }
        }

        public static void InjectTo<T>(System.Type type, T target) where T : Behaviour
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<InjectAttribute>();
                if (attribute != null)
                {
                    if (attribute.Key != null)
                    {
                        field.SetValue(target, target.Inject(field.FieldType, attribute.Key));
                    }
                    else
                    {
                        field.SetValue(target, target.Inject(field.FieldType));
                    }
                }
            }

            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach(var method in methods)
            {
                var attribute = method.GetCustomAttribute<InjectAttribute>();
                if (attribute != null)
                {
                    if (attribute.Key != null)
                    {
                        var args = method.GetParameters().Select(p => target.Inject(p.ParameterType, attribute.Key)).ToArray();
                        method.Invoke(target, args);
                    }
                    else
                    {
                        var args = method.GetParameters().Select(p => target.Inject(p.ParameterType)).ToArray();
                        method.Invoke(target, args);
                    }
                }
            }
        }
    }
}