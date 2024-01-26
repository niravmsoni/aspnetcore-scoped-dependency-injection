using DependencyInjection.Model;
using DependencyInjection.Shared;
using DependencyInjection.Transformation.Transformations;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Transformation
{
    /// <summary>
    /// Core service that is responsible for applying transformations
    /// </summary>
    public class ProductTransformerExperiment : IProductTransformer
    {
        private readonly IProductTransformationContext _productTransformationContext;
        private readonly INameDecapitaliser _nameDecapitaliser;
        private readonly ICurrencyNormalizer _currencyNormalizer;
        private readonly IImportStatistics _importStatistics;

        public ProductTransformerExperiment(IProductTransformationContext productTransformationContext,
            INameDecapitaliser nameDecapitaliser,
            ICurrencyNormalizer currencyNormalizer,
            IImportStatistics importStatistics)
        {
            _productTransformationContext = productTransformationContext;
            _nameDecapitaliser = nameDecapitaliser;
            _currencyNormalizer = currencyNormalizer;
            _importStatistics = importStatistics;
        }
        public Product ApplyTransformations(Product product)
        {   
            // Setting product in context
            _productTransformationContext.SetProduct(product);

            _nameDecapitaliser.Execute();
            _currencyNormalizer.Execute();

            // Check if prodyct has changed, if so increment count
            if (_productTransformationContext.IsProductChanged())
            {
                _importStatistics.IncrementTransformationCount();
            }

            // Return product from context
            return _productTransformationContext.GetProduct();
        }
    }
}
