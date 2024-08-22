using Abdt.Loyal.NoteSaver.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("MigrationSettings.json", false, true)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration.GetConnectionString("NoteConnectionString");

var optionsBuilder = new DbContextOptionsBuilder<NoteContext>();
optionsBuilder.UseNpgsql(connectionString);

var context = new NoteContext(optionsBuilder.Options);
await context.Database.MigrateAsync();
