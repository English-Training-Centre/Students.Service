using FluentValidation;
using FluentValidation.AspNetCore;
using Npgsql;
using Students.Service.src.Application.Interfaces;
using Students.Service.src.Infrastructure.Persistence;
using System.IO.Compression;
using Polly;
using Grpc.Net.Compression;
using Libs.Core.Internal.src.Interfaces;
using Libs.Core.Internal.Protos.UserService;
using Libs.Core.Public.src.Interfaces;

namespace Students.Service.src.Configuration;

public static class MiddlewareConfig
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var logger = LoggerFactory.Create(s => s.AddConsole()).CreateLogger<Program>();
        try
        {
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddFluentValidationAutoValidation();

            services.AddSingleton(nd =>
            {
                var builder = new NpgsqlDataSourceBuilder(
                    PostgresDB.BuildConnectionStringFromEnvironment()
                );
                builder.UseLoggerFactory(nd.GetRequiredService<ILoggerFactory>());
                return builder.Build();
            });

            services.AddHealthChecks()
                    .AddNpgSql(nd => nd.GetRequiredService<NpgsqlDataSource>());

            services.AddScoped<IPostgresDB, PostgresDB>();

            services.AddGrpc();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while configuring Middleware");
        }
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        //app.MapGrpcService<HResourceGrpcService>();

        return app;
    }
}