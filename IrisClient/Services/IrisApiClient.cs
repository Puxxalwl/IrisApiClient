using IrisClient.Config;
using IrisClient.Constants;
using IrisClient.Extensions;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Threading.Tasks;
using IrisClient.Models;
using System.Collections.Generic;

namespace IrisClient.Services
{
    public class IrisApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AppConfig _config;
        private readonly ILoggerService _logger;
        private readonly string _irisUrl;

        public IrisApiClient(HttpClient httpClient, AppConfig config, ILoggerService logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
            _irisUrl = $"{_config.IrisApi.IrisUrl}/{_config.IrisApi.botId}_{_config.IrisApi.IrisToken}";
        }

        public static IrisApiClient Create(string configPath = "IrisSettings.json")
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: false, reloadOnChange: true)
                .Build();

            var appConfig = configuration.Get<AppConfig>();

            var httpClient = new HttpClient();

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            ILoggerService logger = new LoggerService(loggerFactory.CreateLogger<LoggerService>());

            return new IrisApiClient(httpClient, appConfig!, logger);
        }

        public async Task<Balance> GetBalance()
        {
            try
            {
                var url = $"{_irisUrl}{ApiConstants.Balance}";
                var result = await _httpClient.getWithRetryAsync<Balance>(url);
                return result;
            }
            catch (Exception err)
            {
                _logger.LogError($"Ошибка получения баланса: {err.Message}", err);
                throw;
            }
        }
        public async Task<sweetsFull?> GiveSweets(long userId, float sweets, bool withoutDonateScore, string Comment = "")
        {
            try
            {
                var url = $"{_irisUrl}{ApiConstants.GiveSweets}";

                var Params = new Dictionary<string, object>
                {
                    ["sweets"] = sweets,
                    ["user_id"] = userId,
                    ["comment"] = Comment,
                    ["without_donate_score"] = withoutDonateScore
                };

                var result = await _httpClient.getWithRetryAsync<sweetsFull>(url, Params);
                if (result.result != "ok") return null;
                return result;
            }
            catch (Exception err)
            {
                _logger.LogError($"Ошибка перевода ирисок: {err.Message}", err);
                throw;
            }
        }

        public async Task<Result> OpenBag(bool Status)
        {
            try
            {
                var url = $"{_irisUrl}{(Status == true ? ApiConstants.Open : ApiConstants.Close)}";

                var result = await _httpClient.getWithRetryAsync<Result>(url);
                return result;
            }
            catch (Exception err)
            {
                _logger.LogError($"Ошибка открытия/закрытия мешка: {err.Message}", err);
                throw;
            }
        }
        public async Task<Result> AllowAll(bool Status)
        {
            try
            {
                var url = $"{_irisUrl}{(Status == true ? ApiConstants.AllAllow : ApiConstants.AllDeny)}";

                var result = await _httpClient.getWithRetryAsync<Result>(url);
                return result;
            }
            catch (Exception err)
            {
                _logger.LogError($"Ошибка при изменении статуса переводов для всех: {err.Message}", err);
                throw;
            }

        }

        public async Task<Result> AllowUser(bool Status, long userId)
        {
            try
            {
                var url = $"{_irisUrl}{(Status == true ? ApiConstants.AllowUser : ApiConstants.DenyUser)}";
                var Params = new Dictionary<string, object>
                {
                    ["user_id"] = userId
                };

                var result = await _httpClient.getWithRetryAsync<Result>(url, Params);
                return result;
            }
            catch (Exception err)
            {
                _logger.LogError($"Ошибка при изменении статуса переводов для пользователя (userId: {userId}): {err.Message}", err);
                throw;
            }
        }

    }
}