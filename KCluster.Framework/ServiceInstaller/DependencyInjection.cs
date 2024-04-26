﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using KCluster.Framework.Extensions;

namespace KCluster.Framework.ServiceInstaller;

public static class DependencyInjection
{
    public static IServiceCollection InstallServices(this IServiceCollection services,
        IConfiguration configuration, params Assembly[] assemblies)
    {
        return services.InstallServices(configuration, assemblies.ToList());
    }

    public static IServiceCollection InstallServices(this IServiceCollection services,
        IConfiguration configuration, List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.CanInstantiate() == false || type.IsAssignableTo(typeof(IServiceInstaller)) == false)
                {
                    continue;
                }

                var installer = Activator.CreateInstance(type).SmartCast<IServiceInstaller>();

                installer?.Install(services, configuration);
            }
        }

        return services;
    }
}