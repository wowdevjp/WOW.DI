using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOW.DI
{
    [RequireComponent(typeof(ProviderApp))]
    public abstract class ProviderAppInstallerBase : MonoBehaviour
    {
        public abstract void InstallTo(ProviderApp provider);
    }
}