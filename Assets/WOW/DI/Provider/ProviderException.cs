using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOW.DI
{
    public class ProviderException : System.Exception
    {
        public ProviderException(string message) : base(message)
        {
        }
    }

    public class ProviderTypeNotFoundException : System.Exception
    {
        public ProviderTypeNotFoundException(string message) : base(message)
        {

        }
    }

    public class ProviderKeyNotFoundException : System.Exception
    {
        public ProviderKeyNotFoundException(string message) : base(message)
        {

        }
    }
}