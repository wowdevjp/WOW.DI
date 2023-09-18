using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOW.DI.Example
{
    public class WeatherRenderPrefab : MonoBehaviour
    {
        [Inject]
        private IWeatherService weatherService = null;

        private void Start()
        {
            weatherService.GetWeatherAsync("130000").ContinueWith((task) =>
            {
                Debug.Log(task.Result);
            });
        }
    }
}
