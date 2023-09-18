using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace WOW.DI.Example
{
    public interface IWeatherService
    {
        Task<Weather> GetWeatherAsync(string id);
    }
}