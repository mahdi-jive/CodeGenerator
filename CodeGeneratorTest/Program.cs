using CodeGenerator;
using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.NetFx48;
using CodeGenerator.Assembly.Template.NetTiers.Entities;
using CodeGenerator.FileSystem.Abstractions;
using CodeGenerator.FileSystem.Physical;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static IHost host;
    private static IMediator Mediator;
    private static void RegistryService(string[] args)
    {
        host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddMediatR(cfg =>
   cfg.RegisterServicesFromAssemblies(
       typeof(IAssembly).Assembly,
       typeof(IDirectory).Assembly,
       typeof(AssemblyNetFx48).Assembly,
       typeof(Program).Assembly
   ));
    })
    .Build();
        Mediator = host.Services.GetRequiredService<IMediator>();
    }
    private static async Task Main(string[] args)
    {

        RegistryService(args);

        await testGenerateProjectAsync();
        //GetDatabaseModel();
        await host.StopAsync();
        host.Dispose();
    }

    private static async Task testGenerateProjectAsync()
    {
        string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var directory = await PhysicalDirectory.CreateRootAsync(Mediator, rootPath, "TestAssembly");
        //IAssembly behsaz_Entities = await AssemblyNetFx48.CreateAsync(Mediator, directory, "Behsaz.Entities");

        //ICodeFile codeFile = new CodeFile("EntityBase.cs", a);
        //await behsaz_Entities.Directory.AddSourceFileAsync(behsaz_Entities, codeFile);
        List<IGenerator> generators = new List<IGenerator>() { await EntitiesFactory.GenerateAssembly(Mediator, directory, "Entities") };
        var generationManager = new CodeGenerationManager(generators);
        await generationManager.GenerateAllAsync();

    }
}