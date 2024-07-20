using Microsoft.Extensions.DependencyInjection;
using TranslationManagement.Api.Jobs.Persistence;

namespace TranslationManagement.Api.Jobs;

public static class ServiceCollectionExtensions
{
    public static void AddTranslationJobs(this IServiceCollection services)
    {
        services.AddTransient<ITranslationJobService, TranslationJobService>();
        services.AddTransient<ITranslationJobRepository, TranslationJobRepository>();
        services.AddTransient<TranslationJobMapper>();
    }
}