using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace WOW.DI.Example
{
    public class WeatherRender : MonoBehaviour
    {
        [Inject]
        private IWeatherService weatherService = null;
        [SerializeField]
        private WeatherRenderPrefab prefab = null;

        private async void Awake()
        {
            var result = await weatherService.GetWeatherAsync("130000");
            Debug.Log(result);

            await Task.Delay(1000);

            for(int i = 0; i < 3; i++)
            {
                var renderInstance = this.InjectInstantiate<WeatherRenderPrefab>(prefab, this.transform);
                await Task.Delay(1000);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}