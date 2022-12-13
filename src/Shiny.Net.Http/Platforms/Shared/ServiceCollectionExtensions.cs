﻿using Microsoft.Extensions.DependencyInjection;
using Shiny.Net.Http;

namespace Shiny;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpTransfers<TDelegate>(this IServiceCollection services)
        where TDelegate : class, IHttpTransferDelegate
    {
        services.AddConnectivity();
        //services.AddShinyService<HttpTransferManager>();
        services.AddShinyService(typeof(TDelegate));
        //services.AddRepository<HttpTransferStoreConverter, HttpTransfer>();
        return services;
    }
}