using DependencyInjection.Model;

namespace DependencyInjection.Transformation
{
    /// <summary>
    /// Responsible for maintaining state of Product object.
    /// </summary>
    public class ProductTransformationContext : IProductTransformationContext
    {
        //Used to capture initial state of Product object
        private Product? _initialProduct;

        //Used to apply transformations to Product object
        private Product? _product;

        public Product GetProduct()
        {
            if (_product == null)
            {
                throw new InvalidOperationException("Cant get the product before setting it");
            }

            return _product;
        }

        /// <summary>
        /// Comparing _product (Object after applying transformations - If Any) and _initialProduct.
        /// If no transformations are applied, they will be the same
        /// </summary>
        /// <returns></returns>
        public bool IsProductChanged()
        {
            if (_product == null || _initialProduct == null)
            {
                return false;
            }

            return !_initialProduct.Equals(_product);
        }

        /// <summary>
        /// We are setting value to _initialProduct ONLY when _initialProduct is NULL 
        /// </summary>
        /// <param name="product"></param>
        public void SetProduct(Product product)
        {
            _product = product;

            if (_initialProduct == null)
            {
                _initialProduct = product;
            }
        }
    }
}
