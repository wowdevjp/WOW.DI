using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace WOW.DI.Example
{
    public class WeatherServiceJMA : IWeatherService
    {
        public async Task<Weather> GetWeatherAsync(string id)
        {
            string url = $"https://www.jma.go.jp/bosai/forecast/data/overview_forecast/{id}.json";

            var request = UnityWebRequest.Get(url);
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Delay(1);
            }

            return JsonUtility.FromJson<Weather>(request.downloadHandler.text.Trim('[', ']'));
        }
    }
}