using MyApp.Core.Domain.Specification;

namespace MyApp.Core.Domain.Example.ProductAgg
{
    /// <summary>
    /// A list of product specifications. You can learn
    /// about Specifications, Enhanced Query Objects or repository methods
    /// reading our Architecture inte and checking the DesignNotes.txt in Domain.Seedwork project
    /// </summary>
    public static class ProductSpecifications
    {
        /// <summary>
        /// The product full text specification
        /// </summary>
        /// <param name="text">the text to find in title or product description</param>
        /// <returns>Associated specification for this criterion</returns>
        public static ISpecification<Product> ProductFullText(string text)
        {
            Specification<Product> fullTextSpecification = new TrueSpecification<Product>();

            if (!string.IsNullOrWhiteSpace(text))
            {

                var left = new DirectSpecification<Product>(p => p.Title.ToLower().Contains(text.ToLower()));
                var right = new DirectSpecification<Product>(p => p.Description.ToLower().Contains(text.ToLower()));

                fullTextSpecification &= new OrSpecification<Product>(left, right);
            }

            return fullTextSpecification;
        }
    }
}
