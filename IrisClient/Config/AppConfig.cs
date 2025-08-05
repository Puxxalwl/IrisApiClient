using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace IrisClient.Config
{
    public class AppConfig
    {
        public IrisApiConfig IrisApi { get; set; } = new IrisApiConfig();

        public static AppConfig LoadFromJson(string configPath = "IrisSettings.json")
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: false, reloadOnChange: true)
                .Build();

            var appConfig = new AppConfig();
            config.Bind(appConfig);

            appConfig.IrisApi.IrisUrl = appConfig.IrisApi.IrisUrl ?? "https://iris-tg.ru/api";

            if (string.IsNullOrEmpty(appConfig.IrisApi.botId) || string.IsNullOrEmpty(appConfig.IrisApi.IrisToken))
            {
                throw new InvalidOperationException("Не указаны параметры botId и IrisToken в IrisSetting.json.");
            }

            return appConfig;
        }
    }

    public class IrisApiConfig
    {
        public string IrisUrl { get; set; } = "https://iris-tg.ru/api";
        public string botId { get; set; } = string.Empty;
        public string IrisToken { get; set; } = string.Empty;
    }
}

