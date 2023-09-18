using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace WOW.DI.Example
{
    public class WeatherServiceTest : IWeatherService
    {
        public async Task<Weather> GetWeatherAsync(string id)
        {
            await Task.Delay(1);
            return new Weather(
                $"テスト気象台({id})",
                DateTime.Now.ToString(),
                $"テスト地域({id})",
                $"これは、{id}のテストです。",
                $"これは、{id}のテストです。");
        }
    }
}
