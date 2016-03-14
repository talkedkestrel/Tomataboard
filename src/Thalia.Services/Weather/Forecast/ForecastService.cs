﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Newtonsoft.Json;
using Thalia.Services.Extensions;
using Thalia.Services.Locations;

namespace Thalia.Services.Weather.Forecast
{
    /// <summary>
    /// https://developer.forecast.io/docs/v2
    /// </summary>
    public class ForecastService : IForecastService
    {
        #region Private Fields
        private readonly IOptions<ForecastKeys> _keys;
        private readonly ILogger<ForecastService> _logger;
        #endregion

        /// <summary>
        /// 1000 calls a day
        /// </summary>
        public Quota Quota => new Quota() { Requests = 1000, Time = TimeSpan.FromDays(1) };
        public TimeSpan? Expiration => TimeSpan.FromHours(1);

        #region Constructors
        public ForecastService(ILogger<ForecastService> logger, IOptions<ForecastKeys> keys)
        {
            _logger = logger;
            _keys = keys;
        }
        #endregion

        private string GetQueryString(Location location)
        {
            // Forecast allows geographical search only
            return  $"{_keys.Value.ConsumerKey}/{location.Latitude},{location.Longitude}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">serialized Location</param>
        /// <returns></returns>
        public async Task<WeatherConditions> Execute(string parameters)
        {
            var location = JsonConvert.DeserializeObject<Location>(parameters);
            if (location == null)
            {
                _logger.LogError($"{GetType().Name}: Cannot get weather for '{parameters}'. Cannot deserialize parameters");
                return null;
            }

            try
            {
                var url = "https://api.forecast.io/forecast/";

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(new Uri(url + GetQueryString(location), UriKind.Absolute));
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var weatherDto = JsonConvert.DeserializeObject<WeatherDto>(content);
                        if (weatherDto == null) return null;

                        if ((weatherDto.Code == null) && (weatherDto.currently != null))
                        {
                            return GetResult(content);
                        }

                        _logger.LogError($"{GetType().Name}: Cannot get weather for '{parameters}'. Code: {weatherDto.Code}, Message: {weatherDto.Message}, Content: {content}");
                    }

                    _logger.LogError($"{GetType().Name}: Cannot get weather for '{parameters}'. Status code: {response.StatusCode} Content: {content}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}: Cannot get weather for '{parameters}'. Exception: " + ex.GetError());
            }

            return null;
        }

        private WeatherConditions GetResult(string json)
        {
            var weatherDto = JsonConvert.DeserializeObject<WeatherDto>(json);
            if (weatherDto == null) return null;

            var weatherConditions = new WeatherConditions()
            {
                Title = weatherDto.currently.summary,
                Description = weatherDto.currently.summary,
                TemperatureC = (int)Math.Ceiling(TemperatureConverter.FahrenheitToCelsius(weatherDto.currently.temperature)),
                TemperatureF = (int)Math.Ceiling(weatherDto.currently.temperature),
                Icon = Icons.GetCssClass(weatherDto.currently.icon)
            };
            return weatherConditions;
        }
    }
}