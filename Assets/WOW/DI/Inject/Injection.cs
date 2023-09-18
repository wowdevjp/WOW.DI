using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.VersionControl;
using UnityEngine;

namespace WOW.DI
{
    public static class Injection
    {
        public static void InjectToFields<T>(System.Type type, T target, IProvider provider) where T : Behaviour
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
        }

        public static void InjectToFields<T>(System.Type type, T target) where T : Behaviour
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
        }
    }
}