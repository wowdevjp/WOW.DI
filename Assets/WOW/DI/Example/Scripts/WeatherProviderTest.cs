using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOW.DI.Example
{
    public class WeatherProviderTest : Provider
    {
        public override void Install()
        {
            base.Install();
            Provide<IWeatherService>().AsSingle<WeatherServiceTest>();
        }
    }
}