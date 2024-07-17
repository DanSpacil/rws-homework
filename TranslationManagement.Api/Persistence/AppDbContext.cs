﻿using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Jobs;
using TranslationManagement.Api.Translators;

namespace TranslationManagement.Api.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TranslationJobController.TranslationJob> TranslationJobs { get; set; }
    public DbSet<TranslatorManagementController.TranslatorModel> Translators { get; set; }
}