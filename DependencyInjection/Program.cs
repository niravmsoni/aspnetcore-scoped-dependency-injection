using DependencyInjection;
using DependencyInjection.Shared;
using DependencyInjection.Source;
using DependencyInjection.Target;
using DependencyInjection.Transformation.Transformations;
using DependencyInjection.Transformation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Creating default builder for application host
using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<Configuration>();

        services.AddTransient<IPriceParser, PriceParser>();
        services.AddTransient<IProductSource, ProductSource>();

        services.AddTransient<IProductFormatter, ProductFormatter>();
        services.AddTransient<IProductTarget, ProductTarget>();

        services.AddTransient<ProductImporter>();

        services.AddScoped<IImportStatistics, ImportStatistics>();

        //Using these 2 servcies we can understand difference between scoped
        //services.AddTransient<IProductTransformer, ProductTransformer>();
        services.AddTransient<IProductTransformer, ProductTransformerExperiment>();

        services.AddScoped<IProductTransformationContext, ProductTransformationContext>();
        services.AddScoped<INameDecapitaliser, NameDecapitaliser>();
        services.AddScoped<ICurrencyNormalizer, CurrencyNormalizer>();
    })
    .Build();

var productImporter = host.Services.GetRequiredService<ProductImporter>();
productImporter.Run();