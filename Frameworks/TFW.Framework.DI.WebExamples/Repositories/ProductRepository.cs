using System.Linq;
using TFW.Framework.DI.WebExamples.Models;

namespace TFW.Framework.DI.WebExamples.Repositories
{
    public interface IProductRepository
    {
        Product Create(Product product);
        Product Update(Product product);
        Product Delete(int id);
        IQueryable<Product> Get();
    }

    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dataContext;
        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Product Create(Product product)
        {
            return _dataContext.Add(product).Entity;
        }

        public Product Delete(int id)
        {
            return _dataContext.Remove(new Product
            {
                Id = id
            }).Entity;
        }

        public IQueryable<Product> Get()
        {
            return _dataContext.Product;
        }

        public Product Update(Product product)
        {
            return _dataContext.Update(product).Entity;
        }
    }
}
