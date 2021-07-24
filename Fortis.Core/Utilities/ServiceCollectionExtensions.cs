namespace KnowBe4.Core.Utilities
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Scrutor;
    // using System.Net.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Builder;
    using FluentMigrator.Runner;
    using FluentMigrator.Runner.Initialization;
    using FluentMigrator.Runner.Processors;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Spi;
    using KnowBe4.Core.Migrations;
    using KnowBe4.Core.Services;
    using KnowBe4.Core.Contexts;
    using KnowBe4.Core.Repositories;
    // using Polly;
    // using System;
    // using NLog;

    public static class ServiceCollectionExtensions
    {
        // private static ILogger nlog = NLog.LogManager.GetCurrentClassLogger();

        // public static void RegisterHttpClients(this IServiceCollection services)
        // {
        //     services.AddHttpClient("chaseclient")
        //         .ConfigurePrimaryHttpMessageHandler(() =>
        //         {
        //             return new HttpClientHandler()
        //             {
        //                 UseCookies = false
        //             };
        //         })
        //         .AddTransientHttpErrorPolicy(x => x.RetryAsync(3))
        //         .AddTransientHttpErrorPolicy(x => x.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
        // }
        //this is for database Migrations
        // public static IApplicationBuilder Migrate(this IApplicationBuilder app)
        // {
        //     using var scope = app.ApplicationServices.CreateScope();
        //     var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
        //     runner.ListMigrations();
        //     runner.MigrateUp();
        //     return app;
        // }

        public static void RegisterApplicationDependencies(this IServiceCollection services)
        {
            //Scrutor Auto assembly scanning and registeration
            //Will use this for IJob implementations
            // services.Scan(scan => scan
            //     .FromCallingAssembly()
            //         .AddClasses()
            //         .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            //         .AsSelfWithInterfaces()
            //         .WithSingletonLifetime()
            //     .FromAssemblyOf<IJob>()
            //         .AddClasses(classes => classes.AssignableTo<IJob>())
            //         .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            //         .AsSelf()
            //         .WithSingletonLifetime()
            // );
            services.Scan(scan => scan
                .FromCallingAssembly()
                    .AddClasses(classes => classes.AssignableTo<IJob>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime()
            );
            services.AddSingleton<ILogs, Logs>();
            services.AddSingleton<ICrypto, Crypto>();
            services.AddSingleton<ISettings, Settings>();
            services.AddSingleton<IGlobal, Global>();
            services.AddScoped<ICachingProvider,MemoryCacher>();
            services.AddSingleton<IRazorService, RazorService>();
            services.AddSingleton<IJobFactory,SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory,StdSchedulerFactory>();
            services.AddSingleton<IJobs,Jobs>();
            services.AddSingleton<ISettingsCommandText,SettingsCommandText>();
            services.AddSingleton<IImpersonate,Impersonate>();
            services.AddSingleton<IPasswordRepository,SecretServerApiV1>();
            services.AddSingleton<IADExtensions,ADExtensions>();
            services.AddSingleton<IKronosCommandText,KronosCommandText>();
            services.AddScoped<IKnowBe4Api,KnowBe4Api>();
            services.AddScoped<IKronosApi,KronosApi>();
            // services.AddScoped<IChaseApi,ChaseApiV2>();
            // services.AddScoped<IChaseService,ChaseService>();
            //Workaround for AddHostedService registering Scheduler as Transient
            services.AddSingleton<IExcelRepository,ExcelRepository>();
            services.AddSingleton<SchedulerHostedService>();
            services.AddSingleton<ISchedulers>(p => p.GetService<SchedulerHostedService>());
            services.AddSingleton<IHostedService>(p => p.GetService<SchedulerHostedService>());
            // services.AddHostedService<SchedulerHostedService>();
            //For Debugging Scrutor
            // foreach (var service in services)
            // {
            //     nlog.Info("Service " + service.ServiceType.FullName + " loaded with lifetime of " + service.Lifetime + " and instance of " + service.ImplementationType?.FullName);
            // }
            //Settings Migrations
            var sp = services.AddFluentMigratorCore()
                .ConfigureRunner(c => 
                    c.AddSQLite()
                    .ScanIn(typeof(IntializeSettingsDB).Assembly).For.Migrations()
                ).Configure<RunnerOptions>(o =>
                    o.Tags = new [] { "settings" }
                ).BuildServiceProvider(false);
            sp.CreateScope().ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            //First initiate and construct the connection strings
            var settings = services.BuildServiceProvider().GetRequiredService<ISettings>();
            string a = settings.GetSettingValue("dbserver"),
                b = settings.GetSettingValue("dbdefaultdatabase"),
                c = settings.GetSettingValue("dbtrusted"),
                d = settings.GetSettingValue("dbusername"),
                h = settings.GetSettingValue("dbpassword"),
                t = settings.GetSettingValue("dbtype"),
                e = "",
                f = d.IndexOf('\\') != -1 ? d.Split('\\')[0] : "",
                g = d.IndexOf('\\') != -1 ? d.Split('\\')[1] : d;

                switch (t.ToLower())
                {
                    case "sqlite":
                        e = "Data Source=|DataDirectory|/" + b + ".s3db;datetimeformat=CurrentCulture";
                        break;
                    case "sqlserver":
                        e = "server=" + a + ";database=" + b + ";" + "User ID=" + d + ";Password=" + h + ";" + "Trusted_Connection=" + (c=="0" ? "False" : "True") + ";";
                        break;
                    default:
                        e = "";
                        break;
                }

            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>(factory => new DbConnectionFactory(e,t));
            services.AddTransient<IUserCommandText,UserCommandText>();
            services.AddTransient<IGroupCommandText,GroupCommandText>();
            services.AddTransient<IUserGroupCommandText,UserGroupCommandText>();
            services.AddTransient<IRiskScoreCommandText,RiskScoreCommandText>();
            services.AddTransient<IRiskScoreRepository,RiskScoreRepository>();
            services.AddTransient<IGroupRepository,GroupRepository>();
            services.AddTransient<IUserGroupRepository,UserGroupRepository>();
            services.AddTransient<IUserRepository,UserRepository>();
            services.AddTransient<IKnowBe4Service,KnowBe4Service>();

            var sc = new ServiceCollection();
            var sp = sc.AddFluentMigratorCore()
            .ConfigureRunner(c => 
                {
                    if (t == "sqlite")
                    {
                        c.AddSQLite()
                        .WithGlobalConnectionString(e)
                        .ScanIn(typeof(IntializeDB).Assembly).For.Migrations();
                    }
                    else if (t == "sqlserver")
                    {
                        c.AddSqlServer()
                        .WithGlobalConnectionString(e)
                        .ScanIn(typeof(IntializeDB).Assembly).For.Migrations();

                    }
                }
            ).Configure<RunnerOptions>(o => 
                o.Tags = new [] { "data" }
            ).BuildServiceProvider(false);
            using (var scope = sp.CreateScope())
            {
                //first get the Impersonate service
                if (t == "sqlserver")
                {
                    var imp = services.BuildServiceProvider(false).GetRequiredService<IImpersonate>();
                    imp.RunAsImpersonated(() => {
                        scope.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
                        return true;
                    },g,h,f);
                }
                else if (t == "sqlite")
                {
                    scope.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
                }
            }
        }
    }
}