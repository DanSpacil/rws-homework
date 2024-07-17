using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Jobs;
using TranslationManagement.Api.Translators;

namespace TranslationManagement.Api.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TranslationJob> TranslationJobs { get; set; }
    public DbSet<TranslatorModel> Translators { get; set; }
}