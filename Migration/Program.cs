using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Migration.DbContexts.Rnd;
using Migration.DbContexts;
using Sc.Models;
using Migration.MigrationServices.FromRnd;
using System;

var services = new ServiceCollection();
services
    .AddDbContext<ScDbContext>(o =>
    {
        o.UseNpgsql(DbConfigurations.ScContext,
        e => e.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    })
    .AddDbContext<NacidRndContext>(o =>
    {
        o.UseSqlServer(DbConfigurations.RndContext,
        e => e.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    });

services.AddScoped<RndInstitutionsMigration>();
services.AddScoped<RndComplexesMigration>();

ServiceProvider serviceProvider = services.BuildServiceProvider();

Console.WriteLine($"Migration starts ! Time: {DateTime.Now}");

try
{
    var rndMigrationService = serviceProvider.GetService<RndInstitutionsMigration>();
    var rndComplexesMigrationService = serviceProvider.GetService<RndComplexesMigration>();

    rndMigrationService.Start();
    rndComplexesMigrationService.Start();

}
catch (Exception exception)
{
    while (exception.InnerException != null) { exception = exception.InnerException; }
    var exceptionInfo = $"\nDate: {DateTime.Now}\nType: {exception.GetType().FullName}\nMessage: {exception.Message}\nStackTrace: {exception.StackTrace}\n";
    Console.WriteLine(exceptionInfo);
}
finally
{
    Console.WriteLine($"\nMigration finished ! Time: {DateTime.Now}\n");
    Console.ReadLine();
}