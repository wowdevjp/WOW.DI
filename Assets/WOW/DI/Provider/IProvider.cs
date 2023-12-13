using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOW.DI
{
    public interface IProvider
    {
        object Inject(System.Type type);
        object Inject(System.Type type, object key);
        T Inject<T>() where T : class;
        T Inject<T>(object key) where T : class;
        T InjectInstantiate<T>(T behaviour) where T : Behaviour;
        T InjectInstantiate<T>(T behaviour, Transform parent) where T : Behaviour;
    }
}