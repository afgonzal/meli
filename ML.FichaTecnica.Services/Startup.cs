﻿using System;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ML.FichaTecnica.Services
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration config, string runningPath)
        {
            ML.FichaTecnica.Stats.DataLayer.Startup.ConfigureServices(services, config, runningPath);

            services.AddAutoMapper(typeof(EntitiesMappingProfile));

            //use http factory to reuse httpclient
            services.AddHttpClient<IMeliClient, MeliClientService>(client =>
            {
                client.BaseAddress = new Uri(config["MLApiBaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).SetHandlerLifetime(TimeSpan.FromMinutes(10));

            

            services.AddSingleton<INumbersToLanguage, NumerosLeiblesService>();
            services.AddScoped<IStatsService, StatsService>();
            services.AddScoped<IFichaTecnicaService, FichaTecnicaService>();
        }
    }
}
