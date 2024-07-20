using Microsoft.Extensions.DependencyInjection;

namespace TranslationManagement.Api.Translators;

public static class ServiceCollectionExtensions
{
    public static void AddTranslators(this IServiceCollection services)
    {
        services.AddTransient<ITranslatorService, TranslatorService>();
        services.AddTransient<TranslatorMapper>();
    }
}