﻿using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using TranslationManagement.Api.FileParsing.Parsers;

namespace TranslationManagement.Api.FileParsing;

public static class ServiceCollectionExtensions
{
    public static void AddFileParsing(this IServiceCollection services)
    {
        services.AddTransient<IFileParsingProvider, JobParsingProvider>();
        services.AddTransient<IJobParser, TxtJobParser>();
        services.AddTransient<IJobParser, XmlJobParser>();
        services.AddTransient(provider =>
        {
            var parsers = provider.GetServices<IJobParser>();
            return parsers.ToDictionary(parser => parser.SupportedFileType.ToLower());
        });
    }
}