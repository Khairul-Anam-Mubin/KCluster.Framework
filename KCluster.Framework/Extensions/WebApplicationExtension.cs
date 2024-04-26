﻿using KCluster.Framework.Interfaces;
using KCluster.Framework.ORM.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace KCluster.Framework.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication DoCreateIndexes(this WebApplication application, bool doCreateIndexes = false)
    {
        if (doCreateIndexes == false)
        {
            doCreateIndexes = application.Configuration.GetConfig<bool>("EnableIndexCreation");
        }

        if (doCreateIndexes)
        {
            var indexCreators = application.Services.GetServices<IIndexCreator>().ToList();

            Console.WriteLine($"Index Creation Started. IndexCreator Found : {indexCreators.Count}");

            indexCreators.ForEach(indexCreator =>
            {
                indexCreator.CreateIndexes();
            });
        }
        return application;
    }

    public static WebApplication DoMigration(this WebApplication application, bool doMigration = false)
    {
        if (doMigration == false)
        {
            doMigration = application.Configuration.GetConfig<bool>("EnableMigration");
        }

        if (doMigration)
        {
            var enabledMigrationJobs = application.Configuration.GetConfig<List<string>>("MigrationJobs");

            if (enabledMigrationJobs is null)
            {
                return application;
            }

            using var scope = application.Services.CreateScope();

            var migrationJobs = scope.ServiceProvider.GetServices<IMigrationJob>()
                .Where(job => enabledMigrationJobs.Contains(job.GetType().Name))
                .ToList();

            // enabled entry order wise migrations..
            enabledMigrationJobs.ForEach(enabledMigrationJob =>
            {
                var migrationJob = migrationJobs.FirstOrDefault(x => x.GetType().Name == enabledMigrationJob);

                if (migrationJob is null) return;

                migrationJob.MigrateAsync().Wait();
            });
        }

        return application;
    }

    public static WebApplication StartInitialServices(this WebApplication app)
    {
        var initialServices = app.Services.GetServices<IInitialService>().ToList();

        Console.WriteLine($"Start Running Initial Services. Initial Services found: {initialServices.Count}\n");

        foreach (var initialService in initialServices)
        {
            try
            {
                initialService.InitializeAsync().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return app;
    }
}