using Microsoft.Extensions.DependencyInjection;

namespace TranslationManagement.Api.Jobs;

public static class ServiceCollectionExtensions
{
    public static void AddTranslationJobs(this IServiceCollection services)
    {
        services.AddTransient<ITranslationJobService, TranslationJobService>();
        services.AddTransient<TranslationJobMapper>();
    }
}