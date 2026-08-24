using Microsoft.Extensions.DependencyInjection;
using TransactionAggregation.Application.Abstractions;
using TransactionAggregation.Application.DataSources;
using TransactionAggregation.Application.Services;
using TransactionAggregation.Infrastructure.DataSources;

namespace TransactionAggregation.Infrastructure;

// Registers all services and data sources so the app can build them.
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register the three data sources.
        services.AddScoped<ITransactionDataSource, CoreBankingDataSource>();
        services.AddScoped<ITransactionDataSource, CardProcessorDataSource>();
        services.AddScoped<ITransactionDataSource, WalletDataSource>();

        // Register the categorization engine and the aggregator.
        services.AddScoped<ICategorizationEngine, CategorizationEngine>();
        services.AddScoped<TransactionAggregator>();

        // Register the query service the API will use.
        services.AddScoped<IAggregationService, AggregationQueryService>();

        return services;
    }
}