using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOW.DI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class InjectAttribute : Attribute
    {
        public object Key { get; private set; } = null;

        public InjectAttribute() { }
        public InjectAttribute(object key)
        {
            Key = key;
        }
    }
}